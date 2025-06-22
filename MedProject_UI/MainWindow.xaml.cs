using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using MedProject_UI.View.UserControls;
using static MedProject_UI.App;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ObservableCollection<DataItem> data;
    private CollectionViewSource collectionViewSource;

    public MainWindow()
    {
        InitializeComponent();

        SearchBar.TextChanged += RouteViewerOpened;
        SearchBar.PreviewTextInput += PreviewTextSearch;

        btnAddNewPatient.btnClick += AddPatientBtnClick;
    }


    private void GetListDocument(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null)
            return;

        // Get the DataContext of the clicked button
        var dataItem = button.DataContext as DataItem;
        if (dataItem == null)
            return;

        //// Show the message box with the correct card number
        //MessageBox.Show($"Номер картки: {dataItem._colCardNumber}");
        var patientDescription = new PatientDescription(dataItem);
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

        collectionViewSource.View.Filter = item =>
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

        collectionViewSource.View.Refresh();
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
        data = new ObservableCollection<DataItem>(((App)Application.Current).GetDataItems());

        collectionViewSource = new CollectionViewSource { Source = data };
        MainGrid.ItemsSource = collectionViewSource.View;
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
}