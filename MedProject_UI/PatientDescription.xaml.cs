using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedProject_UI
{
    /// <summary>
    /// Interaction logic for PatientDescription.xaml
    /// </summary>
    public partial class PatientDescription : Window
    {
        public PatientDescription()
        {
            InitializeComponent();
            mainDetailsGrid.Visibility = Visibility.Visible;
            diagnosisDetailsGrid.Visibility = Visibility.Hidden;
            shortIllDescGrid.Visibility = Visibility.Hidden;
            additionalDetailsGrid.Visibility = Visibility.Hidden;
        }

        private void btnPage1Next_Click(object sender, RoutedEventArgs e)
        {
            mainDetailsGrid.Visibility = Visibility.Hidden;
            diagnosisDetailsGrid.Visibility = Visibility.Visible;
        }

        private void btnPage2Next_Click(object sender, RoutedEventArgs e)
        {
            diagnosisDetailsGrid.Visibility = Visibility.Hidden;
            shortIllDescGrid.Visibility = Visibility.Visible;
        }

        private void btnPage2Back_Click(object sender, RoutedEventArgs e)
        {
            diagnosisDetailsGrid.Visibility = Visibility.Hidden;
            mainDetailsGrid.Visibility = Visibility.Visible;
        }

        private void btnPage3Back_Click(object sender, RoutedEventArgs e)
        {
            shortIllDescGrid.Visibility = Visibility.Hidden;
            diagnosisDetailsGrid.Visibility = Visibility.Visible;
        }

        private void btnPage3Next_Click(object sender, RoutedEventArgs e)
        {
            shortIllDescGrid.Visibility = Visibility.Hidden;
            additionalDetailsGrid.Visibility = Visibility.Visible;
        }

        private void btnPage4Back_Click(object sender, RoutedEventArgs e)
        {
            additionalDetailsGrid.Visibility = Visibility.Hidden;
            shortIllDescGrid.Visibility = Visibility.Visible;
        }

        private void btnPage4Next_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Документ буде створено", "Doc Builded", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (dialogResult == MessageBoxResult.OK)
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
        }
    }
}
