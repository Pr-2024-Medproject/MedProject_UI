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
        var config = AppConfig.Load();
        var mongoService = new MongoDbService(config.MongoDbConnection, config.DatabaseName);
        allDoctors = await mongoService.GetAllDoctorsAsync();

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
            filtered = filtered.Where(d => IsDoctorOnDuty(d)).ToList(); // метод додамо пізніше

        DoctorsGrid.ItemsSource = filtered;
    }

    private bool IsDoctorOnDuty(Doctor doctor)
    {
        // TODO: реалізувати перевірку по графіку
        return false;
    }

    private void BtnEdit_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Doctor selectedDoctor)
        {
            // TODO: Відкрити форму редагування лікаря
            MessageBox.Show($"Редагування: {selectedDoctor.LastName} {selectedDoctor.FirstName}");
        }
    }

    private void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is Doctor selectedDoctor)
        {
            var result = MessageBox.Show($"Ви впевнені, що хочете видалити лікаря {selectedDoctor.LastName}?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var config = AppConfig.Load();
                var mongoService = new MongoDbService(config.MongoDbConnection, config.DatabaseName);
                mongoService.DeleteDoctorAsync(selectedDoctor.Id);

                // Оновити список
                LoadDoctors();
            }
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
}