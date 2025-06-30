using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MedProject_UI.Models;
using MedProject_UI.Services;
using MedProject_UI.View.UserControls;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CollectionViewSource _collectionViewSource;
    private MongoDbService _mongoService;

    public MainWindow()
    {
        InitializeComponent();
        InitializeMongo();
        _ = RefreshPatientsAsync();

        SearchBar.TextChanged += RouteViewerOpened;
        SearchBar.PreviewTextInput += PreviewTextSearch;

        btnAddNewPatient.btnClick += AddPatientBtnClick;
    }


    private async void GetListDocument(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button?.DataContext is not Patient selectedPatient)
            return;

        var patientDescription = new PatientDescription(selectedPatient);
        patientDescription.ShowDialog();

        await RefreshPatientsAsync();
    }

    private async void AddPatientBtnClick(object sender, RoutedEventArgs e)
    {
        var addPatient = new AddPatient();
        addPatient.ShowDialog();

        await RefreshPatientsAsync();
    }

    private void RouteViewerOpened(object sender, TextChangedEventArgs e)
    {
        var textBox = (sender as ClearableSearchBar)?.tbSearchText;
        if (textBox == null) return;
        var filterText = textBox.Text.Trim();

        _collectionViewSource.View.Filter = item =>
        {
            if (item is Patient patient)
            {
                var fullName = $"{patient.FirstName} {patient.MiddleName} {patient.LastName}".ToLower();
                var searchTerms = filterText.ToLower().Split(' ');

                if (searchTerms.Length == 1)
                    // Check if the single search term matches any of the fields
                    return patient.LastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 ||
                           patient.FirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 ||
                           patient.MiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0;

                if (searchTerms.Length == 2)
                    // Check combinations of two search terms
                    return (patient.FirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            patient.LastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (patient.LastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            patient.FirstName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (patient.FirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            patient.MiddleName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >=
                            0) ||
                           (patient.MiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            patient.FirstName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (patient.MiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            patient.LastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                           (patient.LastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                            patient.MiddleName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0);

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

    private async void BTN_DeleteRecord_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button?.DataContext is not Patient selectedPatient)
            return;

        var dialogResult =
            MessageBox.Show(
                $"Ви точно хочете видалити {selectedPatient.LastName} {selectedPatient.FirstName} {selectedPatient.MiddleName} з бази",
                "Підтвердіть", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        if (dialogResult == MessageBoxResult.OK)
            try
            {
                await _mongoService.DeletePatientAsync(selectedPatient.Id);
                await RefreshPatientsAsync();
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
            config.DatabaseName
        );
    }

    private async Task RefreshPatientsAsync()
    {
        var patients = await _mongoService.GetAllPatientsAsync();
        _collectionViewSource = new CollectionViewSource { Source = patients };
        MainGrid.ItemsSource = _collectionViewSource.View;
    }
}