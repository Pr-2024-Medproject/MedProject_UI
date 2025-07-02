using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MedProject_UI.Helpers;
using MedProject_UI.Models;
using MedProject_UI.Services;

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
        Close();
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

        var hasError = false;

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

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            tbEmail.Style = (Style)FindResource("ErrorTextBox");
            MessageBox.Show("Невірний формат email.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var existingDoctor = await _mongoService.GetDoctorByEmailAsync(email);
        if (existingDoctor != null)
        {
            MessageBox.Show("Лікар з таким email вже існує в системі.", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
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

        if ((startDate.Value - birthDate.Value).TotalDays < 16 * 365.25)
        {
            MessageBox.Show("Дата початку роботи повинна бути не раніше ніж після досягнення 16 років.", "Помилка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
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

        if (password.Length < 8)
        {
            MessageBox.Show("Пароль повинен містити щонайменше 8 символів.", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        if (hasError)
        {
            MessageBox.Show("Будь ласка, заповніть усі обов'язкові поля.", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Warning);
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
            Username = $"{Transliterate(lastName)}_{Transliterate(firstName)}_{birthDate.Value.ToString("dd.MM.yyyy")}",
            Position = position,
            Phone = phone,
            Email = email,
            Address = address,
            BirthDate = birthDate.Value,
            StartDate = startDate.Value,
            PasswordHash = PasswordHelper.HashPassword(password),
            AccessLevel = cbAccessLevel.SelectedValue?.ToString() ?? "doctor", // За замовчуванням
            PatientIds = new List<string>(),
            WorkSchedule = new List<WorkShift>()
        };

        try
        {
            await _mongoService.AddDoctorAsync(newDoctor);
            MessageBox.Show("Доктора успішно зареєстровано!", "Успіх", MessageBoxButton.OK,
                MessageBoxImage.Information);
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Помилка при реєстрації: " + ex.Message, "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
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

    private void UkrainianOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        // Дозволено тільки українські літери без пробілів
        var regex = new Regex("^[а-щА-ЩьЬюЮяЯїЇєЄіІґҐ']+$");
        e.Handled = !regex.IsMatch(e.Text);
    }

    private string Transliterate(string text)
    {
        var map = new Dictionary<char, string>
        {
            ['а'] = "a",
            ['б'] = "b",
            ['в'] = "v",
            ['г'] = "h",
            ['ґ'] = "g",
            ['д'] = "d",
            ['е'] = "e",
            ['є'] = "ie",
            ['ж'] = "zh",
            ['з'] = "z",
            ['и'] = "y",
            ['і'] = "i",
            ['ї'] = "i",
            ['й'] = "i",
            ['к'] = "k",
            ['л'] = "l",
            ['м'] = "m",
            ['н'] = "n",
            ['о'] = "o",
            ['п'] = "p",
            ['р'] = "r",
            ['с'] = "s",
            ['т'] = "t",
            ['у'] = "u",
            ['ф'] = "f",
            ['х'] = "kh",
            ['ц'] = "ts",
            ['ч'] = "ch",
            ['ш'] = "sh",
            ['щ'] = "shch",
            ['ь'] = "",
            ['ю'] = "iu",
            ['я'] = "ia",
            ['ґ'] = "g",
            ['’'] = "",
            ['\''] = ""
        };

        return string.Concat(text.Select(c =>
        {
            var lower = char.ToLower(c);
            var result = map.ContainsKey(lower) ? map[lower] : c.ToString();
            return char.IsUpper(c) ? Capitalize(result) : result;
        }));
    }

    private string Capitalize(string input)
    {
        return string.IsNullOrEmpty(input) ? "" : input[..1].ToUpper() + input[1..];
    }
}