using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MedProject_UI.Helpers;
using MedProject_UI.Models;
using MedProject_UI.Services;
using MedProject_UI.View.UserControls;
using Newtonsoft.Json;
using static MedProject_UI.App;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for AddPatient.xaml
/// </summary>
public partial class AddPatient : Window
{
    #region поля
    private readonly Patient _patient;              // активний пацієнт (або новий)
    private readonly Visit _visit;                  // візит, з яким працює форма
    private readonly bool _isEditMode;              // редагуємо пацієнта цілком
    private readonly bool _isNewVisitMode;          // додаємо лише новий візит існуючому пацієнту
    private readonly MongoDbService _mongoService;  // робота з БД
    private readonly Doctor _currentDoctor;
    #endregion

    #region конструктор
    /// <summary>
    /// Конструктор форми.
    /// 
    /// <para>patient == null  &rarr;  створення нового пацієнта</para>
    /// <para>patient != null &amp;&amp; isNewVisitMode == false  &rarr;  редагування пацієнта</para>
    /// <para>patient != null &amp;&amp; isNewVisitMode == true   &rarr;  додавання нового візиту</para>
    /// </summary>
    public AddPatient(Patient patient = null, bool isNewVisitMode = false, Visit visit = null)
    {
        InitializeComponent();


        // ініціалізуємо доступ до БД
        var config = AppConfig.Load();
        _mongoService = new MongoDbService(
            config.MongoDbConnection,
            config.DatabaseName);

        _currentDoctor = App.CurrentUser;
        _isNewVisitMode = isNewVisitMode;

        // --- режим створення нового пацієнта --------------------------------
        if (patient == null)
        {
            _isEditMode = false;
            _patient = new Patient();
            _visit = new Visit();
        }
        // --- режим додавання нового візиту ----------------------------------
        else if (_isNewVisitMode)
        {
            _isEditMode = false;
            _patient = patient;            // пацієнт вже є в базі
            _visit = new Visit();        // створюємо ПУСТИЙ новий візит
        }
        // --- режим редагування наявного пацієнта ----------------------------
        else
        {
            _isEditMode = true;
            _patient = patient;
            // беремо ОСТАННІЙ візит, або створимо новий, щоб форма відкрилася коректно
            _visit = visit ?? new Visit();
        }

        ConfigureButtons();
        WireUpUiEvents();
        PopulateFields();      // заповнюємо форму згідно з режимом
    }
    #endregion

    #region налаштування UI
    private void ConfigureButtons()
    {
        // «Зберегти» vs «Зберегти зміни»
        if (_isEditMode)
        {
            btnPage8Save.Visibility = Visibility.Hidden;
            btnPage8SaveChanges.Visibility = Visibility.Visible;
        }
        else if (_isNewVisitMode)
        {
            // для нового візиту зручніше підписати як «Зберегти візит» (UI‑текст можна змінити у XAML)
            btnPage8Save.MyBtnText = "Зберегти візит";
            btnPage8Save.Visibility = Visibility.Visible;
            btnPage8SaveChanges.Visibility = Visibility.Hidden;
            tbLastName.IsEnabled = false;
            tbFirstName.IsEnabled = false;
            tbMiddleName.IsEnabled = false;
            datePickerBirthday.IsEnabled = false;
            tbLivingAddress.IsEnabled = false;
            tbWorkAddress.IsEnabled = false;
        }
        else // новий пацієнт
        {
            btnPage8Save.Visibility = Visibility.Visible;
            btnPage8SaveChanges.Visibility = Visibility.Hidden;
        }
    }

    private void WireUpUiEvents()
    {
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


        datePickerBirthday.DateLoad += DateLoad_Birthday;
        datePickerHospitalStart.DateLoad += DateLoad_HospitalStart;
        datePickerHospitalEnd.DateLoad += DateLoad_HospitalEnd;
        dateOperation.DateLoad += DateLoad_Operation;
        dateChemotherapy.DateLoad += DateLoad_Chemotherapy;


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
        btnPage8Save.btnClick += btnPage8Save_Changes;
        btnPage8SaveChanges.btnClick += btnPage8Save_Changes;
    }
    #endregion

    #region заповнення полів
    private void PopulateFields()
    {
        // --- редагування пацієнта – залишаємо повне заповнення як було --------
        if (_isEditMode)
        {
            PopulateAllFieldsFromVisit();
        }
        // --- новий візит – заповнюємо тільки паспортну частину ----------------
        else if (_isNewVisitMode)
        {
            tbLastName.tbSearchText.Text = _patient.LastName;
            tbFirstName.tbSearchText.Text = _patient.FirstName;
            tbMiddleName.tbSearchText.Text = _patient.MiddleName;
            datePickerBirthday.customDatePicker.SelectedDate = _patient.BirthDate;
            tbLivingAddress.tbSearchText.Text = _patient.Address;
            tbWorkAddress.tbSearchText.Text = _patient.Profession;
        }
        // --- новий пацієнт – форма порожня -----------------------------------
    }

