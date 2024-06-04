using MedProject_UI.View.UserControls;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MedProject_UI.App;

namespace MedProject_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    

    public partial class MainWindow : Window
    {
        private ObservableCollection<DataItem> data;
        private CollectionViewSource collectionViewSource;

        public MainWindow()
        {
            InitializeComponent();

            SearchBar.TextChanged += RouteViewerOpened;

            btnAddNewPatient.btnClick += AddPatientBtnClick;
        }



        private void GetListDocument(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button == null)
                return;

            // Get the DataContext of the clicked button
            DataItem? dataItem = button.DataContext as DataItem;
            if (dataItem == null)
                return;

            //// Show the message box with the correct card number
            //MessageBox.Show($"Номер картки: {dataItem._colCardNumber}");
            PatientDescription patientDescription = new PatientDescription(dataItem);
            patientDescription.ShowDialog();

        }

        private void AddPatientBtnClick(object sender, RoutedEventArgs e)
        {
            
            AddPatient addPatient = new AddPatient();
            addPatient.ShowDialog();
        }

        private void RouteViewerOpened(object sender, TextChangedEventArgs e)
        {
            TextBox? textBox = (sender as ClearableSearchBar)?.tbSearchText;
            if (textBox == null) return;
            string filterText = textBox.Text.Trim();

            collectionViewSource.View.Filter = item =>
            {
                if (item is DataItem dataItem)
                {
                    string fullName = $"{dataItem._colFirstName} {dataItem._colMiddleName} {dataItem._colLastName}".ToLower();
                    string[] searchTerms = filterText.ToLower().Split(' ');

                    if (searchTerms.Length == 1)
                    {
                        // Check if the single search term matches any of the fields
                        return dataItem._colLastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 ||
                               dataItem._colFirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 ||
                               dataItem._colMiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0;
                    }
                    else if (searchTerms.Length == 2)
                    {
                        // Check combinations of two search terms
                        return (dataItem._colFirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                dataItem._colLastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                               (dataItem._colLastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                dataItem._colFirstName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                               (dataItem._colFirstName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                dataItem._colMiddleName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                               (dataItem._colMiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                dataItem._colFirstName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                               (dataItem._colMiddleName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                dataItem._colLastName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0) ||
                               (dataItem._colLastName?.IndexOf(searchTerms[0], StringComparison.OrdinalIgnoreCase) >= 0 &&
                                dataItem._colMiddleName?.IndexOf(searchTerms[1], StringComparison.OrdinalIgnoreCase) >= 0);
                    }
                    else if (searchTerms.Length >= 3)
                    {
                        // Check if all search terms exist in any order
                        return searchTerms.All(term => fullName.Contains(term));
                    }
                }
                return false;
            };

            collectionViewSource.View.Refresh();
        }

        private void WIndow_Main_Activated(object sender, EventArgs e)
        {
            data = new ObservableCollection<DataItem>(((App)Application.Current).GetDataItems());

            collectionViewSource = new CollectionViewSource { Source = data };
            MainGrid.ItemsSource = collectionViewSource.View;
        }
    }
}