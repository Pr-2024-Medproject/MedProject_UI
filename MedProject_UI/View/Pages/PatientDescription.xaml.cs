using System.Windows;
using MedProject_UI.Helpers;
using MedProject_UI.Models;
using MedProject_UI.Services;
using Newtonsoft.Json;

namespace MedProject_UI;

public partial class PatientDescription : Window
{
    private readonly Patient _patient;
    private readonly OverallDictionaries _overallDictionaries = new();
    private readonly MongoDbService _mongoDbService;

    public PatientDescription(Patient patient)
    {
        InitializeComponent();

        var config = AppConfig.Load();
        _mongoDbService = new MongoDbService(
            config.MongoDbConnection,
            config.DatabaseName);

        _patient = patient;
        LoadData(_patient);


        btnGenerateDoc.btnClick += btnGenerateDocument;
        btnChangeDesc.btnClick += btnChangeDescription;
        btnAddVisit.btnClick += btnAddNewVisit;
        btnChangeVisit.btnClick += btnChangeVisitTo;
    }

    private Visit GetLatestVisit()
    {
        return _patient.Visits?.LastOrDefault() ?? new Visit();
    }

    private void LoadData(Patient patient, Visit? selectedVisit = null)
    {
        var visit = selectedVisit ?? GetLatestVisit(); // використати обраний або останній

        string GetSymptom(string key, string fallback = "--------")
        {
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

        lbCardNumber.Content = $"Картка #{patient.CardNumber}";
        lbTypDescriptionItem1.Content = "Прізвище: " + patient.LastName;
        lbTypDescriptionItem2.Content = "Ім'я: " + patient.FirstName;
        lbTypDescriptionItem3.Content = "По-батькові: " + patient.MiddleName;
        lbTypDescriptionItem4.Content = "Дата народження: " + patient.BirthDate.ToString("dd.MM.yyyy");
        lbTypDescriptionItem5.Content = "Вік: " + patient.Age;

        lbCurrentVisit.Content = $"Візит від {visit.Date.ToString("d")}";

        tbLastName.Text = patient.LastName;
        tbFirstName.Text = patient.FirstName;
        tbMiddleName.Text = patient.MiddleName;
        tbBirthday.Text = patient.BirthDate.ToString("dd.MM.yyyy");
        tbLivingAddress.Text = patient.Address;
        tbWork.Text = patient.Profession;
        tbHospitalStart.Text = patient.HospitalDate?.ToString("dd.MM.yyyy") ?? "--.--.----";
        tbHospitalEnd.Text = patient.LeaveDate.GetValueOrDefault().Year == 1 ? "--.--.----" : patient.LeaveDate?.ToString("dd.MM.yyyy");

        try
        {
            var claimsRaw = GetSymptom("_fieldClaims");
            var claimsList = JsonConvert.DeserializeObject<string[]>(claimsRaw ?? "[]");
            tbClims.Text = claimsList != null ? string.Join(", ", claimsList) : "--------";
        }
        catch
        {
            tbClims.Text = GetSymptom("_fieldClaims") ??
                           "Скарги не будуть виведені коректно, оскільки збережені в неправильному форматі";
        }

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

        // Extended TabItems fields
        //tbOverallItem1.Text = _overallDictionaries.Dictionaries["dictOverallItem1"][GetSymptom("_fieldOverallItem1")];
        tbOverallItem1.Text = GetDictValue("dictOverallItem1", GetSymptom("_fieldOverallItem1"));
        tbOverallItem2.Text = GetDictValue("dictOverallItem2", GetSymptom("_fieldOverallItem2"));
        tbOverallItem3.Text = GetDictValue("dictOverallItem3", GetSymptom("_fieldOverallItem3"));
        tbOverallItem4.Text = GetDictValue("dictOverallItem4", GetSymptom("_fieldOverallItem4"));
        tbOverallItem5.Text = GetDictValue("dictOverallItem5", GetSymptom("_fieldOverallItem5"));
        tbOverallItem6.Text = GetDictValue("dictOverallItem6", GetSymptom("_fieldOverallItem6"));
        tbOverallItem7.Text = GetSymptom("_fieldOverallItem7") ?? "--------";
        tbOverallItem8.Text = GetDictValue("dictOverallItem8", GetSymptom("_fieldOverallItem8"));

        tbOverallItem9_1.Text = GetSymptom("_fieldOverallItem9_1") == "true" ? "Позитивний" : "Негативний";
        tbOverallItem9_2.Text = GetSymptom("_fieldOverallItem9_1") == "true"
            ? GetSymptom("_fieldOverallItem9_2") == "true" ? "Зліва" : "Зправа"
            : "---------";

        tbOverallItem10.Text = GetMultiDictValue("dictOverallItem10", GetSymptom("_fieldOverallItem10"));
        tbOverallItem11.Text = GetSymptom("_fieldOverallItem11") ?? "--------";
        tbOverallItem12.Text = GetSymptom("_fieldOverallItem12") ?? "--------";
        tbOverallItem13.Text = GetDictValue("dictOverallItem13", GetSymptom("_fieldOverallItem13"));
        tbOverallItem14.Text = GetMultiDictValue("dictOverallItem14", GetSymptom("_fieldOverallItem14"));
        tbOverallItem15.Text = GetSymptom("_fieldOverallItem15") ?? "--------";

        tbAnamnesisItem1.Text = GetSymptom("_fieldAnamnesisItem1") ?? "--------";
        tbAnamnesisItem2.Text = GetSymptom("_fieldAnamnesisItem2") ?? "--------";
        tbAnamnesisItem3.Text = GetSymptom("_fieldAnamnesisItem3") ?? "--------";
        tbAnamnesisItem4.Text = GetSymptom("_fieldAnamnesisItem4") ?? "--------";
        tbAnamnesisItem5.Text = GetSymptom("_fieldAnamnesisItem5") ?? "--------";
        tbAnamnesisItem6.Text = GetSymptom("_fieldAnamnesisItem6") ?? "--------";
        tbAnamnesisItem7.Text = GetSymptom("_fieldAnamnesisItem7") ?? "--------";
        tbAnamnesisItem8.Text = GetSymptom("_fieldAnamnesisItem8") ?? "--------";
        tbAnamnesisItem9.Text = GetSymptom("_fieldAnamnesisItem9") ?? "--------";
        tbAnamnesisItem10.Text = GetSymptom("_fieldAnamnesisItem10") ?? "--------";
        tbAnamnesisItem11.Text = GetSymptom("_fieldAnamnesisItem11") ?? "--------";

        tbLifeAnamnesisItem1.Text = GetSymptom("_fieldLifeAnamnesisItem1") ?? "--------";
        tbLifeAnamnesisItem2.Text = GetSymptom("_fieldLifeAnamnesisItem2") ?? "--------";
        tbLifeAnamnesisItem3.Text = GetSymptom("_fieldLifeAnamnesisItem3") ?? "--------";
        tbLifeAnamnesisItem4.Text = GetSymptom("_fieldLifeAnamnesisItem4") ?? "--------";
        tbLifeAnamnesisItem5.Text = GetSymptom("_fieldLifeAnamnesisItem5") ?? "--------";
        tbLifeAnamnesisItem6.Text = GetSymptom("_fieldLifeAnamnesisItem6") ?? "--------";
        tbLifeAnamnesisItem7.Text = GetSymptom("_fieldLifeAnamnesisItem7") ?? "--------";
        tbLifeAnamnesisItem8.Text = GetSymptom("_fieldLifeAnamnesisItem8") ?? "--------";
        tbLifeAnamnesisItem9.Text = GetSymptom("_fieldLifeAnamnesisItem9") ?? "--------";
        tbLifeAnamnesisItem10.Text = GetSymptom("_fieldLifeAnamnesisItem10") ?? "--------";

        tbLocusMorbiItem1.Text = GetSymptom("_fieldLocusMorbiItem1") ?? "--------";
        tbLocusMorbiItem2.Text = GetSymptom("_fieldLocusMorbiItem2") ?? "--------";
        tbLocusMorbiItem3.Text = GetSymptom("_fieldLocusMorbiItem3") == "true" ? "Везикулярне" : "Жорстке";
        tbLocusMorbiItem4.Text = GetSymptom("_fieldLocusMorbiItem4") ?? "--------";
        tbLocusMorbiItem5.Text = GetSymptom("_fieldLocusMorbiItem5") == "true" ? "Легеневий звук" : "Коробчатий звук";
        tbLocusMorbiItem6.Text = GetSymptom("_fieldLocusMorbiItem6") ?? "--------";
        tbLocusMorbiItem7.Text = GetSymptom("_fieldLocusMorbiItem7") ?? "--------";
    }

    private void btnGenerateDocument(object sender, RoutedEventArgs e)
    {
        var doc = new DocBuilder(_patient);
        doc.Show();
    }

    private async void btnChangeDescription(object sender, RoutedEventArgs e)
    {
        var infoEditor = new AddPatient(_patient);
        infoEditor.ShowDialog();

        var updatedPatient = await _mongoDbService.GetPatientByIdAsync(_patient.Id);

        if (updatedPatient != null)
            LoadData(updatedPatient);
        else
            MessageBox.Show("Не вдалося оновити дані пацієнта.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void btnAddNewVisit(object sender, RoutedEventArgs e)
    {
        var infoEditor = new AddPatient(_patient, true);
        infoEditor.ShowDialog();
    }

    private void btnChangeVisitTo(object sender, RoutedEventArgs e)
    {
        if (_patient.Visits == null || _patient.Visits.Count == 0)
        {
            MessageBox.Show("Немає доступних візитів для вибору.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var visitSelector = new VisitSelector(_patient.Visits, $"{_patient.LastName} {_patient.FirstName} {_patient.MiddleName}");
        if (visitSelector.ShowDialog() == true)
        {
            var selectedVisit = visitSelector.SelectedVisit;
            if (selectedVisit != null)
            {
                LoadData(_patient, selectedVisit); // оновлений метод з параметром
            }
        }
    }

    private void typicalDescription_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void Window_Activated(object sender, EventArgs e)
    {
    }

    private string GetDictValue(string dictName, string indexStr)
    {
        if (int.TryParse(indexStr, out var index))
        {
            var dict = _overallDictionaries.Dictionaries[dictName][index];
            if (dict != null && index >= 0)
                return dict;
        }

        return "--------";
    }

    private string GetMultiDictValue(string dictName, string indicesStr)
    {
        try
        {
            var result = "";

            indicesStr.Where(char.IsDigit).ToList()
                .Select(c => char.GetNumericValue(c)).ToList()
                .ForEach(c => { result += $"{_overallDictionaries.Dictionaries[dictName][Convert.ToInt32(c)]}, "; });

            return string.Join("\n• ", result.Trim()
                    .TrimEnd(',')
                    .Split(", ")
                    .Prepend("")
                )
                .TrimStart('\n').Trim();
        }
        catch
        {
        }

        return "--------";
    }
}