using System.Diagnostics;
using System.Windows;
using EasyDox;
using MedProject_UI.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MedProject_UI;

public partial class DocBuilder : Window
{
    private readonly Patient _patient;
    private readonly Visit _visit;

    public DocBuilder()
    {
        InitializeComponent();
    }

    public DocBuilder(Patient patient, Visit visit = null)
    {
        InitializeComponent();
        _patient = patient;
        _visit = visit ?? new Visit();

        btnCreateF1.btnClick += BtnCreateF1_Click;
        btnCreateF2.btnClick += BtnCreateF2_Click;
    }

    private string GetSymptom(string key, string fallback = "--------")
    {
        var visit = _visit;
        if (visit.Symptoms == null || !visit.Symptoms.TryGetValue(key, out var value))
            return fallback;

        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(value);
            if (list != null)
                return string.Join("\n• ", list.Prepend(""));
        }
        catch
        {
            if (key == "_fieldClaims")
                MessageBox.Show(
                    "Скарги не будуть виведені коректно, оскільки збережені в неправильному форматі.",
                    "Попередження",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
        }

        return value;
    }

    private void BtnCreateF1_Click(object sender, RoutedEventArgs e)
    {
        var claims = FormatList(GetSymptom("_fieldClaims"));
        var anamnesisItems = new[]
        {
            "_fieldAnamnesisItem1", "_fieldAnamnesisItem2", "_fieldAnamnesisItem3",
            "_fieldAnamnesisItem4", "_fieldAnamnesisItem5", "_fieldAnamnesisItem6",
            "_fieldAnamnesisItem7"
        };
        var anamnesisBuilder = string.Join(", ", anamnesisItems
            .Select(key => GetSymptom(key))
            .Where(val => !string.IsNullOrWhiteSpace(val)));

        var fieldValues = new Dictionary<string, string>
        {
            { "_docNumber", "456" },
            { "_docNumberAdditional", "123" },
            { "_docLastFirstMiddleName", $"{_patient.LastName} {_patient.FirstName} {_patient.MiddleName}" },
            { "_docBirthDay", _patient.BirthDate.ToString("dd.MM.yyyy") },
            { "_docLivingAddress", _patient.Address },
            { "_docProfession", _patient.Profession },
            { "_docHospitalStartDate", _visit.StartDate.ToString("dd.MM.yyyy") ?? "" },
            { "_docHospitalEndDate", _visit.EndDate.ToString("dd.MM.yyyy") ?? "" },
            { "_docDiagnosisMain", GetSymptom("_fieldFinalDiagnosis") },
            { "_docComplication", GetSymptom("_fieldComplication") },
            { "_docAdditionalDiagnosis", GetSymptom("_fieldAdditionalDiagnosis") },
            { "_docClaims", claims },
            { "_docAnamnesis", anamnesisBuilder },
            { "_docAnamnesisItem8", GetSymptom("_fieldAnamnesisItem8") },
            { "_docAnamnesisItem9", GetSymptom("_fieldAnamnesisItem9") },
            { "_docAnamnesisItem10", GetSymptom("_fieldAnamnesisItem10") },
            { "_docAnamnesisItem11", GetSymptom("_fieldAnamnesisItem11") },
            { "_docHistology", GetSymptom("_fieldHistology") },
            { "_docChemotherapyDate", GetSymptom("_fieldChemotherapyDate") },
            { "_docChemotherapy", GetSymptom("_fieldChemotherapy") },
            { "_docCreationDate", DateTime.Now.ToString("dd.MM.yyyy") },
            { "_docDoctor", GetSymptom("_fieldDoctor") }
        };

        GenerateDocument("Виписка ОГП ОЦО_template.docx", fieldValues,
            $"Виписка_ОГП_ОЦО_{_patient.LastName}_{_patient.FirstName}_{_patient.MiddleName}");
    }

    private void BtnCreateF2_Click(object sender, RoutedEventArgs e)
    {
        var fieldValues = new Dictionary<string, string>
        {
            { "_docCardNumber", _patient.CardNumber },
            { "_docHospitalStartDate", _visit.StartDate.ToString("dd.MM.yyyy") ?? "" },
            { "_docLastFirstMiddleName", $"{_patient.LastName} {_patient.FirstName} {_patient.MiddleName}" },
            { "_docBirthDay", _patient.BirthDate.ToString("dd.MM.yyyy") },
            { "_docAge", _patient.Age.ToString() },
            { "_docLivingAddress", _patient.Address },
            { "_docProfession", _patient.Profession },
            { "_docHospitalEndDate", _visit.EndDate.ToString("dd.MM.yyyy") ?? "" },
            { "_docDiagnosisMain", GetSymptom("_fieldFinalDiagnosis") },
            { "_docMKX", GetSymptom("_fieldMKX") },
            { "_docFirstOpertaionDate", GetSymptom("_fieldOperationDate") },
            { "_docFirstOpertaionName", GetSymptom("_fieldOperationName") },
            { "_docSecOpertaionDate", "" },
            { "_docSecOpertaionName", "" },
            { "_docDoctor", GetSymptom("_fieldDoctor") },
            { "_docCreationDate", DateTime.Now.ToString("dd.MM.yyyy") }
        };

        GenerateDocument("Виписка 066_template.docx", fieldValues,
            $"Виписка_066_{_patient.LastName}_{_patient.FirstName}_{_patient.MiddleName}");
    }

    private void GenerateDocument(string templateFileName, Dictionary<string, string> values, string outputName)
    {
        var engine = new Engine();
        try
        {
            var dialog = new SaveFileDialog
            {
                FileName = outputName,
                DefaultExt = ".docx",
                Filter = "Word Documents (.docx)|*.docx"
            };

            if (dialog.ShowDialog() == true)
            {
                engine.Merge($"..\\..\\..\\{templateFileName}", values, dialog.FileName);
                MessageBox.Show("Документ було створено!", "Документ", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                new Process
                {
                    StartInfo = new ProcessStartInfo(dialog.FileName) { UseShellExecute = true }
                }.Start();
            }
        }
        catch (Exception ex)
        {
            var msg = ex.Message.Contains("being used")
                ?
                "Документ вже створений та відкритий! Перевірте будь-ласка запущені процеси."
                : ex.Message.Contains("Could not find file")
                    ? "Шаблон документу не знайдено. Будь-ласка зверніться до підтримки!"
                    :
                    "Неочікувана помилка. Будь-ласка зверніться до підтримки!";

            MessageBox.Show(msg, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        Close();
    }

    private string FormatList(string raw)
    {
        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(raw);
            if (list != null)
                return string.Join(", ", list);
        }
        catch
        {
            // do nothing
        }

        return raw ?? "";
    }
}