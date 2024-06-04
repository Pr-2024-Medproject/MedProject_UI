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
    public partial class PatientDescription : System.Windows.Window
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

            lbTypDescriptionItem1.Content = "Прізвище: " + dataObject._colLastName;
            lbTypDescriptionItem2.Content = "Ім'я: " + dataObject._colFirstName;
            lbTypDescriptionItem3.Content = "По-батькові: " + dataObject._colMiddleName;
            lbTypDescriptionItem4.Content = "Дата народження: " + dataObject._colBirthDay.ToString().Split(" ")[0];
            lbTypDescriptionItem5.Content = "Вік: " + dataObject._colAge;

            tbLastName.Text = dataObject._colLastName;
            tbFirstName.Text = dataObject._colFirstName;
            tbMiddleName.Text = dataObject._colMiddleName;
            tbBirthday.Text = dataObject._colBirthDay.ToString().Split(" ")[0];
            tbLivingAddress.Text = dataObject._colAddress;
            tbWork.Text = dataObject._colProfession;
            tbHospitalStart.Text = dataObject._colHospitalDate.ToString().Split(" ")[0];
            tbHospitalEnd.Text = dataObject._colLeaveDate.ToString().Split(" ")[0];

            tbClims.Text =  dataObject._fieldClaims != null
                ? string.Join(", ", dataObject._fieldClaims)
                : "--------";

            tbEntrDiagnosis.Text = dataObject._fieldEntrDiagnosis != null
                ? dataObject._fieldEntrDiagnosis
                : "--------";
            tbFinalDiagnosis.Text = dataObject._fieldFinalDiagnosis != null
               ? dataObject._fieldFinalDiagnosis
               : "--------";
            tbComplications.Text = dataObject._fieldComplication != null
                ?  dataObject._fieldComplication
                : "--------";
            tbAdditionDiagnosis.Text = dataObject._fieldAdditionalDiagnosis != null
                ? dataObject._fieldAdditionalDiagnosis
                : "--------";
            tbMKX.Text = dataObject._fieldMKX != null
                ?  dataObject._fieldMKX
                : "--------";
            tbOperationName.Text = dataObject._fieldOperationName != null
                ?  dataObject._fieldOperationName
                : "--------";


            dateOperation.Text = dataObject._fieldOperationDate != null
                ? dataObject._fieldOperationDate.ToString().Split(" ")[0]
                : "--.--.----";

            tbChemotherapyName.Text = dataObject._fieldChemotherapy != null
                ? dataObject._fieldChemotherapy
                : "--------";
            dateChemotherapy.Text = dataObject._fieldChemotherapyDate != null
                ? dataObject._fieldChemotherapyDate.ToString().Split(" ")[0]
                : "--.--.----";
            tbHistology.Text = dataObject._fieldHistology != null
                ? dataObject._fieldHistology
                : "--------";
            tbDoctorName.Text = dataObject._fieldDoctor != null
                ? dataObject._fieldDoctor
                : "--------";
            tbDepartmentHead.Text = dataObject._fieldDepartmentHead != null
                ? dataObject._fieldDepartmentHead
                : "--------";
            tbDepartHeadAssistant.Text = dataObject._fieldDepartHeadAssistant != null
                ? dataObject._fieldDepartHeadAssistant
                : "--------";




            tbOverallItem1.Text = dataObject._fieldOverallItem1 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem1[(int)dataObject._fieldOverallItem1]
                : "--------";
            tbOverallItem2.Text = dataObject._fieldOverallItem2 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem2[(int)dataObject._fieldOverallItem2]
                : "--------";
            tbOverallItem3.Text = dataObject._fieldOverallItem3 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem3[(int)dataObject._fieldOverallItem3]
                : "--------";
            tbOverallItem4.Text = dataObject._fieldOverallItem4 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem4[(int)dataObject._fieldOverallItem4]
                : "--------";
            tbOverallItem5.Text = dataObject._fieldOverallItem5 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem5[(int)dataObject._fieldOverallItem5]
                : "--------";
            tbOverallItem6.Text = dataObject._fieldOverallItem6 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem6[(int)dataObject._fieldOverallItem6]
                : "--------";
            tbOverallItem7.Text = dataObject._fieldOverallItem7 != null
                ? dataObject._fieldOverallItem7
                : "--------";
            tbOverallItem8.Text = dataObject._fieldOverallItem8 != null
                ? ((App)System.Windows.Application.Current).dictOverallItem1[(int)dataObject._fieldOverallItem8]
                : "--------";

            
                tbOverallItem9_1.Text = dataObject._fieldOverallItem9_1 == true
                ? "Позитивний"
                : "Негативний";
            
            
            if (dataObject._fieldOverallItem9_1 == true )
            {
                tbOverallItem9_2.Text = dataObject._fieldOverallItem9_2 == true
                    ? "Зліва"
                    : "Зправа";
            }
            else { tbOverallItem9_2.Text = "---------"; }

            tbOverallItem10.Text = dataObject._fieldOverallItem10 != null
                ? string.Join(", ", dataObject._fieldOverallItem10.ToList()
                                                                  .Select(x => ((App)System.Windows.Application.Current).dictOverallItem10[x]).ToArray())
                : "--------";
            tbOverallItem11.Text = dataObject._fieldOverallItem11 != null
                ? dataObject._fieldOverallItem11.ToString()
                : "--------";
            tbOverallItem12.Text = dataObject._fieldOverallItem12 != null
                ? dataObject._fieldOverallItem12
                : "--------";
            tbOverallItem13.Text = dataObject._fieldOverallItem13 != null
                ? dataObject._fieldOverallItem13.ToString()
                : "--------";
            tbOverallItem14.Text = dataObject._fieldOverallItem14 != null
                ? string.Join(", ", dataObject._fieldOverallItem14.ToList()
                                                                  .Select(x => ((App)System.Windows.Application.Current).dictOverallItem14[x]).ToArray())
                : "--------";
            tbOverallItem15.Text = dataObject._fieldOverallItem15 != null
                ? dataObject._fieldOverallItem15.ToString()
                : "--------";



            tbAnamnesisItem1.Text = dataObject._fieldAnamnesisItem1 != null
                ? dataObject._fieldAnamnesisItem1
                : "--------";
            tbAnamnesisItem2.Text = dataObject._fieldAnamnesisItem2 != null
                ? dataObject._fieldAnamnesisItem2
                : "--------";
            tbAnamnesisItem3.Text = dataObject._fieldAnamnesisItem3 != null
                ? dataObject._fieldAnamnesisItem3
                : "--------";
            tbAnamnesisItem4.Text = dataObject._fieldAnamnesisItem4 != null
                ? dataObject._fieldAnamnesisItem4
                : "--------";
            tbAnamnesisItem5.Text = dataObject._fieldAnamnesisItem5 != null
                ? dataObject._fieldAnamnesisItem5
                : "--------";
            tbAnamnesisItem6.Text = dataObject._fieldAnamnesisItem6 != null
                ? dataObject._fieldAnamnesisItem6
                : "--------";
            tbAnamnesisItem7.Text = dataObject._fieldAnamnesisItem7 != null
                ? dataObject._fieldAnamnesisItem7
                : "--------";
            tbAnamnesisItem8.Text = dataObject._fieldAnamnesisItem8 != null
                ? dataObject._fieldAnamnesisItem8
                : "--------";
            tbAnamnesisItem9.Text = dataObject._fieldAnamnesisItem9 != null
                ? dataObject._fieldAnamnesisItem9
                : "--------";
            tbAnamnesisItem10.Text = dataObject._fieldAnamnesisItem10 != null
                ? dataObject._fieldAnamnesisItem10
                : "--------";
            tbAnamnesisItem11.Text = dataObject._fieldAnamnesisItem11 != null
                ? dataObject._fieldAnamnesisItem11
                : "--------";

            tbLifeAnamnesisItem1.Text = dataObject._fieldLifeAnamnesisItem1 != null
                ? dataObject._fieldLifeAnamnesisItem1
                : "--------";
            tbLifeAnamnesisItem2.Text = dataObject._fieldLifeAnamnesisItem2 != null
                ? dataObject._fieldLifeAnamnesisItem2
                : "--------";
            tbLifeAnamnesisItem3.Text = dataObject._fieldLifeAnamnesisItem3 != null
                ? dataObject._fieldLifeAnamnesisItem3
                : "--------";
            tbLifeAnamnesisItem4.Text = dataObject._fieldLifeAnamnesisItem4 != null
                ? dataObject._fieldLifeAnamnesisItem4
                : "--------";
            tbLifeAnamnesisItem5.Text = dataObject._fieldLifeAnamnesisItem5 != null
                ? dataObject._fieldLifeAnamnesisItem5
                : "--------";
            tbLifeAnamnesisItem6.Text = dataObject._fieldLifeAnamnesisItem6 != null
                ? dataObject._fieldLifeAnamnesisItem6
                : "--------";
            tbLifeAnamnesisItem7.Text = dataObject._fieldLifeAnamnesisItem7 != null
                ? dataObject._fieldLifeAnamnesisItem7
                : "--------";
            tbLifeAnamnesisItem8.Text = dataObject._fieldLifeAnamnesisItem8 != null
                ? dataObject._fieldLifeAnamnesisItem8.ToString()
                : "--------";
            tbLifeAnamnesisItem9.Text = dataObject._fieldLifeAnamnesisItem9 != null
                ? dataObject._fieldLifeAnamnesisItem9.ToString()
                : "--------";
            tbLifeAnamnesisItem10.Text = dataObject._fieldLifeAnamnesisItem10 != null
                ? dataObject._fieldLifeAnamnesisItem10
                : "--------";


            tbLocusMorbiItem1.Text = dataObject._fieldLocusMorbiItem1 != null
                ? string.Join(", ", dataObject._fieldLocusMorbiItem1)
                : "--------";
            tbLocusMorbiItem2.Text = dataObject._fieldLocusMorbiItem2 != null
                ? string.Join(", ", dataObject._fieldLocusMorbiItem2)
                : "--------";
            if (dataObject._fieldLocusMorbiItem3 != null)
            {
                tbLocusMorbiItem3.Text = dataObject._fieldLocusMorbiItem3 == true
                ? "Везикулярне"
                : "Жорстке";
            }
            else { tbLocusMorbiItem3.Text = "--------"; }
                
            tbLocusMorbiItem4.Text = dataObject._fieldLocusMorbiItem4 != null
                ? string.Join(", ", dataObject._fieldLocusMorbiItem4)
                : "--------";

            if (dataObject._fieldLocusMorbiItem5 != null)
            {
                tbLocusMorbiItem5.Text = dataObject._fieldLocusMorbiItem5 == true
                ? "Легеневий звук"
                : "Коробчатий звук";
            }
            else { tbLocusMorbiItem5.Text = "--------"; }
            
            tbLocusMorbiItem6.Text = dataObject._fieldLocusMorbiItem6 != null
                ? string.Join(", ", dataObject._fieldLocusMorbiItem6)
                : "--------";
            tbLocusMorbiItem7.Text = dataObject._fieldLocusMorbiItem7 != null
                ? string.Join(", ", dataObject._fieldLocusMorbiItem7)
                : "--------";

        }

        private void btnGenerateDocument(object sender, RoutedEventArgs e)
        {
            Dictionary<string, string> fieldValues = new Dictionary<string, string> {
                {"_docNumber", "456"},
                {"_docNumberAdditional",  "123"},
                {"_docLastFirstMiddleName", $"{sourceData._colLastName} {sourceData._colFirstName} {sourceData._colMiddleName}"},
                {"_docBirthDay", $"{sourceData._colBirthDay.ToString().Split(" ")[0]}"},
                {"_docLivingAddress", $"{sourceData._colAddress}"},
                {"_docProfession", $"{sourceData._colProfession}"},
                {"_docHospitalStartDate", $"{sourceData._colHospitalDate.ToString().Split(" ")[0]}"},
                {"_docHospitalEndDate", $"{sourceData._colLeaveDate.ToString().Split(" ")[0]}"},
                {"_docDiagnosisMain", "Text"},
                {"_docComplication", "Text"},
                {"_docAdditionalDiagnosis", "Text Text"},
                {"_docClaims", "Text"},
                {"_docAnamnesis", "Text"},
                {"_docAnamnesisItem8", "Text"},
                {"_docAnamnesisItem9", "Text"},
                {"_docAnamnesisItem10", "Text"},
                {"_docAnamnesisItem11", "Text"},
                {"_docHistology", "Text"},
                {"_docChemotherapyDate", "Text"},
                {"_docChemotherapy", "Text"},
                {"_docCreationDate", $"{DateTime.Now.ToString().Split(" ")[0]}"},
                {"_docDoctor", "Г.В. Трунов"}
            };
            var engine = new Engine();
            engine.Merge("..\\..\\..\\Виписка ОГП ОЦО_template.docx", fieldValues, $"..\\..\\..\\Виписка_ОГП_ОЦО_{sourceData._colLastName}_{sourceData._colFirstName}_{sourceData._colMiddleName}.docx");
            MessageBox.Show("Документ було створено!", "Документ", MessageBoxButton.OKCancel);
            var p = new Process();
            p.StartInfo = new ProcessStartInfo($"..\\..\\..\\Виписка_ОГП_ОЦО_{sourceData._colLastName}_{sourceData._colFirstName}_{sourceData._colMiddleName}.docx")
            {
                UseShellExecute = true
            };
            p.Start();
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
    }
}

