using MedProject_UI.Helpers;
using MedProject_UI.Models;
using MedProject_UI.Services;
using System.Windows;
using System.Windows.Media;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for RegisterDoctorWindow.xaml
/// </summary>
public partial class RegisterDoctorWindow : Window
{
    private readonly MongoDbService _mongoService;

    public RegisterDoctorWindow()
    {
        InitializeComponent();

        var config = AppConfig.Load();

        _mongoService = new MongoDbService(
            config.MongoDbConnection,
            config.DatabaseName
        );
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private async void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        // Отримання даних з полів
        var firstName = tbFirstName.Text.Trim();
        var lastName = tbLastName.Text.Trim();
        var middleName = tbMiddleName.Text.Trim();
        var position = cbPosition.SelectionBoxItem?.ToString();
        var phone = tbPhone.Text.Trim();
        var email = tbEmail.Text.Trim();
        var address = tbAddress.Text.Trim();
        var birthDate = dpBirthDate.SelectedDate;
        var startDate = dpStartDate.SelectedDate;
        var password = pbPassword.Password.Trim();
        var confirmPassword = pbConfirmPassword.Password.Trim();

        ResetFieldHighlights();

        bool hasError = false;

        #region Перевірка необхідних полів

        if (string.IsNullOrWhiteSpace(firstName))
        {
            tbFirstName.Style = (Style)FindResource("ErrorTextBox");
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            tbLastName.Style = (Style)FindResource("ErrorTextBox");
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(middleName))
        {
            tbMiddleName.Style = (Style)FindResource("ErrorTextBox");
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(position))
        {
            cbPosition.Style = (Style)FindResource("ErrorComboBox");
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(phone))
        {
            tbPhone.Style = (Style)FindResource("ErrorTextBox");
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            tbEmail.Style = (Style)FindResource("ErrorTextBox");
            hasError = true;
        }
        if (birthDate == null)
        {
            dpBirthDate.Style = (Style)FindResource("ErrorDatePicker");
            hasError = true;
        }
        if (startDate == null)
        {
            dpStartDate.Style = (Style)FindResource("ErrorDatePicker");
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            pbPassword.BorderBrush = Brushes.Red;
            pbPassword.BorderThickness = new Thickness(2);
            hasError = true;
        }
        if (string.IsNullOrWhiteSpace(confirmPassword))
        {
            pbConfirmPassword.BorderBrush = Brushes.Red;
            pbConfirmPassword.BorderThickness = new Thickness(2);
            hasError = true;
        }

        if (hasError)
        {
            MessageBox.Show("Будь ласка, заповніть усі обов'язкові поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        #endregion

        if (password != confirmPassword)
        {
            MessageBox.Show("Паролі не співпадають.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Створення нового об'єкта Doctor
        var newDoctor = new Doctor
        {
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Position = position,
            Phone = phone,
            Email = email,
            Address = address,
            BirthDate = birthDate.Value,
            StartDate = startDate.Value,
            PasswordHash = PasswordHelper.HashPassword(password),
            AccessLevel = cbAccessLevel.SelectionBoxItem?.ToString() ?? "doctor", // За замовчуванням
            PatientIds = new List<string>(),
            WorkSchedule = new List<WorkShift>()
        };

        try
        {
            await _mongoService.AddDoctorAsync(newDoctor);
            MessageBox.Show("Доктора успішно зареєстровано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Помилка при реєстрації: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ResetFieldHighlights()
    {
        // Відновлення стилів
        tbFirstName.ClearValue(StyleProperty);
        tbLastName.ClearValue(StyleProperty);
        tbMiddleName.ClearValue(StyleProperty);
        tbPhone.ClearValue(StyleProperty);
        tbEmail.ClearValue(StyleProperty);
        tbAddress.ClearValue(StyleProperty);
        cbPosition.ClearValue(StyleProperty);
        cbAccessLevel.ClearValue(StyleProperty);
        dpBirthDate.ClearValue(StyleProperty);
        dpStartDate.ClearValue(StyleProperty);

        pbPassword.BorderBrush = Brushes.Gray;
        pbPassword.BorderThickness = new Thickness(1);

        pbConfirmPassword.BorderBrush = Brushes.Gray;
        pbConfirmPassword.BorderThickness = new Thickness(1);
    }
}