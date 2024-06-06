using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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

            tbLastName.PreviewTextInput += PreviewTextLastName;
            tbFirstName.PreviewTextInput += PreviewTextFirstName;
            tbMiddleName.PreviewTextInput += PreviewTextMiddleName;
            tbLivingAddress.PreviewTextInput += PreviewTextLivingAddress;
            tbWorkAddress.PreviewTextInput += PreviewTextWork;
            tbMKX.PreviewTextInput += PreviewTextMKX;
            tbHistology.PreviewTextInput += PreviewTextHistology;
            tbOperationName.PreviewTextInput += PreviewTextOperationName;
            tbChemotherapyName.PreviewTextInput += PreviewTextChemotherapyName;

            datePickerBirthday.DateSelectionChange += DateSelectionChanged_Birthday;
            datePickerHospitalStart.DateSelectionChange += DateSelectionChanged_HospitalStart;
            datePickerHospitalEnd.DateSelectionChange += DateSelectionChanged_HospitalEnd;
            dateOperation.DateSelectionChange += DateSelectionChanged_Operation;
            dateChemotherapy.DateSelectionChange += DateSelectionChanged_Chemotherapy;

            datePickerBirthday.DatePrewiewText += DatePrewiewText_Birthday;
            datePickerHospitalStart.DatePrewiewText += DatePrewiewText_HospitalStart;
            datePickerHospitalEnd.DatePrewiewText += DatePrewiewText_HospitalEnd;
            dateOperation.DatePrewiewText += DatePrewiewText_Operation;
            dateChemotherapy.DatePrewiewText += DatePrewiewText_Chemotherapy;


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
            btnPage8SaveChanges.Visibility = Visibility.Hidden;
            //btnPage8SaveChanges.btnClick += btnPage8Save_Changes;
        }

        public AddPatient(DataItem editData)
        {
            InitializeComponent();

            newPatient = editData;

            this.Title = "Edit Patient";



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

            tbLastName.PreviewTextInput += PreviewTextLastName;
            tbFirstName.PreviewTextInput += PreviewTextFirstName;
            tbMiddleName.PreviewTextInput += PreviewTextMiddleName;
            tbLivingAddress.PreviewTextInput += PreviewTextLivingAddress;
            tbWorkAddress.PreviewTextInput += PreviewTextWork;
            tbMKX.PreviewTextInput += PreviewTextMKX;
            tbHistology.PreviewTextInput += PreviewTextHistology;
            tbOperationName.PreviewTextInput += PreviewTextOperationName;
            tbChemotherapyName.PreviewTextInput += PreviewTextChemotherapyName;

            datePickerBirthday.DateSelectionChange += DateSelectionChanged_Birthday;
            datePickerHospitalStart.DateSelectionChange += DateSelectionChanged_HospitalStart;
            datePickerHospitalEnd.DateSelectionChange += DateSelectionChanged_HospitalEnd;
            dateOperation.DateSelectionChange += DateSelectionChanged_Operation;
            dateChemotherapy.DateSelectionChange += DateSelectionChanged_Chemotherapy;

            datePickerBirthday.DatePrewiewText += DatePrewiewText_Birthday;
            datePickerHospitalStart.DatePrewiewText += DatePrewiewText_HospitalStart;
            datePickerHospitalEnd.DatePrewiewText += DatePrewiewText_HospitalEnd;
            dateOperation.DatePrewiewText += DatePrewiewText_Operation;
            dateChemotherapy.DatePrewiewText += DatePrewiewText_Chemotherapy;


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
            //btnPage8Save.btnClick += btnPage8Next_Click;
            btnPage8Save.Visibility = Visibility.Hidden;
            btnPage8SaveChanges.btnClick += btnPage8Save_Changes;
            btnPage8SaveChanges.Visibility = Visibility.Visible;


            #region Put fields into fields
            tbLastName.tbSearchText.Text = newPatient._colLastName;
            tbFirstName.tbSearchText.Text = newPatient._colFirstName;
            tbMiddleName.tbSearchText.Text = newPatient._colMiddleName;
            datePickerBirthday.customDatePicker.SelectedDate = newPatient._colBirthDay;
            tbLivingAddress.tbSearchText.Text = newPatient._colAddress;
            tbWorkAddress.tbSearchText.Text = newPatient._colProfession;
            datePickerHospitalStart.customDatePicker.SelectedDate = newPatient._colHospitalDate;
            datePickerHospitalEnd.customDatePicker.SelectedDate = newPatient._colLeaveDate;
            tbClaims.Text = newPatient._fieldClaims != null ? string.Join(", ", newPatient._fieldClaims) : "";
            tbEntrDiagnosis.Text = newPatient._fieldEntrDiagnosis != null ? newPatient._fieldEntrDiagnosis : "";
            tbFinalDiagnosis.Text = newPatient._fieldFinalDiagnosis != null ? newPatient._fieldFinalDiagnosis : "";
            tbComplications.Text = newPatient._fieldComplication != null ? newPatient._fieldComplication : "";
            tbAdditionDiagnosis.Text = newPatient._fieldAdditionalDiagnosis != null ? newPatient._fieldAdditionalDiagnosis : "";
            tbMKX.tbSearchText.Text = newPatient._fieldMKX != null ? newPatient._fieldMKX : "";
            tbOperationName.tbSearchText.Text = newPatient._fieldOperationName != null ? newPatient._fieldOperationName : "";
            dateOperation.customDatePicker.SelectedDate = newPatient._fieldOperationDate;
            tbChemotherapyName.tbSearchText.Text = newPatient._fieldChemotherapy != null ? newPatient._fieldChemotherapy : "";
            dateChemotherapy.customDatePicker.SelectedDate = newPatient._fieldChemotherapyDate;
            tbHistology.tbSearchText.Text = newPatient._fieldHistology != null ? newPatient._fieldHistology : "";
            List<string> doctors = new List<string>();
            foreach (var item in comboBoxDoctorName.Items)
            {
                doctors.Add((item as ComboBoxItem).Content.ToString());
            }
            comboBoxDoctorName.SelectedIndex = doctors.IndexOf(newPatient._fieldDoctor != null ? newPatient._fieldDoctor : "");
            tbDepartmentHead.Text = newPatient._fieldDepartmentHead != null ? newPatient._fieldDepartmentHead : "Г.В. Трунов";
            tbDepartHeadAssistant.Text = newPatient._fieldDepartHeadAssistant != null ? newPatient._fieldDepartHeadAssistant : "";
            LogicalTreeHelper.GetChildren(containerOverallItem1)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem1").ToList()[newPatient._fieldOverallItem1 != null ? (int)newPatient._fieldOverallItem1 : 0].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem2)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem2").ToList()[newPatient._fieldOverallItem2 != null ? (int)newPatient._fieldOverallItem2 : 0].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem3)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem3").ToList()[newPatient._fieldOverallItem3 != null ? (int)newPatient._fieldOverallItem3 : 0].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem4)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem4").ToList()[newPatient._fieldOverallItem4 != null ? (int)newPatient._fieldOverallItem4 : 0].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem5)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem5").ToList()[newPatient._fieldOverallItem5 != null ? (int)newPatient._fieldOverallItem5 : 0].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem6)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem6").ToList()[newPatient._fieldOverallItem6 != null ? (int)newPatient._fieldOverallItem6 : 0].IsChecked = true;
            string liverExtend = newPatient._fieldOverallItem7 != null
                                    ? newPatient._fieldOverallItem7.Contains("Збільшена")
                                        ? newPatient._fieldOverallItem7.Split(" на ")[1].Trim().Split("см")[0].Trim()
                                        : ""
                                    : "";
            tbOverallItem7.Text = liverExtend;
            if (liverExtend == "")
            {
                LogicalTreeHelper.GetChildren(containerOverallItem7)
                                 .OfType<RadioButton>()
                                 .ToList()
                                 .Where(x => x.GroupName == "rbOverallItem7").ToList()[0].IsChecked = true;
            }
            else
            {
                LogicalTreeHelper.GetChildren(containerOverallItem7)
                                 .OfType<RadioButton>()
                                 .ToList()
                                 .Where(x => x.GroupName == "rbOverallItem7").ToList()[1].IsChecked = true;
            }
            LogicalTreeHelper.GetChildren(containerOverallItem8)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem8").ToList()[newPatient._fieldOverallItem8 != null ? (int)newPatient._fieldOverallItem8 : 0].IsChecked = true;
            if (newPatient._fieldOverallItem9_1 != null)
            {
                if ((bool)newPatient._fieldOverallItem9_1)
                {
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                                 .OfType<RadioButton>()
                                 .ToList()
                                 .Where(x => x.GroupName == "rbOverallItem9_1").ToList()[1].IsChecked = true;
                }
                else
                {
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                                     .OfType<RadioButton>()
                                     .ToList()
                                     .Where(x => x.GroupName == "rbOverallItem9_1").ToList()[0].IsChecked = true;
                }
            }
            if (newPatient._fieldOverallItem9_2 != null)
            {
                if ((bool)newPatient._fieldOverallItem9_2)
                {
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                                 .OfType<RadioButton>()
                                 .ToList()
                                 .Where(x => x.GroupName == "rbOverallItem9_2").ToList()[1].IsChecked = true;
                }
                else
                {
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                                     .OfType<RadioButton>()
                                     .ToList()
                                     .Where(x => x.GroupName == "rbOverallItem9_2").ToList()[0].IsChecked = true;
                }
            }

            if (newPatient._fieldOverallItem10 != null)
            {
                List<CheckBox> cbOverallItem10List = new List<CheckBox>() {
                    cbOverallItem10_1,
                    cbOverallItem10_2,
                    cbOverallItem10_3,
                    cbOverallItem10_4,
                    cbOverallItem10_5,
                    cbOverallItem10_6
                };

                foreach (int i in newPatient._fieldOverallItem10)
                {
                    cbOverallItem10List[i].IsChecked = true;
                }
            }
            tbOverallItem11.Text = newPatient._fieldOverallItem11 != null ? newPatient._fieldOverallItem11.ToString() : "";
            tbOverallItem12.Text = newPatient._fieldOverallItem12 != null ? newPatient._fieldOverallItem12 : "";
            LogicalTreeHelper.GetChildren(containerOverallItem13)
                             .OfType<RadioButton>()
                             .ToList()
                             .Where(x => x.GroupName == "rbOverallItem13").ToList()[newPatient._fieldOverallItem13 != null ? (int)newPatient._fieldOverallItem13 : 0].IsChecked = true;
            if (newPatient._fieldOverallItem14 != null)
            {
                List<CheckBox> cbOverallItem14List = new List<CheckBox>() {
                    cbOverallItem14_1,
                    cbOverallItem14_2,
                    cbOverallItem14_3,
                    cbOverallItem14_4,
                    cbOverallItem14_5
                };
                foreach (int i in newPatient._fieldOverallItem14)
                {
                    cbOverallItem14List[i].IsChecked = true;
                }
            }
            tbOverallItem15.Text = newPatient._fieldOverallItem15 != null ? newPatient._fieldOverallItem15.ToString() : "";
            tbAnamnesisItem1.Text = newPatient._fieldAnamnesisItem1 != null ? newPatient._fieldAnamnesisItem1 : "";
            tbAnamnesisItem2.Text = newPatient._fieldAnamnesisItem2 != null ? newPatient._fieldAnamnesisItem2 : "";
            tbAnamnesisItem3.Text = newPatient._fieldAnamnesisItem3 != null ? newPatient._fieldAnamnesisItem3 : "";
            tbAnamnesisItem4.Text = newPatient._fieldAnamnesisItem4 != null ? newPatient._fieldAnamnesisItem4 : "";
            tbAnamnesisItem5.Text = newPatient._fieldAnamnesisItem5 != null ? newPatient._fieldAnamnesisItem5 : "";
            tbAnamnesisItem6.Text = newPatient._fieldAnamnesisItem6 != null ? newPatient._fieldAnamnesisItem6 : "";
            tbAnamnesisItem7.Text = newPatient._fieldAnamnesisItem7 != null ? newPatient._fieldAnamnesisItem7 : "";
            tbAnamnesisItem8.Text = newPatient._fieldAnamnesisItem8 != null ? newPatient._fieldAnamnesisItem8 : "";
            tbAnamnesisItem9.Text = newPatient._fieldAnamnesisItem9 != null ? newPatient._fieldAnamnesisItem9 : "";
            tbAnamnesisItem10.Text = newPatient._fieldAnamnesisItem10 != null ? newPatient._fieldAnamnesisItem10 : "";
            tbAnamnesisItem11.Text = newPatient._fieldAnamnesisItem11 != null ? newPatient._fieldAnamnesisItem11 : "";
            tbLifeAnamnesisItem1.Text = newPatient._fieldLifeAnamnesisItem1 != null ? newPatient._fieldLifeAnamnesisItem1 : "Не ховрів";
            tbLifeAnamnesisItem2.Text = newPatient._fieldLifeAnamnesisItem2 != null ? newPatient._fieldLifeAnamnesisItem2 : "Не ховрів";
            tbLifeAnamnesisItem3.Text = newPatient._fieldLifeAnamnesisItem3 != null ? newPatient._fieldLifeAnamnesisItem3 : "Не ховрів";
            tbLifeAnamnesisItem4.Text = newPatient._fieldLifeAnamnesisItem4 != null ? newPatient._fieldLifeAnamnesisItem4 : "Не ховрів";
            tbLifeAnamnesisItem5.Text = newPatient._fieldLifeAnamnesisItem5 != null ? newPatient._fieldLifeAnamnesisItem5 : "Не ховрів";
            tbLifeAnamnesisItem6.Text = newPatient._fieldLifeAnamnesisItem6 != null ? newPatient._fieldLifeAnamnesisItem6 : "Не ховрів";
            tbLifeAnamnesisItem7.Text = newPatient._fieldLifeAnamnesisItem7 != null ? newPatient._fieldLifeAnamnesisItem7 : "";
            tbLifeAnamnesisItem8.Text = newPatient._fieldLifeAnamnesisItem8 != null ? newPatient._fieldLifeAnamnesisItem8.ToString() : "";
            tbLifeAnamnesisItem9.Text = newPatient._fieldLifeAnamnesisItem9 != null ? newPatient._fieldLifeAnamnesisItem9.ToString() : ""; ;
            tbLifeAnamnesisItem10.Text = newPatient._fieldLifeAnamnesisItem10 != null ? newPatient._fieldLifeAnamnesisItem10 : "";
            tbLocusMorbiItem1.Text = newPatient._fieldLocusMorbiItem1 != null ? string.Join(", ", newPatient._fieldLocusMorbiItem1) : "";
            tbLocusMorbiItem2.Text = newPatient._fieldLocusMorbiItem2 != null ? string.Join(", ", newPatient._fieldLocusMorbiItem2) : "";
            if (newPatient._fieldLocusMorbiItem3 != null)
            {
                if ((bool)newPatient._fieldLocusMorbiItem3)
                {
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                                 .OfType<RadioButton>()
                                 .ToList()
                                 .Where(x => x.GroupName == "rbLocusMorbiItem3").ToList()[1].IsChecked = true;
                }
                else
                {
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                                     .OfType<RadioButton>()
                                     .ToList()
                                     .Where(x => x.GroupName == "rbLocusMorbiItem3").ToList()[0].IsChecked = true;
                }
            }
            tbLocusMorbiItem4.Text = newPatient._fieldLocusMorbiItem4 != null ? string.Join(", ", newPatient._fieldLocusMorbiItem4) : "";
            if (newPatient._fieldLocusMorbiItem5 != null)
            {
                if ((bool)newPatient._fieldLocusMorbiItem5)
                {
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                                 .OfType<RadioButton>()
                                 .ToList()
                                 .Where(x => x.GroupName == "rbLocusMorbiItem5").ToList()[1].IsChecked = true;
                }
                else
                {
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                                     .OfType<RadioButton>()
                                     .ToList()
                                     .Where(x => x.GroupName == "rbLocusMorbiItem5").ToList()[0].IsChecked = true;
                }
            }
            tbLocusMorbiItem6.Text = newPatient._fieldLocusMorbiItem6 != null ? string.Join(", ", newPatient._fieldLocusMorbiItem6) : "";
            tbLocusMorbiItem7.Text = newPatient._fieldLocusMorbiItem7 != null ? string.Join(", ", newPatient._fieldLocusMorbiItem7) : "";
            #endregion
        }

        
        
        private void DatePrewiewText_Birthday(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[ࢶ]$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void DatePrewiewText_HospitalStart(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[ࢶ]$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void DatePrewiewText_HospitalEnd(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[ࢶ]$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void DatePrewiewText_Operation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[ࢶ]$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void DatePrewiewText_Chemotherapy(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[ࢶ]$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextLastName(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextFirstName(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextMiddleName(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextLivingAddress(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9/',. ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextWork(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ'\- ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextMKX(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[a-zA-ZА-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9\,\.\/ ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextHistology(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9/', ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextOperationName(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
        private void PreviewTextChemotherapyName(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }


        private void DateSelectionChanged_Birthday(object sender, SelectionChangedEventArgs e)
        {
            datePickerBirthday.customDatePicker.BlackoutDates.Clear();
            datePickerBirthday.customDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.Date.AddDays(1) , new DateTime(2100, 12, 31)));

            if (datePickerBirthday.customDatePicker.SelectedDate.HasValue)
            {

                try
                {
                    if (datePickerHospitalStart.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        datePickerHospitalStart.customDatePicker.BlackoutDates.Clear();
                    }
                    datePickerHospitalStart.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                }
                catch (Exception ex)
                {

                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата госпіталізації не може бути раніше за дати народження!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        datePickerHospitalStart.customDatePicker.SelectedDate = null;
                        datePickerHospitalStart.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                try
                {
                    if (datePickerHospitalEnd.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        datePickerHospitalEnd.customDatePicker.BlackoutDates.Clear();
                    }
                    datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата виписки не може бути раніше за дати народження!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        datePickerHospitalEnd.customDatePicker.SelectedDate = null;
                        datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }


                try
                {
                    if (dateOperation.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        dateOperation.customDatePicker.BlackoutDates.Clear();
                    }
                    dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата операції не може бути раніше за дати народження!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        dateOperation.customDatePicker.SelectedDate = null;
                        dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                try
                {
                    if (dateChemotherapy.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        dateChemotherapy.customDatePicker.BlackoutDates.Clear();
                    }
                    dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата ПХТ не може бути раніше за дати народження!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        dateChemotherapy.customDatePicker.SelectedDate = null;
                        dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void DateSelectionChanged_HospitalStart(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerHospitalStart.customDatePicker.SelectedDate.HasValue)
            {
                try
                {
                    if (datePickerHospitalEnd.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        datePickerHospitalEnd.customDatePicker.BlackoutDates.Clear();
                    }
                    datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата виписки не може бути раніше за дату госпіталізації!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        datePickerHospitalEnd.customDatePicker.SelectedDate = null;
                        datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }


                try
                {
                    if (dateOperation.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        dateOperation.customDatePicker.BlackoutDates.Clear();
                    }
                    dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата операції не може бути раніше за дату госпіталізації!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        dateOperation.customDatePicker.SelectedDate = null;
                        dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                try
                {
                    if (dateChemotherapy.customDatePicker.BlackoutDates.Count() > 0)
                    {
                        dateChemotherapy.customDatePicker.BlackoutDates.Clear();
                    }
                    dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                    {
                        MessageBox.Show("Обрана дата ПХТ не може бути раніше за дату госпіталізації!", "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        dateChemotherapy.customDatePicker.SelectedDate = null;
                        dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1), datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));

                    }
                    else
                    {
                        MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }
        private void DateSelectionChanged_HospitalEnd(object sender, SelectionChangedEventArgs e)
        { }
        private void DateSelectionChanged_Operation(object sender, SelectionChangedEventArgs e)
        { }
        private void DateSelectionChanged_Chemotherapy(object sender, SelectionChangedEventArgs e)
        { }



        private void AddTextLastName(object sender, TextChangedEventArgs e)
        { }
        private void AddTextFirstName(object sender, TextChangedEventArgs e)
        { }
        private void AddTextMiddleName(object sender, TextChangedEventArgs e)
        { }
        private void AddTextLivingAddress(object sender, TextChangedEventArgs e)
        { }
        private void AddTextWorkAddress(object sender, TextChangedEventArgs e)
        { }
        private void AddTextMKX(object sender, TextChangedEventArgs e)
        { }
        private void AddTextOperationName(object sender, TextChangedEventArgs e)
        { }
        private void AddTextChemotherapyName(object sender, TextChangedEventArgs e)
        { }
        private void AddTextHistology(object sender, TextChangedEventArgs e)
        { }




        private void btnPage1Next_Click(object sender, RoutedEventArgs e)
        {

            List<UserControl> listInputs = new List<UserControl>() { tbLastName, tbFirstName, tbMiddleName, datePickerBirthday, tbLivingAddress, tbWorkAddress, datePickerHospitalStart };
            foreach (UserControl input in listInputs)
            {
                if (input is View.UserControls.ClearableSearchBar)
                {
                    (input as View.UserControls.ClearableSearchBar).borderCustom.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
                }
                else
                {
                    (input as View.UserControls.DatePickerTemplate).borderCustom.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
                }
            }
            listInputs.Where(x => x is View.UserControls.ClearableSearchBar && string.IsNullOrEmpty((x as View.UserControls.ClearableSearchBar).tbSearchText.Text))
                      .ToList()
                      .ForEach(x => (x as View.UserControls.ClearableSearchBar).borderCustom.BorderBrush = new SolidColorBrush(Colors.Red));
            listInputs.Where(x => x is View.UserControls.DatePickerTemplate && (x as View.UserControls.DatePickerTemplate).customDatePicker.SelectedDate == null)
                      .ToList()
                      .ForEach(x => (x as View.UserControls.DatePickerTemplate).borderCustom.BorderBrush = new SolidColorBrush(Colors.Red));


            if (!listInputs.Where(x => x is View.UserControls.ClearableSearchBar && string.IsNullOrEmpty((x as View.UserControls.ClearableSearchBar).tbSearchText.Text)).Any()
             || !listInputs.Where(x => x is View.UserControls.DatePickerTemplate && (x as View.UserControls.DatePickerTemplate).customDatePicker.SelectedDate == null).Any())
            {
                mainGridPage1.Visibility = Visibility.Hidden;
                mainGridPage2.Visibility = Visibility.Visible;


                newPatient._colLastName = tbLastName.tbSearchText.Text;
                newPatient._colFirstName = tbFirstName.tbSearchText.Text;
                newPatient._colMiddleName = tbMiddleName.tbSearchText.Text;
                newPatient._colBirthDay = datePickerBirthday.customDatePicker.SelectedDate;
                newPatient._colAddress = tbLivingAddress.tbSearchText.Text;
                newPatient._colProfession = tbWorkAddress.tbSearchText.Text;
                newPatient._colHospitalDate = datePickerHospitalStart.customDatePicker.SelectedDate;
                newPatient._colLeaveDate = datePickerHospitalEnd.customDatePicker.SelectedDate;

            }

        }


        private void btnPage2Next_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> checkMandatory = new List<TextBox>() 
            {
                tbClaims,
                tbEntrDiagnosis
            };
            checkMandatory.ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
            if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
            {
                checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                    .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
                return;
            }

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
            borderDepartmentHead.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            if (string.IsNullOrEmpty(tbDepartmentHead.Text))
            {

                borderDepartmentHead.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                mainGridPage3.Visibility = Visibility.Hidden;
                mainGridPage4.Visibility = Visibility.Visible;


                newPatient._fieldOperationName = tbOperationName.tbSearchText.Text;
                newPatient._fieldOperationDate = dateOperation.customDatePicker.SelectedDate;
                newPatient._fieldChemotherapy = tbChemotherapyName.tbSearchText.Text;
                newPatient._fieldChemotherapyDate = dateChemotherapy.customDatePicker.SelectedDate;
                newPatient._fieldHistology = tbHistology.tbSearchText.Text;
                newPatient._fieldDoctor = (comboBoxDoctorName.Items[comboBoxDoctorName.SelectedIndex] as ComboBoxItem).Content.ToString();
                newPatient._fieldDepartmentHead = tbDepartmentHead.Text;
                newPatient._fieldDepartHeadAssistant = tbDepartHeadAssistant.Text;
            }
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
            List<CheckBox> cbOverallItem10List = new List<CheckBox>() {
                cbOverallItem10_1,
                cbOverallItem10_2,
                cbOverallItem10_3,
                cbOverallItem10_4,
                cbOverallItem10_5,
                cbOverallItem10_6
            };
            List<CheckBox> cbOverallItem14List = new List<CheckBox>() {
                cbOverallItem14_1,
                cbOverallItem14_2,
                cbOverallItem14_3,
                cbOverallItem14_4,
                cbOverallItem14_5
            };
            borderOverallItem10.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 170, 170, 170));
            borderOverallItem11.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            borderOverallItem12.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            borderOverallItem14.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 170, 170, 170));
            borderOverallItem15.BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170));
            if (string.IsNullOrEmpty(tbOverallItem11.Text)
                || string.IsNullOrEmpty(tbOverallItem12.Text)
                || string.IsNullOrEmpty(tbOverallItem15.Text)
                || !cbOverallItem10List.Where(x => (bool)x.IsChecked).Any()
                || !cbOverallItem14List.Where(x => (bool)x.IsChecked).Any())
            {
                if (string.IsNullOrEmpty(tbOverallItem11.Text)) borderOverallItem11.BorderBrush = new SolidColorBrush(Colors.Red);
                if (string.IsNullOrEmpty(tbOverallItem12.Text)) borderOverallItem12.BorderBrush = new SolidColorBrush(Colors.Red);
                if (string.IsNullOrEmpty(tbOverallItem15.Text)) borderOverallItem15.BorderBrush = new SolidColorBrush(Colors.Red);
                if (!cbOverallItem10List.Where(x => (bool)x.IsChecked).Any()) borderOverallItem10.BorderBrush = new SolidColorBrush(Colors.Red);
                if (!cbOverallItem14List.Where(x => (bool)x.IsChecked).Any()) borderOverallItem14.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }

            mainGridPage5.Visibility = Visibility.Hidden;
            mainGridPage6.Visibility = Visibility.Visible;

            var contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem9)
                                                 .OfType<RadioButton>()
                                                 .ToList()
                                                 .Where(x => x.GroupName == "rbOverallItem9_1")
                                                 .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem9_1 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Негативний" ? false : true
                                                : null;
            contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem9)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbOverallItem9_2")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldOverallItem9_2 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Справа" ? false : true
                                                : null;

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
            List<TextBox> checkMandatory = new List<TextBox>()
            {
                tbLifeAnamnesisItem1,
                tbLifeAnamnesisItem2,
                tbLifeAnamnesisItem3,
                tbLifeAnamnesisItem4,
                tbLifeAnamnesisItem5,
                tbLifeAnamnesisItem6
            };
            checkMandatory.ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
            if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
            {
                checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                    .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
                return;
            }

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
            List<TextBox> checkMandatory = new List<TextBox>()
            {
                tbLocusMorbiItem1,
                tbLocusMorbiItem2,
                tbLocusMorbiItem4,
                tbLocusMorbiItem6,
                tbLocusMorbiItem7
            };

            checkMandatory.ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
            if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
            {
                checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                    .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
                return;
            }

            newPatient._fieldLocusMorbiItem1 = tbLocusMorbiItem1.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldLocusMorbiItem2 = tbLocusMorbiItem2.Text.Trim().TrimEnd(',').Split(", ");

            var contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbLocusMorbiItem3")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldLocusMorbiItem3 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Везикулярне" ? false : true
                                                : null;

            newPatient._fieldLocusMorbiItem4 = tbLocusMorbiItem4.Text.Trim().TrimEnd(',').Split(", ");

            contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbLocusMorbiItem5")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldLocusMorbiItem5 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Легеневий звук" ? false : true
                                                : null;
            newPatient._fieldLocusMorbiItem6 = tbLocusMorbiItem6.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldLocusMorbiItem7 = tbLocusMorbiItem7.Text.Trim().TrimEnd(',').Split(", ");

            MessageBoxResult dialogResult = MessageBox.Show($"Додати паціента {newPatient._colLastName} {newPatient._colFirstName} до бази?", "Перевірка", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (dialogResult == MessageBoxResult.OK)
            {
                try
                {
                    string fileName = "..\\..\\..\\database.json";
                    string jsonString = JsonSerializer.Serialize<DataItem>(newPatient);
                    File.AppendAllText(fileName, jsonString + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Виникла помилка при записі в базу даних!", "Помилка запису в базу", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }

                ((App)Application.Current).AddDataToStorage(newPatient);
                this.Close();
            }


        }

        private void btnPage8Save_Changes(object sender, RoutedEventArgs e)
        {
            List<TextBox> checkMandatory = new List<TextBox>()
            {
                tbLocusMorbiItem1,
                tbLocusMorbiItem2,
                tbLocusMorbiItem4,
                tbLocusMorbiItem6,
                tbLocusMorbiItem7
            };

            checkMandatory.ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
            if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
            {
                checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                    .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
                return;
            }

            newPatient._fieldLocusMorbiItem1 = tbLocusMorbiItem1.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldLocusMorbiItem2 = tbLocusMorbiItem2.Text.Trim().TrimEnd(',').Split(", ");

            var contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbLocusMorbiItem3")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldLocusMorbiItem3 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Везикулярне" ? false : true
                                                : null;

            newPatient._fieldLocusMorbiItem4 = tbLocusMorbiItem4.Text.Trim().TrimEnd(',').Split(", ");

            contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                                                             .OfType<RadioButton>()
                                                             .ToList()
                                                             .Where(x => x.GroupName == "rbLocusMorbiItem5")
                                                             .FirstOrDefault(x => (bool)x.IsChecked);
            newPatient._fieldLocusMorbiItem5 = contentHolder != null
                                                ? contentHolder.Content.ToString() == "Легеневий звук" ? false : true
                                                : null;
            newPatient._fieldLocusMorbiItem6 = tbLocusMorbiItem6.Text.Trim().TrimEnd(',').Split(", ");
            newPatient._fieldLocusMorbiItem7 = tbLocusMorbiItem7.Text.Trim().TrimEnd(',').Split(", ");

            MessageBoxResult dialogResult = MessageBox.Show($"Додати зміни до даних паціента {newPatient._colLastName} {newPatient._colFirstName}?", "Перевірка", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (dialogResult == MessageBoxResult.OK)
            {
                string fileName = "..\\..\\..\\database.json";
                if (File.Exists(fileName))
                {
                    try
                    {
                        var oldLines = File.ReadAllLines(fileName);
                        var newLines = oldLines.Where(line => !line.Contains($"{newPatient._colCardNumber}"));
                        File.WriteAllLines(fileName, newLines);

                        string jsonString = JsonSerializer.Serialize<DataItem>(newPatient);
                        File.AppendAllText(fileName, jsonString + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Виникла помилка при читанні з бази даних!", "Помилка читання з бази", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    }
                }

                ((App)Application.Current).EditDataInStorage(newPatient);
                this.Close();
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

        private void tbClaims_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbEntrDiagnosis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbFinalDiagnosis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbComplications_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAdditionDiagnosis_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbDepartmentHead_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ'. ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbDepartHeadAssistant_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ'. ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbOverallItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9\,\.]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbOverallItem11_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbOverallItem12_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9\/]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbOverallItem15_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem5_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem6_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem8_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem9_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem10_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbAnamnesisItem11_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem3_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem5_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem6_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem8_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9/ ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem9_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9/ ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLifeAnamnesisItem10_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLocusMorbiItem1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLocusMorbiItem2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLocusMorbiItem4_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLocusMorbiItem6_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }

        private void tbLocusMorbiItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}