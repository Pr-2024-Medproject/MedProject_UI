using MedProject_UI.Helpers;
using MedProject_UI.Models;
using MedProject_UI.Services;
using MedProject_UI.View.UserControls;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static MedProject_UI.App;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObservableCollection<DataItem> _data;
    private CollectionViewSource _collectionViewSource;

    private MongoDbService _mongoService;
    private ObservableCollection<Patient> Patients { get; set; } = new();

    public MainWindow()
    {
        InitializeComponent();
        InitializeMongo();

        SearchBar.TextChanged += RouteViewerOpened;
        SearchBar.PreviewTextInput += PreviewTextSearch;

        btnAddNewPatient.btnClick += AddPatientBtnClick;
    }


    private void GetListDocument(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button?.DataContext is not Patient selectedPatient)
            return;

        var patientDescription = new PatientDescription(selectedPatient);
        patientDescription.ShowDialog();
    }

    private void AddPatientBtnClick(object sender, RoutedEventArgs e)
    {
        var addPatient = new AddPatient();
        addPatient.ShowDialog();
    }

    private void RouteViewerOpened(object sender, TextChangedEventArgs e)
    {
        var textBox = (sender as ClearableSearchBar)?.tbSearchText;
        if (textBox == null) return;
        var filterText = textBox.Text.Trim();

        _collectionViewSource.View.Filter = item =>
        {
            if (item is DataItem dataItem)
            {
                var fullName = $"{dataItem._colFirstName} {dataItem._colMiddleName} {dataItem._colLastName}".ToLower();
                var searchTerms = filterText.ToLower().Split(' ');

                if (searchTerms.Length == 1)
                    // Check if the single search term matches any of the fields
                    return dataItem._colLastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 ||
                           dataItem._colFirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 ||
                           dataItem._colMiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0;

                if (searchTerms.Length == 2)
                    // Check combinations of two search terms
                    return (dataItem._colFirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            dataItem._colLastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (dataItem._colLastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            dataItem._colFirstName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (dataItem._colFirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            dataItem._colMiddleName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >=
                            0) ||
                           (dataItem._colMiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            dataItem._colFirstName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (dataItem._colMiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            dataItem._colLastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (dataItem._colLastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            dataItem._colMiddleName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0);

                if (searchTerms.Length >= 3)
                    // Check if all search terms exist in any order
                    return searchTerms.All(term => fullName.Contains(term));
            }

            return false;
        };

        _collectionViewSource.View.Refresh();
    }

    private void PreviewTextSearch(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void WIndow_Main_Activated(object sender, EventArgs e)
    {
        //_data = new ObservableCollection<DataItem>(((App)Application.Current).GetDataItems());
        _collectionViewSource = new CollectionViewSource { Source = Patients };
        MainGrid.ItemsSource = _collectionViewSource.View;
    }

    private void BTN_DeleteRecord_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null)
            return;

        // Get the DataContext of the clicked button
        var dataItem = button.DataContext as DataItem;
        if (dataItem == null)
            return;

        var dialogResult =
            MessageBox.Show(
                $"Ви точно хочете видалити {dataItem._colLastName} {dataItem._colFirstName} {dataItem._colMiddleName} з бази",
                "Підтвердіть", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        if (dialogResult == MessageBoxResult.OK)
            try
            {
                ((App)Application.Current).RemoveDataFromStorage(dataItem);
                WIndow_Main_Activated(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Виникла помилка при видаленні користувача з БД!", "Помилка", MessageBoxButton.OKCancel,
                    MessageBoxImage.Exclamation);
            }
    }

    private void InitializeMongo()
    {
        var config = AppConfig.Load();
        _mongoService = new MongoDbService(
            config.MongoDbConnection,
            config.DatabaseName,
            config.PatientsCollection
        );
    }

    private async Task LoadPatientsAsync()
    {
        var patientsFromDb = await _mongoService.GetAllPatientsAsync();
        Patients.Clear();
        foreach (var patient in patientsFromDb)
        {
            Patients.Add(patient);
        }
    }

    private async void WIndow_Main_Loaded(object sender, RoutedEventArgs e)
    {
        //await JsonImporter.ImportFromJsonAsync("database.json", _mongoService);
        await LoadPatientsAsync();
    }
}