using EasyDox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// Interaction logic for PatientDescription.xaml
    /// </summary>
    public partial class PatientDescription : Window
    {
        DataItem sourceData = new DataItem();
        public PatientDescription()
        {
            InitializeComponent();

            tbLastName.TextChanged += AddTextLastName;
            tbFirstName.TextChanged += AddTextFirstName;
            tbMiddleName.TextChanged += AddTextMiddleName;
            tbLivingAddress.TextChanged += AddTextLivingAddress;
            tbWork.TextChanged += AddTextWork;
            tbMKX.TextChanged += AddTextMKX;

            tbOperationName.TextChanged += AddTextOperationName;
            tbChemotherapyName.TextChanged += AddTextChemotherapyName;
            tbHistology.TextChanged += AddTextHistology;
            tbDoctorName.TextChanged += AddTextDoctorName;
            tbDepartmentHead.TextChanged += AddTextDepartmentHead;
            tbDepartHeadAssistant.TextChanged += AddTextDepartHeadAssistant;


            btnGenerateDoc.btnClick += btnGenerateDocument;
        }
        
        public PatientDescription(DataItem dataObject)
        {
            InitializeComponent();
           
            sourceData = dataObject;

            
            tbLastName.TextChanged += AddTextLastName;
            tbFirstName.TextChanged += AddTextFirstName;
            tbMiddleName.TextChanged += AddTextMiddleName;
            tbLivingAddress.TextChanged += AddTextLivingAddress;
            tbWork.TextChanged += AddTextWork;
            tbMKX.TextChanged += AddTextMKX;

            tbOperationName.TextChanged += AddTextOperationName;
            tbChemotherapyName.TextChanged += AddTextChemotherapyName;
            tbHistology.TextChanged += AddTextHistology;
            tbDoctorName.TextChanged += AddTextDoctorName;
            tbDepartmentHead.TextChanged += AddTextDepartmentHead;
            tbDepartHeadAssistant.TextChanged += AddTextDepartHeadAssistant;

            btnGenerateDoc.btnClick += btnGenerateDocument;
            btnChangeDesc.btnClick += btnChangeDescription;

        }

        private void btnGenerateDocument(object sender, RoutedEventArgs e)
        {
            
            var claimsBuilder = sourceData._fieldClaims != null ? string.Join(", ", sourceData._fieldClaims) : "Text";
            var anamnesisBuilder = "";
            if (sourceData._fieldAnamnesisItem1 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem1 + ", ";
            }
            if(sourceData._fieldAnamnesisItem2 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem2 + ", ";
            }
            if (sourceData._fieldAnamnesisItem3 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem3 + ", ";
            }
            if(sourceData._fieldAnamnesisItem4 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem4 + ", ";
            }
            if (sourceData._fieldAnamnesisItem5 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem5 + ", ";
            }
            if(sourceData._fieldAnamnesisItem6 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem6 + ", ";
            }
            if (sourceData._fieldAnamnesisItem7 != null)
            {
                anamnesisBuilder += sourceData._fieldAnamnesisItem7 + ", ";
            }
            anamnesisBuilder = anamnesisBuilder.Trim().TrimEnd(',');
            Dictionary<string, string> fieldValues = new Dictionary<string, string> {
                {"_docNumber", "456"},
                {"_docNumberAdditional",  "123"},
                {"_docLastFirstMiddleName", $"{sourceData._colLastName} {sourceData._colFirstName} {sourceData._colMiddleName}"},
                {"_docBirthDay", $"{sourceData._colBirthDay.ToString().Split(" ")[0]}"},
                {"_docLivingAddress", $"{sourceData._colAddress}"},
                {"_docProfession", $"{sourceData._colProfession}"},
                {"_docHospitalStartDate", $"{sourceData._colHospitalDate.ToString().Split(" ")[0]}"},
                {"_docHospitalEndDate", $"{sourceData._colLeaveDate.ToString().Split(" ")[0]}"},
                {"_docDiagnosisMain", $"{sourceData._fieldFinalDiagnosis}"},
                {"_docComplication", $"{sourceData._fieldComplication}"},
                {"_docAdditionalDiagnosis", $"{sourceData._fieldAdditionalDiagnosis}"},
                {"_docClaims", $"{claimsBuilder}"},
                {"_docAnamnesis", $"{anamnesisBuilder}"},
                {"_docAnamnesisItem8", $"{sourceData._fieldAnamnesisItem8}"},
                {"_docAnamnesisItem9", $"{sourceData._fieldAnamnesisItem9}"},
                {"_docAnamnesisItem10", $"{sourceData._fieldAnamnesisItem10}"},
                {"_docAnamnesisItem11", $"{sourceData._fieldAnamnesisItem11}"},
                {"_docHistology", $"{sourceData._fieldHistology}"},
                {"_docChemotherapyDate", $"{sourceData._fieldChemotherapyDate.ToString().Split(" ")[0]}"},
                {"_docChemotherapy", $"{sourceData._fieldChemotherapy}"},
                {"_docCreationDate", $"{DateTime.Now.ToString().Split(" ")[0]}"},
                {"_docDoctor", $"{sourceData._fieldDoctor}"}
            };
            var engine = new Engine();
            try
            {
                engine.Merge("..\\..\\..\\Виписка ОГП ОЦО_template.docx", fieldValues, $"..\\..\\..\\Виписка_ОГП_ОЦО_{sourceData._colLastName}_{sourceData._colFirstName}_{sourceData._colMiddleName}.docx");
                MessageBox.Show("Документ було створено!", "Документ", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                var p = new Process();
                p.StartInfo = new ProcessStartInfo($"..\\..\\..\\Виписка_ОГП_ОЦО_{sourceData._colLastName}_{sourceData._colFirstName}_{sourceData._colMiddleName}.docx")
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("because it is being used by another process."))
                {
                    MessageBox.Show("Скоріш за все документ вже створений та відкритий! Перевірте будь-ласка запущені процеси.", "Документ вже відкритий!", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
                else if (ex.Message.Contains("Could not find file "))
                {
                    MessageBox.Show("Шаблон документу не знайдено. Будь-ласка зверніться до підтримки!", "Шаблон не знайдено!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show("Неочікувана помилка. Будь-ласка зверніться до підтримки!", "Шаблон не знайдено!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
            }
        }

        private void btnChangeDescription(object sender, RoutedEventArgs e)
        {
            AddPatient infoEditor = new AddPatient(sourceData);
            infoEditor.ShowDialog();
        }

        private void reloadData (DataItem source)
        {
            lbTypDescriptionItem1.Content = "Прізвище: " + source._colLastName;
            lbTypDescriptionItem2.Content = "Ім'я: " + source._colFirstName;
            lbTypDescriptionItem3.Content = "По-батькові: " + source._colMiddleName;
            lbTypDescriptionItem4.Content = "Дата народження: " + source._colBirthDay.ToString().Split(" ")[0];
            lbTypDescriptionItem5.Content = "Вік: " + source._colAge;

            tbLastName.Text = source._colLastName;
            tbFirstName.Text = source._colFirstName;
            tbMiddleName.Text = source._colMiddleName;
            tbBirthday.Text = source._colBirthDay.ToString().Split(" ")[0];
            tbLivingAddress.Text = source._colAddress;
            tbWork.Text = source._colProfession;
            tbHospitalStart.Text = source._colHospitalDate.ToString().Split(" ")[0];
            tbHospitalEnd.Text = source._colLeaveDate.ToString().Split(" ")[0];

            tbClims.Text = source._fieldClaims != null
                ? string.Join(", ", source._fieldClaims)
                : "--------";

            tbEntrDiagnosis.Text = source._fieldEntrDiagnosis != null
                ? source._fieldEntrDiagnosis
                : "--------";
            tbFinalDiagnosis.Text = source._fieldFinalDiagnosis != null
               ? source._fieldFinalDiagnosis
               : "--------";
            tbComplications.Text = source._fieldComplication != null
                ? source._fieldComplication
                : "--------";
            tbAdditionDiagnosis.Text = source._fieldAdditionalDiagnosis != null
                ? source._fieldAdditionalDiagnosis
                : "--------";
            tbMKX.Text = source._fieldMKX != null
                ? source._fieldMKX
                : "--------";
            tbOperationName.Text = source._fieldOperationName != null
                ? source._fieldOperationName
                : "--------";


            dateOperation.Text = source._fieldOperationDate != null
                ? source._fieldOperationDate.ToString().Split(" ")[0]
                : "--.--.----";

            tbChemotherapyName.Text = source._fieldChemotherapy != null
                ? source._fieldChemotherapy
                : "--------";
            dateChemotherapy.Text = source._fieldChemotherapyDate != null
                ? source._fieldChemotherapyDate.ToString().Split(" ")[0]
                : "--.--.----";
            tbHistology.Text = source._fieldHistology != null
                ? source._fieldHistology
                : "--------";
            tbDoctorName.Text = source._fieldDoctor != null
                ? source._fieldDoctor
                : "--------";
            tbDepartmentHead.Text = source._fieldDepartmentHead != null
                ? source._fieldDepartmentHead
                : "--------";
            tbDepartHeadAssistant.Text = source._fieldDepartHeadAssistant != null
                ? source._fieldDepartHeadAssistant
                : "--------";




            tbOverallItem1.Text = source._fieldOverallItem1 != null
                ? ((App)Application.Current).dictOverallItem1[(int)source._fieldOverallItem1]
                : "--------";
            tbOverallItem2.Text = source._fieldOverallItem2 != null
                ? ((App)Application.Current).dictOverallItem2[(int)source._fieldOverallItem2]
                : "--------";
            tbOverallItem3.Text = source._fieldOverallItem3 != null
                ? ((App)Application.Current).dictOverallItem3[(int)source._fieldOverallItem3]
                : "--------";
            tbOverallItem4.Text = source._fieldOverallItem4 != null
                ? ((App)Application.Current).dictOverallItem4[(int)source._fieldOverallItem4]
                : "--------";
            tbOverallItem5.Text = source._fieldOverallItem5 != null
                ? ((App)Application.Current).dictOverallItem5[(int)source._fieldOverallItem5]
                : "--------";
            tbOverallItem6.Text = source._fieldOverallItem6 != null
                ? ((App)Application.Current).dictOverallItem6[(int)source._fieldOverallItem6]
                : "--------";
            tbOverallItem7.Text = source._fieldOverallItem7 != null
                ? source._fieldOverallItem7
                : "--------";
            tbOverallItem8.Text = source._fieldOverallItem8 != null
                ? ((App)Application.Current).dictOverallItem8[(int)source._fieldOverallItem8]
                : "--------";


            tbOverallItem9_1.Text = source._fieldOverallItem9_1 == true
                ? "Позитивний"
                : "Негативний";


            if (source._fieldOverallItem9_1 == true)
            {
                tbOverallItem9_2.Text = source._fieldOverallItem9_2 == true
                    ? "Зліва"
                    : "Зправа";
            }
            else { tbOverallItem9_2.Text = "---------"; }

            tbOverallItem10.Text = source._fieldOverallItem10 != null
                ? string.Join(", ", source._fieldOverallItem10.ToList()
                                                                  .Select(x => ((App)Application.Current).dictOverallItem10[x]).ToArray())
                : "--------";
            tbOverallItem11.Text = source._fieldOverallItem11 != null
                ? source._fieldOverallItem11.ToString()
                : "--------";
            tbOverallItem12.Text = source._fieldOverallItem12 != null
                ? source._fieldOverallItem12
                : "--------";
            tbOverallItem13.Text = source._fieldOverallItem13 != null
                ? ((App)Application.Current).dictOverallItem13[(int)source._fieldOverallItem13]
                : "--------";
            tbOverallItem14.Text = source._fieldOverallItem14 != null
                ? string.Join(", ", source._fieldOverallItem14.ToList()
                                                                  .Select(x => ((App)Application.Current).dictOverallItem14[x]).ToArray())
                : "--------";
            tbOverallItem15.Text = source._fieldOverallItem15 != null
                ? source._fieldOverallItem15.ToString()
                : "--------";



            tbAnamnesisItem1.Text = source._fieldAnamnesisItem1 != null
                ? source._fieldAnamnesisItem1
                : "--------";
            tbAnamnesisItem2.Text = source._fieldAnamnesisItem2 != null
                ? source._fieldAnamnesisItem2
                : "--------";
            tbAnamnesisItem3.Text = source._fieldAnamnesisItem3 != null
                ? source._fieldAnamnesisItem3
                : "--------";
            tbAnamnesisItem4.Text = source._fieldAnamnesisItem4 != null
                ? source._fieldAnamnesisItem4
                : "--------";
            tbAnamnesisItem5.Text = source._fieldAnamnesisItem5 != null
                ? source._fieldAnamnesisItem5
                : "--------";
            tbAnamnesisItem6.Text = source._fieldAnamnesisItem6 != null
                ? source._fieldAnamnesisItem6
                : "--------";
            tbAnamnesisItem7.Text = source._fieldAnamnesisItem7 != null
                ? source._fieldAnamnesisItem7
                : "--------";
            tbAnamnesisItem8.Text = source._fieldAnamnesisItem8 != null
                ? source._fieldAnamnesisItem8
                : "--------";
            tbAnamnesisItem9.Text = source._fieldAnamnesisItem9 != null
                ? source._fieldAnamnesisItem9
                : "--------";
            tbAnamnesisItem10.Text = source._fieldAnamnesisItem10 != null
                ? source._fieldAnamnesisItem10
                : "--------";
            tbAnamnesisItem11.Text = source._fieldAnamnesisItem11 != null
                ? source._fieldAnamnesisItem11
                : "--------";

            tbLifeAnamnesisItem1.Text = source._fieldLifeAnamnesisItem1 != null
                ? source._fieldLifeAnamnesisItem1
                : "--------";
            tbLifeAnamnesisItem2.Text = source._fieldLifeAnamnesisItem2 != null
                ? source._fieldLifeAnamnesisItem2
                : "--------";
            tbLifeAnamnesisItem3.Text = source._fieldLifeAnamnesisItem3 != null
                ? source._fieldLifeAnamnesisItem3
                : "--------";
            tbLifeAnamnesisItem4.Text = source._fieldLifeAnamnesisItem4 != null
                ? source._fieldLifeAnamnesisItem4
                : "--------";
            tbLifeAnamnesisItem5.Text = source._fieldLifeAnamnesisItem5 != null
                ? source._fieldLifeAnamnesisItem5
                : "--------";
            tbLifeAnamnesisItem6.Text = source._fieldLifeAnamnesisItem6 != null
                ? source._fieldLifeAnamnesisItem6
                : "--------";
            tbLifeAnamnesisItem7.Text = source._fieldLifeAnamnesisItem7 != null
                ? source._fieldLifeAnamnesisItem7
                : "--------";
            tbLifeAnamnesisItem8.Text = source._fieldLifeAnamnesisItem8 != null
                ? source._fieldLifeAnamnesisItem8.ToString()
                : "--------";
            tbLifeAnamnesisItem9.Text = source._fieldLifeAnamnesisItem9 != null
                ? source._fieldLifeAnamnesisItem9.ToString()
                : "--------";
            tbLifeAnamnesisItem10.Text = source._fieldLifeAnamnesisItem10 != null
                ? source._fieldLifeAnamnesisItem10
                : "--------";


            tbLocusMorbiItem1.Text = source._fieldLocusMorbiItem1 != null
                ? string.Join(", ", source._fieldLocusMorbiItem1)
                : "--------";
            tbLocusMorbiItem2.Text = source._fieldLocusMorbiItem2 != null
                ? string.Join(", ", source._fieldLocusMorbiItem2)
                : "--------";
            if (source._fieldLocusMorbiItem3 != null)
            {
                tbLocusMorbiItem3.Text = source._fieldLocusMorbiItem3 == true
                ? "Везикулярне"
                : "Жорстке";
            }
            else { tbLocusMorbiItem3.Text = "--------"; }

            tbLocusMorbiItem4.Text = source._fieldLocusMorbiItem4 != null
                ? string.Join(", ", source._fieldLocusMorbiItem4)
                : "--------";

            if (source._fieldLocusMorbiItem5 != null)
            {
                tbLocusMorbiItem5.Text = source._fieldLocusMorbiItem5 == true
                ? "Легеневий звук"
                : "Коробчатий звук";
            }
            else { tbLocusMorbiItem5.Text = "--------"; }

            tbLocusMorbiItem6.Text = source._fieldLocusMorbiItem6 != null
                ? string.Join(", ", source._fieldLocusMorbiItem6)
                : "--------";
            tbLocusMorbiItem7.Text = source._fieldLocusMorbiItem7 != null
                ? string.Join(", ", source._fieldLocusMorbiItem7)
                : "--------";
        }
            
        private void typicalDescription_Loaded(object sender, RoutedEventArgs e)
        {
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
        private void AddTextWork(object sender, TextChangedEventArgs e)
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
        private void AddTextDoctorName(object sender, TextChangedEventArgs e)
        {
        }
        private void AddTextDepartmentHead(object sender, TextChangedEventArgs e)
        {
        }
        private void AddTextDepartHeadAssistant(object sender, TextChangedEventArgs e)
        {
        }

        private void datePickerBirthday_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (sourceData != null && sourceData != new DataItem())
            {
                if (((App)Application.Current).GetDataItemsByID(sourceData._colCardNumber) != null)
                {
                    sourceData = ((App)Application.Current).GetDataItemsByID(sourceData._colCardNumber);
                }
                reloadData(sourceData);
            }
        }
    }
}

