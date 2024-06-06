using EasyDox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for DocBuilder.xaml
    /// </summary>
    public partial class DocBuilder : Window
    {
        DataItem sourceData = new DataItem();
        public DocBuilder()
        {
            InitializeComponent();

            btnCreateF1.btnClick += btnCreateF1_Click;
            btnCreateF2.btnClick += btnCreateF2_Click;
        }
        public DocBuilder(DataItem dataObject)
        {

            InitializeComponent();

            sourceData = dataObject;

            btnCreateF1.btnClick += btnCreateF1_Click;
            btnCreateF2.btnClick += btnCreateF2_Click;
        }



        private void btnCreateF1_Click(object sender, RoutedEventArgs e)
        {
            var claimsBuilder = sourceData._fieldClaims != null ? string.Join(", ", sourceData._fieldClaims) : "Text";
            var anamnesisBuilder = "";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem1)) anamnesisBuilder += sourceData._fieldAnamnesisItem1 + ", ";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem2)) anamnesisBuilder += sourceData._fieldAnamnesisItem2 + ", ";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem3)) anamnesisBuilder += sourceData._fieldAnamnesisItem3 + ", ";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem4)) anamnesisBuilder += sourceData._fieldAnamnesisItem4 + ", ";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem5)) anamnesisBuilder += sourceData._fieldAnamnesisItem5 + ", ";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem6)) anamnesisBuilder += sourceData._fieldAnamnesisItem6 + ", ";
            if (!string.IsNullOrEmpty(sourceData._fieldAnamnesisItem7)) anamnesisBuilder += sourceData._fieldAnamnesisItem7 + ", ";
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
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = $"Виписка_ОГП_ОЦО_{sourceData._colLastName}_{sourceData._colFirstName}_{sourceData._colMiddleName}";
                dialog.DefaultExt = ".docx";
                dialog.Filter = "Word Documents (.docx)|*.docx";

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    string filename = dialog.FileName;
                    engine.Merge("..\\..\\..\\Виписка ОГП ОЦО_template.docx", fieldValues, filename);
                    MessageBox.Show("Документ було створено!", "Документ", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(filename)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
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
            this.Close();
        }

        private void btnCreateF2_Click(object sender, RoutedEventArgs e)
        {
            
             Dictionary<string, string> fieldValues = new Dictionary<string, string> {
               {"_docCardNumber", $"{sourceData._colCardNumber}"},
               {"_docHospitalStartDate",  $"{sourceData._colHospitalDate.ToString().Split(" ")[0]}"},
               {"_docLastFirstMiddleName", $"{sourceData._colLastName} {sourceData._colFirstName} {sourceData._colMiddleName}"},
               {"_docBirthDay", $"{sourceData._colBirthDay.ToString().Split(" ")[0]}"},
               {"_docAge", $"{sourceData._colAge.ToString()}"},
               {"_docLivingAddress", $"{sourceData._colAddress}"},
               {"_docProfession", $"{sourceData._colProfession}"},
               {"_docHospitalEndDate", $"{sourceData._colLeaveDate.ToString().Split(" ")[0]}"},
               {"_docDiagnosisMain", $"{sourceData._fieldFinalDiagnosis}"},
               {"_docMKX", $"{sourceData._fieldMKX}"},
               {"_docFirstOpertaionDate", $"{sourceData._fieldOperationDate.ToString().Split(" ")[0]}"},
               {"_docFirstOpertaionName", $"{sourceData._fieldOperationName}"},
               {"_docSecOpertaionDate", $""},
               {"_docSecOpertaionName", $""},
               {"_docDoctor", $"{sourceData._fieldDoctor}"},
               {"_docCreationDate", $"{DateTime.Now.ToString().Split(" ")[0]}"}
                };
            var engine = new Engine();
            try
            {
                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.FileName = $"Виписка_066_{sourceData._colLastName}_{sourceData._colFirstName}_{sourceData._colMiddleName}";
                dialog.DefaultExt = ".docx";
                dialog.Filter = "Word Documents (.docx)|*.docx";

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    string filename = dialog.FileName;
                    engine.Merge("..\\..\\..\\Виписка 066_template.docx", fieldValues, filename);
                    MessageBox.Show("Документ було створено!", "Документ", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    var p = new Process();
                    p.StartInfo = new ProcessStartInfo(filename)
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
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
            this.Close();
            
        }
    }
}
