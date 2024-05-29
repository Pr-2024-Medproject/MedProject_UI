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
    /// Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        public AddPatient()
        {
            InitializeComponent(); 
            mainGridPage1.Visibility = Visibility.Visible;
            mainGridPage2.Visibility = Visibility.Hidden;
            mainGridPage3.Visibility = Visibility.Hidden;
            mainGridPage4.Visibility = Visibility.Hidden;
            mainGridPage5.Visibility = Visibility.Hidden;
            mainGridPage6.Visibility = Visibility.Hidden;
            mainGridPage7.Visibility = Visibility.Hidden;


            tbLastName.TextChanged += AddTextLastName;
            tbFirstName.TextChanged += AddTextFirstName;
            tbMiddleName.TextChanged += AddTextMiddleName;
            tbLivingAddress.TextChanged += AddTextLivingAddress;
            tbWorkAddress.TextChanged += AddTextWorkAddress;
            tbMKX.TextChanged += AddTextMKX;

            tbOperationName.TextChanged += AddTextOperationName;
            tbChemotherapyName.TextChanged += AddTextChemotherapyName;


            btnPage1Next.btnClick += btnPage1Next_Click;

            btnPage2Next.btnClick += btnPage2Next_Click;
            btnPage2Back.btnClick += btnPage2Back_Click;

            btnPage3Next.btnClick += btnPage3Next_Click;
            btnPage3Back.btnClick += btnPage3Back_Click;

            btnPage4Next.btnClick += btnPage4Next_Click;
            btnPage4Back.btnClick += btnPage4Back_Click;

            btnPage5Next.btnClick += btnPage5Next_Click;
            btnPage5Back.btnClick += btnPage5Back_Click;

            btnPage6Next.btnClick += btnPage6Next_Click;
            btnPage6Back.btnClick += btnPage6Back_Click;

            btnPage7Back.btnClick += btnPage7Back_Click;
        }

        private void AddTextLastName(object sender, TextChangedEventArgs e)
        { 
        }

        private void AddTextFirstName(object sender, TextChangedEventArgs e)
        {
        }

        private void AddTextMiddleName(object sender, TextChangedEventArgs e)
        {
        }

        private void AddTextLivingAddress(object sender, TextChangedEventArgs e)
        {
        }
        private void AddTextWorkAddress(object sender, TextChangedEventArgs e)
        {
        }
        private void AddTextMKX(object sender, TextChangedEventArgs e)
        {
        }
        private void AddTextOperationName(object sender, TextChangedEventArgs e)
        {
        }
        private void AddTextChemotherapyName(object sender, TextChangedEventArgs e)
        {
        }
        



        private void btnPage1Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage1.Visibility = Visibility.Hidden;
            mainGridPage2.Visibility = Visibility.Visible;
        }


        private void btnPage2Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage2.Visibility = Visibility.Hidden;
            mainGridPage3.Visibility = Visibility.Visible;
        }

        private void btnPage2Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage2.Visibility = Visibility.Hidden;
            mainGridPage1.Visibility = Visibility.Visible;
        }



        private void btnPage3Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage3.Visibility = Visibility.Hidden;
            mainGridPage2.Visibility = Visibility.Visible;
        }

        private void btnPage3Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage3.Visibility = Visibility.Hidden;
            mainGridPage4.Visibility = Visibility.Visible;
        }



        private void btnPage4Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage4.Visibility = Visibility.Hidden;
            mainGridPage3.Visibility = Visibility.Visible;
        }
        private void btnPage4Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage4.Visibility = Visibility.Hidden;
            mainGridPage5.Visibility = Visibility.Visible;
        }



        private void btnPage5Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage5.Visibility = Visibility.Hidden;
            mainGridPage4.Visibility = Visibility.Visible;
        }
        private void btnPage5Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage5.Visibility = Visibility.Hidden;
            mainGridPage6.Visibility = Visibility.Visible;
        }


        private void btnPage6Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage6.Visibility = Visibility.Hidden;
            mainGridPage5.Visibility = Visibility.Visible;
        }
        private void btnPage6Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage6.Visibility = Visibility.Hidden;
            mainGridPage7.Visibility = Visibility.Visible;
        }

        private void btnPage7Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage7.Visibility = Visibility.Hidden;
            mainGridPage6.Visibility = Visibility.Visible;
        }

        /*private void btnPage6Next_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Документ буде створено", "Doc Builded", MessageBoxButton.OKCancel, MessageBoxImage.Information);

           *//* if (dialogResult == MessageBoxResult.OK)
            {
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }*//*
        }*/

        private void comboBoxClaims_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? newClaims = sender as ComboBox;
            tbClims.Text += newClaims.SelectedValue.ToString().Split(": ")[1] + ", ";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            gridOverallItem7.Visibility = Visibility.Visible;
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            gridOverallItem7.Visibility = Visibility.Hidden;
            
        }

        private void comboBoxDoctorName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
