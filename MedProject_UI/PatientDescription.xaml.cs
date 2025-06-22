using System.Windows;
using MedProject_UI.Models;
using Newtonsoft.Json;

namespace MedProject_UI;

public partial class PatientDescription : Window
{
    private readonly Patient _patient;

    public PatientDescription(Patient patient)
    {
        InitializeComponent();
        _patient = patient;
        LoadData(_patient);

        tbLastName.TextChanged += (_, _) => _patient.LastName = tbLastName.Text;
        tbFirstName.TextChanged += (_, _) => _patient.FirstName = tbFirstName.Text;
        tbMiddleName.TextChanged += (_, _) => _patient.MiddleName = tbMiddleName.Text;
        tbLivingAddress.TextChanged += (_, _) => _patient.Address = tbLivingAddress.Text;
        tbWork.TextChanged += (_, _) => _patient.Profession = tbWork.Text;

        tbMKX.TextChanged += (_, _) => SetSymptom("_fieldMKX", tbMKX.Text);
        tbOperationName.TextChanged += (_, _) => SetSymptom("_fieldOperationName", tbOperationName.Text);
        tbChemotherapyName.TextChanged += (_, _) => SetSymptom("_fieldChemotherapy", tbChemotherapyName.Text);
        tbHistology.TextChanged += (_, _) => SetSymptom("_fieldHistology", tbHistology.Text);
        tbDoctorName.TextChanged += (_, _) => SetSymptom("_fieldDoctor", tbDoctorName.Text);
        tbDepartmentHead.TextChanged += (_, _) => SetSymptom("_fieldDepartmentHead", tbDepartmentHead.Text);
        tbDepartHeadAssistant.TextChanged +=
            (_, _) => SetSymptom("_fieldDepartHeadAssistant", tbDepartHeadAssistant.Text);

        btnGenerateDoc.btnClick += btnGenerateDocument;
        btnChangeDesc.btnClick += btnChangeDescription;
    }

    private Visit GetLatestVisit()
    {
        return _patient.Visits?.LastOrDefault() ?? new Visit();
    }

    private string GetSymptom(string key, string fallback = "--------")
    {
        var visit = GetLatestVisit();
        if (visit.Symptoms == null || !visit.Symptoms.TryGetValue(key, out var value))
            return fallback;

        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(value);
            if (list != null)
                return string.Join("\n• ", list.Prepend("")).TrimStart('\n').Trim();
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


    private void SetSymptom(string key, string value)
    {
        var visit = GetLatestVisit();
        if (visit.Symptoms == null)
            visit.Symptoms = new Dictionary<string, string>();

        visit.Symptoms[key] = value;
    }

    private void LoadData(Patient patient)
    {
        var visit = GetLatestVisit();

        lbTypDescriptionItem1.Content = "Прізвище: " + patient.LastName;
        lbTypDescriptionItem2.Content = "Ім'я: " + patient.FirstName;
        lbTypDescriptionItem3.Content = "По-батькові: " + patient.MiddleName;
        lbTypDescriptionItem4.Content = "Дата народження: " + patient.BirthDate.ToString("dd.MM.yyyy");
        lbTypDescriptionItem5.Content = "Вік: " + patient.Age;

        tbLastName.Text = patient.LastName;
        tbFirstName.Text = patient.FirstName;
        tbMiddleName.Text = patient.MiddleName;
        tbBirthday.Text = patient.BirthDate.ToString("dd.MM.yyyy");
        tbLivingAddress.Text = patient.Address;
        tbWork.Text = patient.Profession;
        tbHospitalStart.Text = patient.HospitalDate?.ToString("dd.MM.yyyy") ?? "--.--.----";
        tbHospitalEnd.Text = patient.LeaveDate?.ToString("dd.MM.yyyy") ?? "--.--.----";

        tbClims.Text = GetSymptom("_fieldClaims");
        tbEntrDiagnosis.Text = GetSymptom("_fieldEntrDiagnosis");
        tbFinalDiagnosis.Text = GetSymptom("_fieldFinalDiagnosis");
        tbComplications.Text = GetSymptom("_fieldComplication");
        tbAdditionDiagnosis.Text = GetSymptom("_fieldAdditionalDiagnosis");
        tbMKX.Text = GetSymptom("_fieldMKX");
        tbOperationName.Text = GetSymptom("_fieldOperationName");
        dateOperation.Text = GetSymptom("_fieldOperationDate", "--.--.----");
        tbChemotherapyName.Text = GetSymptom("_fieldChemotherapy");
        dateChemotherapy.Text = GetSymptom("_fieldChemotherapyDate", "--.--.----");
        tbHistology.Text = GetSymptom("_fieldHistology");
        tbDoctorName.Text = patient.Doctor;
        tbDepartmentHead.Text = GetSymptom("_fieldDepartmentHead");
        tbDepartHeadAssistant.Text = GetSymptom("_fieldDepartHeadAssistant");
    }

    private void btnGenerateDocument(object sender, RoutedEventArgs e)
    {
        var doc = new DocBuilder(_patient);
        doc.Show();
    }

    private void btnChangeDescription(object sender, RoutedEventArgs e)
    {
        var infoEditor = new AddPatient(_patient);
        infoEditor.ShowDialog();
    }

    private void typicalDescription_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void Window_Activated(object sender, EventArgs e)
    {
    }
}