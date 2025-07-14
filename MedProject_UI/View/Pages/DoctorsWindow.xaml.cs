using System.Windows;
using System.Windows.Controls;
using MedProject_UI.Models;
using MedProject_UI.Services;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for DoctorsWindow.xaml
/// </summary>
public partial class DoctorsWindow : Window
{
    private List<Doctor> allDoctors = new();

    public DoctorsWindow()
    {
        InitializeComponent();
    }

    private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        btnClearSearch.Visibility = string.IsNullOrEmpty(tbSearch.Text)
            ? Visibility.Collapsed
            : Visibility.Visible;

        tbSearchPlaceholder.Visibility = string.IsNullOrWhiteSpace(tbSearch.Text) && !tbSearch.IsFocused
            ? Visibility.Visible
            : Visibility.Collapsed;

        ApplyFilters();
    }


    private void tbSearch_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(tbSearch.Text))
            tbSearchPlaceholder.Visibility = Visibility.Visible;
    }

    private void btnClearSearch_Click(object sender, RoutedEventArgs e)
    {
        tbSearch.Clear();
        tbSearch.Focus();
    }

    private async void LoadDoctors()
    {
        using var mongoService = new MongoDbService(AppConfig.Load().MongoDbConnection, AppConfig.Load().DatabaseName);
        allDoctors = await mongoService.GetAllDoctorsAsync();

        foreach (var doctor in allDoctors) doctor.OnDutyStatus = GetOnDutyStatus(doctor);

        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filtered = allDoctors;

        // Фільтр по тексту
        if (!string.IsNullOrWhiteSpace(tbSearch.Text))
        {
            var search = tbSearch.Text.Trim().ToLower();
            filtered = filtered.Where(d =>
                $"{d.LastName} {d.FirstName} {d.MiddleName}".ToLower().Contains(search) ||
                $"{d.FirstName} {d.MiddleName} {d.LastName}".ToLower().Contains(search) ||
                $"{d.LastName} {d.FirstName}".ToLower().Contains(search)
            ).ToList();
        }

        // Фільтр по "на зміні"
        if (cbOnDutyOnly.IsChecked == true)
            filtered = filtered.Where(d => IsDoctorOnDuty(d)).ToList(); 

        DoctorsGrid.ItemsSource = filtered;
    }

    private bool IsDoctorOnDuty(Doctor doctor)
    {
        var today = DateTime.Today;
        return doctor.WorkSchedule.Any(p =>
            p.Status == WorkStatus.Work &&
            p.From.Date <= today &&
            p.To.Date >= today);
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Doctor selectedDoctor)
        {
            var editWindow = new RegisterDoctorWindow(selectedDoctor);
            editWindow.ShowDialog();

            // Оновити список після редагування
            LoadDoctors();
        }
    }

    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Doctor selectedDoctor)
        {
            DeleteDoctorWithReassignment(selectedDoctor);
        }
    }

    private void tbSearch_GotFocus(object sender, RoutedEventArgs e)
    {
        tbSearchPlaceholder.Visibility = Visibility.Collapsed;
    }

    private void btnBack_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CbOnDutyOnly_Checked(object sender, RoutedEventArgs e)
    {
        ApplyFilters();
    }

    private void cbOnDutyOnly_Unchecked(object sender, RoutedEventArgs e)
    {
        ApplyFilters();
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Показати кнопки тільки для головного лікаря або адміністратора
        if (App.CurrentUser != null &&
            (App.CurrentUser.AccessLevel.ToLower() == "admin" ||
             App.CurrentUser.AccessLevel.ToLower() == "chief_doctor"))
        {
            colEdit.Visibility = Visibility.Visible;
            colDelete.Visibility = Visibility.Visible;
        }
        else
        {
            colEdit.Visibility = Visibility.Collapsed;
            colDelete.Visibility = Visibility.Collapsed;
        }

        LoadDoctors();
    }

    private string GetOnDutyStatus(Doctor doctor)
    {
        var today = DateTime.Today;

        // Якщо на зміні сьогодні
        var isOnShiftToday = doctor.WorkSchedule.Any(p =>
            p.Status == WorkStatus.Work &&
            p.From.Date <= today &&
            p.To.Date >= today);

        if (isOnShiftToday)
            return "На зміні";

        // Знайти найближчу майбутню зміну
        var future = doctor.WorkSchedule
            .Where(p => p.Status == WorkStatus.Work && p.From.Date > today)
            .OrderBy(p => p.From)
            .FirstOrDefault();

        return future != null ? future.From.ToString("dd.MM.yyyy") : "—";
    }

    private async void DeleteDoctorWithReassignment(Doctor _doctor) {

        using var mongoService = new MongoDbService(AppConfig.Load().MongoDbConnection, AppConfig.Load().DatabaseName);
        var patients = await mongoService.GetPatientsByDoctorIdAsync(_doctor.Id);

        if (_doctor.AccessLevel.ToLower() == "viewer" || patients.Count == 0)
        {
            var confirm = MessageBox.Show(
                $"Ви впевнені, що хочете видалити лікаря {_doctor.LastName}?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm == MessageBoxResult.Yes)
            {
                await mongoService.DeleteDoctorAsync(_doctor.Id);
            }

            LoadDoctors();
            return;
        }

        // Якщо є пацієнти, відкриваємо вікно переназначення
        var allDoctors = await mongoService.GetAllDoctorsAsync();

        var reassignableDoctors = allDoctors
            .Where(d =>
                d.Id != _doctor.Id &&
                d.AccessLevel.ToLower() != "visitor"
                && d.AccessLevel.ToLower() != "admin")
            .ToList();

        var reassignWindow = new ReassignPatientsWindow(patients, reassignableDoctors);

        if (reassignWindow.ShowDialog() == true)
        {
            // Зберігаємо переназначення
            foreach (var (patient, newDoctor) in reassignWindow.GetReassignments())
            {
                patient.DoctorId = newDoctor.Id;
                patient.Doctor = $"{newDoctor.FullName}";
                await mongoService.UpdatePatientAsync(patient);
            }

            // Видаляємо лікаря
            await mongoService.DeleteDoctorAsync(_doctor.Id);

            MessageBox.Show("Лікаря видалено, а пацієнтів переназначено.", "Успіх",
                MessageBoxButton.OK, MessageBoxImage.Information);

            LoadDoctors();
        }
    }
}