    private void PopulateAllFieldsFromVisit()
    {
        if (_isEditMode)
        {
            tbLastName.tbSearchText.Text = _patient.LastName;
            tbFirstName.tbSearchText.Text = _patient.FirstName;
            tbMiddleName.tbSearchText.Text = _patient.MiddleName;
            datePickerBirthday.customDatePicker.SelectedDate = _patient.BirthDate;
            tbLivingAddress.tbSearchText.Text = _patient.Address;
            tbWorkAddress.tbSearchText.Text = _patient.Profession;
            datePickerHospitalStart.customDatePicker.SelectedDate = _visit.StartDate.Year == 1 ? null : _visit.StartDate;
            datePickerHospitalEnd.customDatePicker.SelectedDate = _visit.EndDate.Year == 1 ? null : _visit.EndDate;
            tbClaims.Text = GetSymptom("_fieldClaims") ?? "";
            tbEntrDiagnosis.Text = GetSymptom("_fieldEntrDiagnosis") ?? "";
            tbFinalDiagnosis.Text = GetSymptom("_fieldFinalDiagnosis") ?? "";
            tbComplications.Text = GetSymptom("_fieldComplication") ?? "";
            tbAdditionDiagnosis.Text = GetSymptom("_fieldAdditionalDiagnosis") ?? "";
            tbMKX.tbSearchText.Text = GetSymptom("_fieldMKX") ?? "";
            tbOperationName.tbSearchText.Text = GetSymptom("_fieldOperationName") ?? "";
            var temp = ParseDate(GetSymptom("_fieldOperationDate"));

            dateOperation.customDatePicker.SelectedDate = ParseDate(GetSymptom("_fieldOperationDate")).GetValueOrDefault().Year == 1 
                                                            ? null 
                                                            : ParseDate(GetSymptom("_fieldOperationDate"));
            tbChemotherapyName.tbSearchText.Text = GetSymptom("_fieldChemotherapy") ?? "";
            dateChemotherapy.customDatePicker.SelectedDate = ParseDate(GetSymptom("_fieldChemotherapyDate")).GetValueOrDefault().Year == 1 
                                                            ? null 
                                                            : ParseDate(GetSymptom("_fieldChemotherapyDate"));
            tbHistology.tbSearchText.Text = GetSymptom("_fieldHistology") ?? "";

            tbDepartHeadAssistant.Text = GetSymptom("_fieldDepartHeadAssistant") ?? "";
            LogicalTreeHelper.GetChildren(containerOverallItem1)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem1")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem1") ?? "0")].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem2)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem2")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem2") ?? "0")].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem3)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem3")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem3") ?? "0")].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem4)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem4")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem4") ?? "0")].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem5)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem5")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem5") ?? "0")].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem6)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem6")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem6") ?? "0")].IsChecked = true;

            var liverHolder = GetSymptom("_fieldOverallItem7") ?? "";
            var liverExtend = !string.IsNullOrEmpty(liverHolder)
                ? GetSymptom("_fieldOverallItem7").Contains("Збільшена")
                    ? GetSymptom("_fieldOverallItem7").Split(" на ")[1].Trim().Split("см")[0].Trim()
                    : ""
                : "";
            tbOverallItem7.Text = liverExtend;
            if (liverExtend == "")
                LogicalTreeHelper.GetChildren(containerOverallItem7)
                    .OfType<RadioButton>()
                    .ToList()
                    .Where(x => x.GroupName == "rbOverallItem7").ToList()[0].IsChecked = true;
            else
                LogicalTreeHelper.GetChildren(containerOverallItem7)
                    .OfType<RadioButton>()
                    .ToList()
                    .Where(x => x.GroupName == "rbOverallItem7").ToList()[1].IsChecked = true;
            LogicalTreeHelper.GetChildren(containerOverallItem8)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem8")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem8") ?? "0")].IsChecked = true;
            if (!string.IsNullOrEmpty(GetSymptom("_fieldOverallItem9_1")))
            {
                if (Convert.ToBoolean(GetSymptom("_fieldOverallItem9_1")))
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbOverallItem9_1").ToList()[1].IsChecked = true;
                else
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbOverallItem9_1").ToList()[0].IsChecked = true;
            }

            if (!string.IsNullOrEmpty(GetSymptom("_fieldOverallItem9_2")))
            {
                if (Convert.ToBoolean(GetSymptom("_fieldOverallItem9_2")))
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbOverallItem9_2").ToList()[1].IsChecked = true;
                else
                    LogicalTreeHelper.GetChildren(containerOverallItem9)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbOverallItem9_2").ToList()[0].IsChecked = true;
            }

            if (!string.IsNullOrEmpty(GetSymptom("_fieldOverallItem10")))
            {
                var cbOverallItem10List = new List<CheckBox>
                {
                    cbOverallItem10_1,
                    cbOverallItem10_2,
                    cbOverallItem10_3,
                    cbOverallItem10_4,
                    cbOverallItem10_5,
                    cbOverallItem10_6
                };
                var OverallItem10 = GetSymptom("_fieldOverallItem10");

                foreach (var i in OverallItem10.Split(',').Select(n => Convert.ToInt32(n)).ToArray())
                    cbOverallItem10List[i].IsChecked = true;
            }

            tbOverallItem11.Text = GetSymptom("_fieldOverallItem11") ?? "";
            tbOverallItem12.Text = GetSymptom("_fieldOverallItem12") ?? "";
            LogicalTreeHelper.GetChildren(containerOverallItem13)
                .OfType<RadioButton>()
                .ToList()
                .Where(x => x.GroupName == "rbOverallItem13")
                .ToList()[int.Parse(GetSymptom("_fieldOverallItem13") ?? "0")].IsChecked = true;
            if (!string.IsNullOrEmpty(GetSymptom("_fieldOverallItem14")))
            {
                var cbOverallItem14List = new List<CheckBox>
                {
                    cbOverallItem14_1,
                    cbOverallItem14_2,
                    cbOverallItem14_3,
                    cbOverallItem14_4,
                    cbOverallItem14_5
                };

                var OverallItem14 = GetSymptom("_fieldOverallItem14");

                foreach (var i in OverallItem14.Split(',').Select(n => Convert.ToInt32(n)).ToArray())
                    cbOverallItem14List[i].IsChecked = true;
            }

            tbOverallItem15.Text = GetSymptom("_fieldOverallItem15") ?? "";
            tbAnamnesisItem1.Text = GetSymptom("_fieldAnamnesisItem1") ?? "";
            tbAnamnesisItem2.Text = GetSymptom("_fieldAnamnesisItem2") ?? "";
            tbAnamnesisItem3.Text = GetSymptom("_fieldAnamnesisItem3") ?? "";
            tbAnamnesisItem4.Text = GetSymptom("_fieldAnamnesisItem4") ?? "";
            tbAnamnesisItem5.Text = GetSymptom("_fieldAnamnesisItem5") ?? "";
            tbAnamnesisItem6.Text = GetSymptom("_fieldAnamnesisItem6") ?? "";
            tbAnamnesisItem7.Text = GetSymptom("_fieldAnamnesisItem7") ?? "";
            tbAnamnesisItem8.Text = GetSymptom("_fieldAnamnesisItem8") ?? "";
            tbAnamnesisItem9.Text = GetSymptom("_fieldAnamnesisItem9") ?? "";
            tbAnamnesisItem10.Text = GetSymptom("_fieldAnamnesisItem10") ?? "";
            tbAnamnesisItem11.Text = GetSymptom("_fieldAnamnesisItem11") ?? "";
            tbLifeAnamnesisItem1.Text = GetSymptom("_fieldLifeAnamnesisItem1") ?? "Не хворів";
            tbLifeAnamnesisItem2.Text = GetSymptom("_fieldLifeAnamnesisItem2") ?? "Не хворів";
            tbLifeAnamnesisItem3.Text = GetSymptom("_fieldLifeAnamnesisItem3") ?? "Не хворів";
            tbLifeAnamnesisItem4.Text = GetSymptom("_fieldLifeAnamnesisItem4") ?? "Не хворів";
            tbLifeAnamnesisItem5.Text = GetSymptom("_fieldLifeAnamnesisItem5") ?? "Не хворів";
            tbLifeAnamnesisItem6.Text = GetSymptom("_fieldLifeAnamnesisItem6") ?? "Не хворів";
            tbLifeAnamnesisItem7.Text = GetSymptom("_fieldLifeAnamnesisItem7") ?? "";
            tbLifeAnamnesisItem8.Text = GetSymptom("_fieldLifeAnamnesisItem8") ?? "";
            tbLifeAnamnesisItem9.Text = GetSymptom("_fieldLifeAnamnesisItem9") ?? "";
            tbLifeAnamnesisItem10.Text = GetSymptom("_fieldLifeAnamnesisItem10") ?? "";
            tbLocusMorbiItem1.Text = GetSymptom("_fieldLocusMorbiItem1") ?? "";
            tbLocusMorbiItem2.Text = GetSymptom("_fieldLocusMorbiItem2") ?? "";

            if (!string.IsNullOrEmpty(GetSymptom("_fieldLocusMorbiItem3")))
            {
                if (Convert.ToBoolean(GetSymptom("_fieldLocusMorbiItem3")))
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbLocusMorbiItem3").ToList()[1].IsChecked = true;
                else
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbLocusMorbiItem3").ToList()[0].IsChecked = true;
            }

            tbLocusMorbiItem4.Text = GetSymptom("_fieldLocusMorbiItem4") ?? "";

            if (!string.IsNullOrEmpty(GetSymptom("_fieldLocusMorbiItem5")))
            {
                if (Convert.ToBoolean(GetSymptom("_fieldLocusMorbiItem5")))
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbLocusMorbiItem5").ToList()[1].IsChecked = true;
                else
                    LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
                        .OfType<RadioButton>()
                        .ToList()
                        .Where(x => x.GroupName == "rbLocusMorbiItem5").ToList()[0].IsChecked = true;
            }

            tbLocusMorbiItem6.Text = GetSymptom("_fieldLocusMorbiItem6") ?? "";
            tbLocusMorbiItem7.Text = GetSymptom("_fieldLocusMorbiItem7") ?? "";
        }
    }
    #endregion

    private void DatePrewiewText_Birthday(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[ࢶ]$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void DatePrewiewText_HospitalStart(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[ࢶ]$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void DatePrewiewText_HospitalEnd(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[ࢶ]$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void DatePrewiewText_Operation(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[ࢶ]$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void DatePrewiewText_Chemotherapy(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[ࢶ]$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextLastName(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextFirstName(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextMiddleName(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextLivingAddress(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9/',. ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextWork(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ'\- ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextMKX(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[a-zA-ZА-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9\,\.\/ ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextHistology(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9/', ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextOperationName(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void PreviewTextChemotherapyName(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }


    private void DateSelectionChanged_Birthday(object sender, SelectionChangedEventArgs e)
    {
        if (datePickerBirthday.customDatePicker.SelectedDate.HasValue)
        {
            try
            {
                if (datePickerHospitalStart.customDatePicker.BlackoutDates.Count() > 0)
                    datePickerHospitalStart.customDatePicker.BlackoutDates.Clear();
                datePickerHospitalStart.customDatePicker.BlackoutDates.Add(new CalendarDateRange(
                    new DateTime(1900, 1, 1), datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата госпіталізації не може бути раніше за дати народження!",
                        "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    datePickerHospitalStart.customDatePicker.SelectedDate = null;
                    datePickerHospitalStart.customDatePicker.BlackoutDates.Add(
                        new CalendarDateRange(new DateTime(1900, 1, 1),
                            datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            try
            {
                if (datePickerHospitalEnd.customDatePicker.BlackoutDates.Count() > 0)
                    datePickerHospitalEnd.customDatePicker.BlackoutDates.Clear();
                datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                    datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата виписки не може бути раніше за дати народження!", "Заблокована дата!",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    datePickerHospitalEnd.customDatePicker.SelectedDate = null;
                    datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(
                        new CalendarDateRange(new DateTime(1900, 1, 1),
                            datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            try
            {
                if (dateOperation.customDatePicker.BlackoutDates.Count() > 0)
                    dateOperation.customDatePicker.BlackoutDates.Clear();
                dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                    datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата операції не може бути раніше за дати народження!", "Заблокована дата!",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    dateOperation.customDatePicker.SelectedDate = null;
                    dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                        datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            try
            {
                if (dateChemotherapy.customDatePicker.BlackoutDates.Count() > 0)
                    dateChemotherapy.customDatePicker.BlackoutDates.Clear();
                dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                    datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата ПХТ не може бути раніше за дати народження!", "Заблокована дата!",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    dateChemotherapy.customDatePicker.SelectedDate = null;
                    dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                        datePickerBirthday.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
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
                    datePickerHospitalEnd.customDatePicker.BlackoutDates.Clear();
                datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                    datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата виписки не може бути раніше за дату госпіталізації!",
                        "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    datePickerHospitalEnd.customDatePicker.SelectedDate = null;
                    datePickerHospitalEnd.customDatePicker.BlackoutDates.Add(
                        new CalendarDateRange(new DateTime(1900, 1, 1),
                            datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            try
            {
                if (dateOperation.customDatePicker.BlackoutDates.Count() > 0)
                    dateOperation.customDatePicker.BlackoutDates.Clear();
                dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                    datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата операції не може бути раніше за дату госпіталізації!",
                        "Заблокована дата!", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    dateOperation.customDatePicker.SelectedDate = null;
                    dateOperation.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                        datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            try
            {
                if (dateChemotherapy.customDatePicker.BlackoutDates.Count() > 0)
                    dateChemotherapy.customDatePicker.BlackoutDates.Clear();
                dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                    datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Specified argument was out of the range of valid values."))
                {
                    MessageBox.Show("Обрана дата ПХТ не може бути раніше за дату госпіталізації!", "Заблокована дата!",
                        MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    dateChemotherapy.customDatePicker.SelectedDate = null;
                    dateChemotherapy.customDatePicker.BlackoutDates.Add(new CalendarDateRange(new DateTime(1900, 1, 1),
                        datePickerHospitalStart.customDatePicker.SelectedDate.Value.Date.AddDays(-1)));
                }
                else
                {
                    MessageBox.Show("Сталась помилка! Звреніться до технічної підтримки!", "Помилка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    private void DateSelectionChanged_HospitalEnd(object sender, SelectionChangedEventArgs e)
    {
    }

    private void DateSelectionChanged_Operation(object sender, SelectionChangedEventArgs e)
    {
    }

    private void DateSelectionChanged_Chemotherapy(object sender, SelectionChangedEventArgs e)
    {
    }


    private void DateLoad_Birthday(object sender, RoutedEventArgs e)
    {
        datePickerBirthday.customDatePicker.BlackoutDates.Clear();
        datePickerBirthday.customDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.Date.AddDays(1),
            new DateTime(2100, 12, 31)));
    }

    private void DateLoad_HospitalStart(object sender, RoutedEventArgs e)
    {
    }

    private void DateLoad_HospitalEnd(object sender, RoutedEventArgs e)
    {
    }

    private void DateLoad_Operation(object sender, RoutedEventArgs e)
    {
    }

    private void DateLoad_Chemotherapy(object sender, RoutedEventArgs e)
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
        var listInputs = new List<UserControl>
        {
            tbLastName, tbFirstName, tbMiddleName, datePickerBirthday, tbLivingAddress, tbWorkAddress,
            datePickerHospitalStart
        };
        foreach (var input in listInputs)
            if (input is ClearableSearchBar)
                (input as ClearableSearchBar).borderCustom.BorderBrush =
                    new SolidColorBrush(Color.FromRgb(170, 170, 170));
            else
                (input as DatePickerTemplate).borderCustom.BorderBrush =
                    new SolidColorBrush(Color.FromRgb(170, 170, 170));

        listInputs.Where(x =>
                x is ClearableSearchBar && string.IsNullOrEmpty((x as ClearableSearchBar).tbSearchText.Text))
            .ToList()
            .ForEach(x => (x as ClearableSearchBar).borderCustom.BorderBrush = new SolidColorBrush(Colors.Red));
        listInputs
            .Where(x => x is DatePickerTemplate && (x as DatePickerTemplate).customDatePicker.SelectedDate == null)
            .ToList()
            .ForEach(x => (x as DatePickerTemplate).borderCustom.BorderBrush = new SolidColorBrush(Colors.Red));


        if (!listInputs.Where(x =>
                x is ClearableSearchBar && string.IsNullOrEmpty((x as ClearableSearchBar).tbSearchText.Text)).Any()
            || !listInputs.Where(x =>
                x is DatePickerTemplate && (x as DatePickerTemplate).customDatePicker.SelectedDate == null).Any())
        {
            mainGridPage1.Visibility = Visibility.Hidden;
            mainGridPage2.Visibility = Visibility.Visible;


            _patient.LastName = tbLastName.tbSearchText.Text;
            _patient.FirstName = tbFirstName.tbSearchText.Text;
            _patient.MiddleName = tbMiddleName.tbSearchText.Text;
            _patient.BirthDate = datePickerBirthday.customDatePicker.SelectedDate.GetValueOrDefault();
            _patient.Address = tbLivingAddress.tbSearchText.Text;
            _patient.Profession = tbWorkAddress.tbSearchText.Text;
            if (_isEditMode) 
            {
                _visit.StartDate = datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault();
                _visit.EndDate = datePickerHospitalEnd.customDatePicker.SelectedDate.GetValueOrDefault();
            }
            //_patient.HospitalDate = datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault();
            //_patient.LeaveDate = datePickerHospitalEnd.customDatePicker.SelectedDate.GetValueOrDefault();
        }
    }


    private async void btnPage2Next_Click(object sender, RoutedEventArgs e)
    {
        var checkMandatory = new List<TextBox>
        {
            tbClaims,
            tbEntrDiagnosis
        };
        checkMandatory.ForEach(x =>
            (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
        if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
        {
            checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
            return;
        }

        mainGridPage2.Visibility = Visibility.Hidden;
        mainGridPage3.Visibility = Visibility.Visible;

        if (_isEditMode)
        {

            var savedDoctorName = await _mongoService.GetDoctorByIdAsync(_patient.DoctorId);
            if (!string.IsNullOrWhiteSpace(savedDoctorName.ShortName))
            {
                foreach (ComboBoxItem item in comboBoxDoctorName.Items)
                {
                    if (item.Tag is Doctor doctor)
                    {
                        var formattedName = $"{doctor.FirstName[0]}.{doctor.MiddleName[0]}. {doctor.LastName}";
                        if (formattedName == savedDoctorName.ShortName)
                        {
                            comboBoxDoctorName.SelectedItem = item;
                            break;
                        }
                    }
                }
            }

            // Препопуляція cbDepartmentHead
            string savedHead = GetSymptom("_fieldDepartmentHead");
            if (!string.IsNullOrWhiteSpace(savedHead))
            {
                foreach (ComboBoxItem item in cbDepartmentHead.Items)
                {
                    if (item.Tag is Doctor chief)
                    {
                        var formatted = $"{chief.FirstName[0]}.{chief.MiddleName[0]}. {chief.LastName}";
                        if (formatted == savedHead)
                        {
                            cbDepartmentHead.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }

        SetSymptom("_fieldClaims", JsonConvert.SerializeObject(tbClaims.Text.Trim().TrimEnd(',').Split("\n")));
        SetSymptom("_fieldEntrDiagnosis", tbEntrDiagnosis.Text);
        SetSymptom("_fieldFinalDiagnosis", tbFinalDiagnosis.Text);
        SetSymptom("_fieldComplication", tbComplications.Text);
        SetSymptom("_fieldAdditionalDiagnosis", tbAdditionDiagnosis.Text);
        SetSymptom("_fieldMKX", tbMKX.tbSearchText.Text);
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


        SetSymptom("_fieldOperationName", tbOperationName.tbSearchText.Text);
        SetSymptom("_fieldOperationDate", dateOperation.customDatePicker.SelectedDate?.ToString());
        SetSymptom("_fieldChemotherapy", tbChemotherapyName.tbSearchText.Text);
        SetSymptom("_fieldChemotherapyDate", dateChemotherapy.customDatePicker.SelectedDate?.ToString());
        SetSymptom("_fieldHistology", tbHistology.tbSearchText.Text);
        if (comboBoxDoctorName.SelectedItem is ComboBoxItem selectedDoctorItem &&
            selectedDoctorItem.Tag is Doctor selectedDoctor)
        {
            var displayName =
                $"{selectedDoctor.FirstName[0]}.{selectedDoctor.MiddleName[0]}. {selectedDoctor.LastName}";
            SetSymptom("_fieldDoctor", displayName);
            _patient.Doctor = displayName;
            _patient.DoctorId = selectedDoctor.Id;
        }

        if (cbDepartmentHead.SelectedItem is ComboBoxItem selectedHeadItem &&
            selectedHeadItem.Tag is Doctor headDoctor)
        {
            var headName = $"{headDoctor.FirstName[0]}.{headDoctor.MiddleName[0]}. {headDoctor.LastName}";
            SetSymptom("_fieldDepartmentHead", headName);
        }

        SetSymptom("_fieldDepartHeadAssistant", tbDepartHeadAssistant.Text);
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
        SetSymptom("_fieldOverallItem1", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem1
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem2)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem2")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem2", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem2
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem3)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem3")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem3", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem3
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem4)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem4")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem4", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem4
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem5)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem5")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem5", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem5
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem6)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem6")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem6", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem6
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem7)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem7")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem7", contentHolder != null
            ? contentHolder.Content.ToString() == "Не збільшена"
                ? "Не збільшена"
                : contentHolder.Content + $" на {tbOverallItem7.Text} см."
            : "");


        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem8)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem8")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem8", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem8
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));
    }


    private void btnPage5Back_Click(object sender, RoutedEventArgs e)
    {
        mainGridPage5.Visibility = Visibility.Hidden;
        mainGridPage4.Visibility = Visibility.Visible;
    }

    private void btnPage5Next_Click(object sender, RoutedEventArgs e)
    {
        var cbOverallItem10List = new List<CheckBox>
        {
            cbOverallItem10_1,
            cbOverallItem10_2,
            cbOverallItem10_3,
            cbOverallItem10_4,
            cbOverallItem10_5,
            cbOverallItem10_6
        };
        var cbOverallItem14List = new List<CheckBox>
        {
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
            if (string.IsNullOrEmpty(tbOverallItem11.Text))
                borderOverallItem11.BorderBrush = new SolidColorBrush(Colors.Red);
            if (string.IsNullOrEmpty(tbOverallItem12.Text) || tbOverallItem12.Text == "___/___")
                borderOverallItem12.BorderBrush = new SolidColorBrush(Colors.Red);
            if (string.IsNullOrEmpty(tbOverallItem15.Text))
                borderOverallItem15.BorderBrush = new SolidColorBrush(Colors.Red);
            if (!cbOverallItem10List.Where(x => (bool)x.IsChecked).Any())
                borderOverallItem10.BorderBrush = new SolidColorBrush(Colors.Red);
            if (!cbOverallItem14List.Where(x => (bool)x.IsChecked).Any())
                borderOverallItem14.BorderBrush = new SolidColorBrush(Colors.Red);
            return;
        }

        mainGridPage5.Visibility = Visibility.Hidden;
        mainGridPage6.Visibility = Visibility.Visible;

        var contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem9)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem9_1")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem9_1", Convert.ToString(contentHolder != null
            ? contentHolder.Content.ToString() == "Негативний" ? false : true
            : null));
        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem9)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem9_2")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem9_2", Convert.ToString(contentHolder != null
            ? contentHolder.Content.ToString() == "Справа" ? false : true
            : null));

        var intHolder = new List<int>();
        for (var i = 0; cbOverallItem10List.Count > i; i++)
            if ((bool)cbOverallItem10List[i].IsChecked)
                intHolder.Add(i);
        SetSymptom("_fieldOverallItem10",
            JsonConvert.SerializeObject(string.Join(", ", intHolder.ToArray()).Trim().TrimEnd(',').Split(", ")));
        //JsonConvert.SerializeObject(string.Join(", ", intHolder.ToArray()).Trim().TrimEnd(',').Split(", "))

        var flag = 0;
        int.TryParse(tbOverallItem11.Text, out flag);
        SetSymptom("_fieldOverallItem11", flag.ToString());
        SetSymptom("_fieldOverallItem12", tbOverallItem12.Text.TrimEnd('_'));

        contentHolder = LogicalTreeHelper.GetChildren(containerOverallItem13)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbOverallItem13")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldOverallItem13", Convert.ToString(contentHolder != null
            ? ((App)Application.Current).dictOverallItem13
            .FirstOrDefault(x => x.Value == contentHolder.Content.ToString()).Key
            : 0));

        intHolder = new List<int>();
        for (var i = 0; cbOverallItem14List.Count > i; i++)
            if ((bool)cbOverallItem14List[i].IsChecked)
                intHolder.Add(i);
        SetSymptom("_fieldOverallItem14",
            JsonConvert.SerializeObject(string.Join(", ", intHolder.ToArray()).Trim().TrimEnd(',').Split(", ")));

        flag = 0;
        int.TryParse(tbOverallItem15.Text, out flag);
        SetSymptom("_fieldOverallItem15", flag.ToString());
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

        SetSymptom("_fieldAnamnesisItem1", tbAnamnesisItem1.Text);
        SetSymptom("_fieldAnamnesisItem2", tbAnamnesisItem2.Text);
        SetSymptom("_fieldAnamnesisItem3", tbAnamnesisItem3.Text);
        SetSymptom("_fieldAnamnesisItem4", tbAnamnesisItem4.Text);
        SetSymptom("_fieldAnamnesisItem5", tbAnamnesisItem5.Text);
        SetSymptom("_fieldAnamnesisItem6", tbAnamnesisItem6.Text);
        SetSymptom("_fieldAnamnesisItem7", tbAnamnesisItem7.Text);
        SetSymptom("_fieldAnamnesisItem8", tbAnamnesisItem8.Text);
        SetSymptom("_fieldAnamnesisItem9", tbAnamnesisItem9.Text);
        SetSymptom("_fieldAnamnesisItem10", tbAnamnesisItem10.Text);
        SetSymptom("_fieldAnamnesisItem11", tbAnamnesisItem11.Text);
    }

    private void btnPage7Back_Click(object sender, RoutedEventArgs e)
    {
        mainGridPage7.Visibility = Visibility.Hidden;
        mainGridPage6.Visibility = Visibility.Visible;
    }

    private void btnPage7Next_Click(object sender, RoutedEventArgs e)
    {
        var checkMandatory = new List<TextBox>
        {
            tbLifeAnamnesisItem1,
            tbLifeAnamnesisItem2,
            tbLifeAnamnesisItem3,
            tbLifeAnamnesisItem4,
            tbLifeAnamnesisItem5,
            tbLifeAnamnesisItem6
        };
        checkMandatory.ForEach(x =>
            (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
        if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
        {
            checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
            return;
        }

        mainGridPage7.Visibility = Visibility.Hidden;
        mainGridPage8.Visibility = Visibility.Visible;

        SetSymptom("_fieldLifeAnamnesisItem1", tbLifeAnamnesisItem1.Text);
        SetSymptom("_fieldLifeAnamnesisItem2", tbLifeAnamnesisItem2.Text);
        SetSymptom("_fieldLifeAnamnesisItem3", tbLifeAnamnesisItem3.Text);
        SetSymptom("_fieldLifeAnamnesisItem4", tbLifeAnamnesisItem4.Text);
        SetSymptom("_fieldLifeAnamnesisItem5", tbLifeAnamnesisItem5.Text);
        SetSymptom("_fieldLifeAnamnesisItem6", tbLifeAnamnesisItem6.Text);
        SetSymptom("_fieldLifeAnamnesisItem7", tbLifeAnamnesisItem7.Text);

        var flag = 0;
        int.TryParse(tbLifeAnamnesisItem8.Text, out flag);
        SetSymptom("_fieldLifeAnamnesisItem8", flag.ToString());

        flag = 0;
        int.TryParse(tbLifeAnamnesisItem9.Text, out flag);
        SetSymptom("_fieldLifeAnamnesisItem9", flag.ToString());

        SetSymptom("_fieldLifeAnamnesisItem10", tbLifeAnamnesisItem10.Text);
    }

    private void btnPage8Back_Click(object sender, RoutedEventArgs e)
    {
        mainGridPage8.Visibility = Visibility.Hidden;
        mainGridPage7.Visibility = Visibility.Visible;
    }


    private async void btnPage8Save_Changes(object sender, RoutedEventArgs e)
    {
        var checkMandatory = new List<TextBox>
        {
            tbLocusMorbiItem1,
            tbLocusMorbiItem2,
            tbLocusMorbiItem4,
            tbLocusMorbiItem6,
            tbLocusMorbiItem7
        };

        checkMandatory.ForEach(x =>
            (x.Parent as Border).BorderBrush = new SolidColorBrush(Color.FromRgb(170, 170, 170)));
        if (checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).Any())
        {
            checkMandatory.Where(x => string.IsNullOrEmpty(x.Text)).ToList()
                .ForEach(x => (x.Parent as Border).BorderBrush = new SolidColorBrush(Colors.Red));
            return;
        }

        SetSymptom("_fieldLocusMorbiItem1",
            JsonConvert.SerializeObject(tbLocusMorbiItem1.Text.Trim().TrimEnd(',').Split(", ")));
        SetSymptom("_fieldLocusMorbiItem2",
            JsonConvert.SerializeObject(tbLocusMorbiItem2.Text.Trim().TrimEnd(',').Split(", ")));

        var contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem3)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbLocusMorbiItem3")
            .FirstOrDefault(x => (bool)x.IsChecked);

        SetSymptom("_fieldLocusMorbiItem3", Convert.ToString(contentHolder != null
            ? contentHolder.Content.ToString() == "Везикулярне" ? false : true
            : null));

        SetSymptom("_fieldLocusMorbiItem4",
            JsonConvert.SerializeObject(tbLocusMorbiItem4.Text.Trim().TrimEnd(',').Split(", ")));

        contentHolder = LogicalTreeHelper.GetChildren(containerLocusMorbiItem5)
            .OfType<RadioButton>()
            .ToList()
            .Where(x => x.GroupName == "rbLocusMorbiItem5")
            .FirstOrDefault(x => (bool)x.IsChecked);
        SetSymptom("_fieldLocusMorbiItem5", Convert.ToString(contentHolder != null
            ? contentHolder.Content.ToString() == "Легеневий звук" ? false : true
            : null));
        SetSymptom("_fieldLocusMorbiItem6",
            JsonConvert.SerializeObject(tbLocusMorbiItem6.Text.Trim().TrimEnd(',').Split(", ")));
        SetSymptom("_fieldLocusMorbiItem7",
            JsonConvert.SerializeObject(tbLocusMorbiItem7.Text.Trim().TrimEnd(',').Split(", ")));

        // 3. Формулюємо підтвердження
        var dlgText = _isEditMode
            ? $"Додати зміни до даних пацієнта {_patient.LastName} {_patient.FirstName}?"
            : _isNewVisitMode
                ? $"Додати новий візит пацієнту {_patient.LastName} {_patient.FirstName}?"
                : $"Додати пацієнта {_patient.LastName} {_patient.FirstName} до бази?";

        if (MessageBox.Show(dlgText, "Перевірка", MessageBoxButton.OKCancel, MessageBoxImage.Question) != MessageBoxResult.OK)
            return;

        try
        {
            if (_isNewVisitMode)
            {
                //_patient.HospitalDate = datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault();
                //_patient.LeaveDate = datePickerHospitalEnd.customDatePicker.SelectedDate.GetValueOrDefault();

                // додаємо новий візит і оновлюємо пацієнта в БД
                _patient.Visits ??= new List<Visit>();
                _visit.StartDate = datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault().Year == 1
                                        ? DateTime.Today
                                        : datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault();
                _visit.EndDate = datePickerHospitalEnd.customDatePicker.SelectedDate.GetValueOrDefault();
                _visit.Notes = $"Запис від {DateTime.Today.ToString("D")}";
                _patient.Visits.Add(_visit);
                await _mongoService.InsertOrUpdatePatientAsync(_patient);
            }
            else if (_isEditMode)
            {
                // зміни всередині поточного візиту вже враховано у _visit, а він вже є у списку.
                await _mongoService.InsertOrUpdatePatientAsync(_patient);
            }
            else // новий пацієнт
            {
                _patient.CardNumber = GuidHelper.GenerateShortGuid();
                var unfixedAge = DateTime.Today.Year - _patient.BirthDate.Year;
                _patient.Age = _patient.BirthDate > DateTime.Today.AddYears(-unfixedAge) ? --unfixedAge : unfixedAge;
                _visit.StartDate = datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault().Year == 1
                    ? DateTime.Today
                    : datePickerHospitalStart.customDatePicker.SelectedDate.GetValueOrDefault();
                _visit.EndDate = datePickerHospitalEnd.customDatePicker.SelectedDate.GetValueOrDefault();
                _visit.Notes = $"Запис від {DateTime.Today.ToString("D")}";
                _patient.Visits = new List<Visit> { _visit };
                await _mongoService.InsertOrUpdatePatientAsync(_patient);
            }

            MessageBox.Show("Дані збережено успішно!", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Сталася помилка при збереженні! Спробуйте пізніше.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.WriteLine(ex);
        }
    }


    private void comboBoxClaims_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var newClaims = sender as ComboBox;
        tbClaims.Text += newClaims.SelectedValue.ToString().Split(": ")[1] + "\n";
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
        var locusMorbiItem1 = sender as ComboBox;
        tbLocusMorbiItem1.Text += locusMorbiItem1.SelectedValue.ToString().Split(": ")[1] + ", ";
    }

    private void cbLocusMorbiItem2_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var locusMorbiItem2 = sender as ComboBox;
        tbLocusMorbiItem2.Text += locusMorbiItem2.SelectedValue.ToString().Split(": ")[1] + ", ";
    }

    private void cbLocusMorbiItem4_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var locusMorbiItem4 = sender as ComboBox;
        tbLocusMorbiItem4.Text += locusMorbiItem4.SelectedValue.ToString().Split(": ")[1] + ", ";
    }

    private void cbLocusMorbiItem6_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var locusMorbiItem6 = sender as ComboBox;
        tbLocusMorbiItem6.Text += locusMorbiItem6.SelectedValue.ToString().Split(": ")[1] + ", ";
    }

    private void cbLocusMorbiItem7_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var locusMorbiItem7 = sender as ComboBox;
        tbLocusMorbiItem7.Text += locusMorbiItem7.SelectedValue.ToString().Split(": ")[1] + ", ";
    }

    private void tbClaims_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbEntrDiagnosis_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbFinalDiagnosis_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbComplications_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAdditionDiagnosis_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,'+\- ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbDepartmentHead_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ'. ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbDepartHeadAssistant_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ'. ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbOverallItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[0-9\,\.]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbOverallItem11_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[0-9]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbOverallItem12_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[0-9\/]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbOverallItem15_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[0-9]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem1_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem2_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem3_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem4_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem5_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem6_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem8_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem9_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem10_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbAnamnesisItem11_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem1_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem2_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem3_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem4_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem5_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem6_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem8_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[0-9/ ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem9_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[0-9/ ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLifeAnamnesisItem10_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLocusMorbiItem1_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLocusMorbiItem2_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLocusMorbiItem4_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLocusMorbiItem6_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void tbLocusMorbiItem7_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9,' ]+$");
        if (!regex.IsMatch(e.Text))
            e.Handled = true;
        base.OnPreviewTextInput(e);
    }

    private void SetSymptom(string key, string value)
    {
        if (_visit.Symptoms == null)
            _visit.Symptoms = new Dictionary<string, string>();
        _visit.Symptoms[key] = value;
    }

    private string GetSymptom(string key)
    {
        var visit = _visit;
        if (visit.Symptoms == null || !visit.Symptoms.TryGetValue(key, out var value))
            return null;

        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(value);
            if (list != null)
                return string.Join(", ", list.Prepend("")).TrimStart(',').Trim();
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

    private DateTime? ParseDate(string text) => DateTime.TryParse(text, out var result) ? result : null;

    private async void LoadDoctorsAndChiefsAsync()
    {
        var allDoctors = await _mongoService.GetAllDoctorsAsync();

        // Встановити список лікарів у comboBoxDoctorName
        comboBoxDoctorName.Items.Clear();
        foreach (var doctor in allDoctors.Where(d => d.AccessLevel != "visitor" && d.AccessLevel != "admin"))
        {
            var formatted = $"{doctor.FirstName[0]}.{doctor.MiddleName[0]}. {doctor.LastName}";
            var item = new ComboBoxItem { Content = formatted, Tag = doctor };
            comboBoxDoctorName.Items.Add(item);

            // Якщо поточний користувач = doctor => заборонити редагування
            if (_currentDoctor != null && doctor.Email == _currentDoctor.Email && _currentDoctor.AccessLevel == "doctor")
            {
                comboBoxDoctorName.SelectedItem = item;
                comboBoxDoctorName.IsEnabled = false;
            }
        }

        if (_currentDoctor != null && (_currentDoctor.AccessLevel == "chief_doctor" || _currentDoctor.AccessLevel == "admin"))
        {
            if (!_isEditMode) comboBoxDoctorName.SelectedIndex = 0;
            comboBoxDoctorName.IsEnabled = true;
        }

        // Замість tbDepartmentHead використовуємо comboBox
        cbDepartmentHead.Items.Clear();
        foreach (var chief in allDoctors.Where(d => d.AccessLevel == "chief_doctor"))
        {
            var formatted = $"{chief.FirstName[0]}.{chief.MiddleName[0]}. {chief.LastName}";
            cbDepartmentHead.Items.Add(new ComboBoxItem
            {
                Content = formatted,
                Tag = chief
            });
        }

        cbDepartmentHead.SelectedIndex = 0;

        // Встановлюємо попередній вибір, якщо він був (можна за Email чи ID)
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadDoctorsAndChiefsAsync();
    }
}