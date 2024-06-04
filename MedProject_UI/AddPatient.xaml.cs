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
using static MedProject_UI.App;

namespace MedProject_UI
{
    /// <summary>
    /// Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        DataItem newPatient = new DataItem();

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
            mainGridPage8.Visibility = Visibility.Hidden;


            tbLastName.TextChanged += AddTextLastName;
            tbFirstName.TextChanged += AddTextFirstName;
            tbMiddleName.TextChanged += AddTextMiddleName;
            tbLivingAddress.TextChanged += AddTextLivingAddress;
            tbWorkAddress.TextChanged += AddTextWorkAddress;
            tbMKX.TextChanged += AddTextMKX;
            tbHistology.TextChanged += AddTextHistology;
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
            btnPage7Next.btnClick += btnPage7Next_Click;

            btnPage8Back.btnClick += btnPage8Back_Click;
            btnPage8Save.btnClick += btnPage8Next_Click;
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
        private void AddTextHistology(object sender, TextChangedEventArgs e)
        {
        }




        private void btnPage1Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage1.Visibility = Visibility.Hidden;
            mainGridPage2.Visibility = Visibility.Visible;


            newPatient._colLastName = tbLastName.tbSearchText.Text;
            newPatient._colFirstName = tbFirstName.tbSearchText.Text;
            newPatient._colMiddleName = tbMiddleName.tbSearchText.Text;
            newPatient._colBirthDay = datePickerBirthday.customDatePicker.SelectedDate != null
                                            ? datePickerBirthday.customDatePicker.SelectedDate
                                            : DateTime.Now.AddYears(-1);
            newPatient._colAddress = tbLivingAddress.tbSearchText.Text;
            newPatient._colProfession = tbWorkAddress.tbSearchText.Text;
            newPatient._colHospitalDate = datePickerHospitalStart.customDatePicker.SelectedDate != null
                                            ? datePickerHospitalStart.customDatePicker.SelectedDate
                                            : DateTime.Now;
            newPatient._colLeaveDate = datePickerHospitalEnd.customDatePicker.SelectedDate != null
                                            ? datePickerHospitalEnd.customDatePicker.SelectedDate
                                            : DateTime.Now;
        }


        private void btnPage2Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage2.Visibility = Visibility.Hidden;
            mainGridPage3.Visibility = Visibility.Visible;

            newPatient._fieldClaims = tbClaims.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldEntrDiagnosis = tbEntrDiagnosis.Text;
            newPatient._fieldFinalDiagnosis = tbFinalDiagnosis.Text;
            newPatient._fieldComplication = tbComplications.Text;
            newPatient._fieldAdditionalDiagnosis = tbAdditionDiagnosis.Text;
            newPatient._fieldMKX = tbMKX.tbSearchText.Text;
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


            newPatient._fieldOperationName = tbOperationName.tbSearchText.Text;
            newPatient._fieldOperationDate = dateOperation.customDatePicker.SelectedDate != null
                                            ? dateOperation.customDatePicker.SelectedDate
                                            : DateTime.Now;
            newPatient._fieldChemotherapy = tbChemotherapyName.tbSearchText.Text;
            newPatient._fieldChemotherapyDate = dateChemotherapy.customDatePicker.SelectedDate != null
                                            ? dateChemotherapy.customDatePicker.SelectedDate
                                            : DateTime.Now;
            newPatient._fieldHistology = tbHistology.tbSearchText.Text;
            newPatient._fieldDoctor = comboBoxDoctorName.Text;
            newPatient._fieldDepartmentHead = tbDepartmentHead.Text;
            newPatient._fieldDepartHeadAssistant = tbDepartHeadAssistant.Text;
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


            var contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem1)
                                                 .OfType<RadioButton>()
                                                 .ToList()
                                                 .Where(x => x.GroupName == "rbOverallItem1")
                                                 .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem1 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem1
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem2)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem2")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem2 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem2
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem3)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem3")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem3 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem3
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem4)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem4")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem4 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem4
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem5)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem5")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem5 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem5
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem6)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem6")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem6 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem6
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem7)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem7")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem7 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Не збільшена" ? "Не збільшена" : contentHolder.Content.ToString() + $" на {tbOverallItem7.Text} см."
                                                : "";
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem8)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem8")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem8 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem8
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;
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

            var contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem9)
                                                 .OfType<RadioButton>()
                                                 .ToList()
                                                 .Where(x => x.GroupName == "rbOverallItem9_1")
                                                 .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem9_1 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Негативний" ? false : true
                                                : false;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem9)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem9_2")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem9_2 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Справа" ? false : true
                                                : false;
            List<CheckBox> cbOverallItem10List = new List<CheckBox>() {
                cbOverallItem10_1,
                cbOverallItem10_2,
                cbOverallItem10_3,
                cbOverallItem10_4,
                cbOverallItem10_5,
                cbOverallItem10_6
            };
            List<int> intHolder = new List<int>();
            for (int i = 0; cbOverallItem10List.Count > i; i++)
            {
                if ((bool)cbOverallItem10List[i].IsChecked) intHolder.Add(i);
            }
            newPatient._fieldOverallItem10 = intHolder.ToArray();

            int flag = 0;
            int.TryParse(tbOverallItem11.Text, out flag);
            newPatient._fieldOverallItem11 = flag;
            newPatient._fieldOverallItem12 = tbOverallItem12.Text;

            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem13)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem13")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem13 = contentHolder != null
                                                ? ((App)Application.Current).dictOverallItem13
                                                             .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
                                                : 0;

            List<CheckBox> cbOverallItem14List = new List<CheckBox>() {
                cbOverallItem14_1,
                cbOverallItem14_2,
                cbOverallItem14_3,
                cbOverallItem14_4,
                cbOverallItem14_5
             };
            intHolder = new List<int>();
            for (int i = 0; cbOverallItem14List.Count > i; i++)
            {
                if ((bool)cbOverallItem14List[i].IsChecked) intHolder.Add(i);
            }
            newPatient._fieldOverallItem14 = intHolder.ToArray();
            flag = 0;
            int.TryParse(tbOverallItem15.Text, out flag);
            newPatient._fieldOverallItem15 = flag;
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

            newPatient._fieldAnamnesisItem1 = tbAnamnesisItem1.Text;
            newPatient._fieldAnamnesisItem2 = tbAnamnesisItem2.Text;
            newPatient._fieldAnamnesisItem3 = tbAnamnesisItem3.Text;
            newPatient._fieldAnamnesisItem4 = tbAnamnesisItem4.Text;
            newPatient._fieldAnamnesisItem5 = tbAnamnesisItem5.Text;
            newPatient._fieldAnamnesisItem6 = tbAnamnesisItem6.Text;
            newPatient._fieldAnamnesisItem7 = tbAnamnesisItem7.Text;
            newPatient._fieldAnamnesisItem8 = tbAnamnesisItem8.Text;
            newPatient._fieldAnamnesisItem9 = tbAnamnesisItem9.Text;
            newPatient._fieldAnamnesisItem10 = tbAnamnesisItem10.Text;
            newPatient._fieldAnamnesisItem11 = tbAnamnesisItem11.Text;
        }

        private void btnPage7Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage7.Visibility = Visibility.Hidden;
            mainGridPage6.Visibility = Visibility.Visible;
        }
        private void btnPage7Next_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage7.Visibility = Visibility.Hidden;
            mainGridPage8.Visibility = Visibility.Visible;

            newPatient._fieldLifeAnamnesisItem1 = tbLifeAnamnesisItem1.Text;
            newPatient._fieldLifeAnamnesisItem2 = tbLifeAnamnesisItem2.Text;
            newPatient._fieldLifeAnamnesisItem3 = tbLifeAnamnesisItem3.Text;
            newPatient._fieldLifeAnamnesisItem4 = tbLifeAnamnesisItem4.Text;
            newPatient._fieldLifeAnamnesisItem5 = tbLifeAnamnesisItem5.Text;
            newPatient._fieldLifeAnamnesisItem6 = tbLifeAnamnesisItem6.Text;
            newPatient._fieldLifeAnamnesisItem7 = tbLifeAnamnesisItem7.Text;
            int flag = 0;
            int.TryParse(tbLifeAnamnesisItem8.Text, out flag);
            newPatient._fieldLifeAnamnesisItem8 = flag;

            flag = 0;
            int.TryParse(tbLifeAnamnesisItem9.Text, out flag);
            newPatient._fieldLifeAnamnesisItem9 = flag;

            newPatient._fieldLifeAnamnesisItem10 = tbLifeAnamnesisItem10.Text;
        }

        private void btnPage8Back_Click(object sender, RoutedEventArgs e)
        {
            mainGridPage8.Visibility = Visibility.Hidden;
            mainGridPage7.Visibility = Visibility.Visible;
        }


        private void btnPage8Next_Click(object sender, RoutedEventArgs e)
        {


            newPatient._fieldLocusMorbiItem1 = tbLocusMorbiItem1.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldLocusMorbiItem2 = tbLocusMorbiItem2.Text.Trim().TrimEnd(',').Split(", ");

            var contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbLocusMorbiItem3")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldLocusMorbiItem3 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Везикулярне" ? false : true
                                                : false;

            newPatient._fieldLocusMorbiItem4 = tbLocusMorbiItem4.Text.Trim().TrimEnd(',').Split(", ");

            contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbLocusMorbiItem5")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldLocusMorbiItem5 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Легеневий звук" ? false : true
                                                : false;
            newPatient._fieldLocusMorbiItem6 = tbLocusMorbiItem6.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldLocusMorbiItem7 = tbLocusMorbiItem7.Text.Trim().TrimEnd(',').Split(", ");

            MessageBoxResult dialogResult = MessageBox.Show($"Додати паціента {newPatient._colLastName} {newPatient._colFirstName} до бази?", "Перевірка", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (dialogResult == MessageBoxResult.OK)
            {
                ((App)Application.Current).AddDateToStorage(newPatient);
                //MainWindow mainWindow = new MainWindow();
                this.Close();
                //mainWindow.Show();
            }

        }

            private void comboBoxClaims_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? newClaims = sender as ComboBox;
            tbClaims.Text += newClaims.SelectedValue.ToString().Split(": ")[1] + ", ";
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

        private void cbLocusMorbiItem1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? locusMorbiItem1 = sender as ComboBox;
            tbLocusMorbiItem1.Text += locusMorbiItem1.SelectedValue.ToString().Split(": ")[1] + ", ";
        }

        private void cbLocusMorbiItem2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? locusMorbiItem2 = sender as ComboBox;
            tbLocusMorbiItem2.Text += locusMorbiItem2.SelectedValue.ToString().Split(": ")[1] + ", ";
        }

        private void cbLocusMorbiItem4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? locusMorbiItem4 = sender as ComboBox;
            tbLocusMorbiItem4.Text += locusMorbiItem4.SelectedValue.ToString().Split(": ")[1] + ", ";
        }

        private void cbLocusMorbiItem6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? locusMorbiItem6 = sender as ComboBox;
            tbLocusMorbiItem6.Text += locusMorbiItem6.SelectedValue.ToString().Split(": ")[1] + ", ";
        }

        private void cbLocusMorbiItem7_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox? locusMorbiItem7 = sender as ComboBox;
            tbLocusMorbiItem7.Text += locusMorbiItem7.SelectedValue.ToString().Split(": ")[1] + ", ";
        }

    }
}
