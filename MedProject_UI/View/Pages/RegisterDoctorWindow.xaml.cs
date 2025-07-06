using MedProject_UI.Helpers;
using MedProject_UI.Models;
using MedProject_UI.Services;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for RegisterDoctorWindow.xaml
/// </summary>
public partial class RegisterDoctorWindow : Window
{
    private readonly MongoDbService _mongoService;
    private readonly bool _isEditMode = false;
    private readonly bool _isSelfEdit;
    private readonly Doctor _editingDoctor;


    public RegisterDoctorWindow()
    {
        InitializeComponent();

        var config = AppConfig.Load();

        _mongoService = new MongoDbService(
            config.MongoDbConnection,
            config.DatabaseName
        );

        cbAccessLevel.ItemsSource = new List<AccessLevelItem>
        {
            new AccessLevelItem { Value = "visitor", Display = "Відвідувач" },
            new AccessLevelItem { Value = "doctor", Display = "Лікар" }
        };
        cbAccessLevel.SelectedIndex = 0;
    }

    public RegisterDoctorWindow(Doctor doctorToEdit, bool isSelfEdit = false) : this()
    {
        _isEditMode = true;
        _editingDoctor = doctorToEdit;
        _isSelfEdit = isSelfEdit;

        Title = "Редагування доктора";
        btnRegister.Content = "Зберегти";

        // Заповнення полів
        tbFirstName.Text = doctorToEdit.FirstName;
        tbLastName.Text = doctorToEdit.LastName;
        tbMiddleName.Text = doctorToEdit.MiddleName;
        cbPosition.Text = doctorToEdit.Position;
        tbPhone.Text = doctorToEdit.Phone;
        tbEmail.Text = doctorToEdit.Email;
        tbAddress.Text = doctorToEdit.Address;
        dpBirthDate.SelectedDate = doctorToEdit.BirthDate;
        dpStartDate.SelectedDate = doctorToEdit.StartDate;
        cbAccessLevel.SelectedValue = doctorToEdit.AccessLevel;

        // Заблоковані поля
        tbEmail.IsEnabled = _isSelfEdit;
        if (App.CurrentUser?.AccessLevel != "admin") dpBirthDate.IsEnabled = false;
        pbPassword.IsEnabled = _isSelfEdit;
        pbConfirmPassword.IsEnabled = _isSelfEdit;

        // Розширені варіанти рівня доступу
        cbAccessLevel.ItemsSource = new List<AccessLevelItem>
        {
            new AccessLevelItem { Value = "admin", Display = "Адміністратор" },
            new AccessLevelItem { Value = "chief_doctor", Display = "Головний лікар" },
            new AccessLevelItem { Value = "doctor", Display = "Лікар" },
            new AccessLevelItem { Value = "visitor", Display = "Відвідувач" }
        };

        cbAccessLevel.SelectedItem = ((List<AccessLevelItem>)cbAccessLevel.ItemsSource)
            .FirstOrDefault(item => item.Value == _editingDoctor.AccessLevel);

        cbAccessLevel.IsEnabled = !_isSelfEdit;
        cbPosition.IsEnabled = !_isSelfEdit;
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private async void btnRegister_Click(object sender, RoutedEventArgs e)
    {
        bool hasErrors = false;

        // Очищення попередніх підсвічувань
        void ClearBorders()
        {
            tbFirstName.ClearValue(Border.BorderBrushProperty);
            tbLastName.ClearValue(Border.BorderBrushProperty);
            tbMiddleName.ClearValue(Border.BorderBrushProperty);
            cbPosition.ClearValue(Border.BorderBrushProperty);
            tbEmail.ClearValue(Border.BorderBrushProperty);
            tbPhone.ClearValue(Border.BorderBrushProperty);
            tbAddress.ClearValue(Border.BorderBrushProperty);
            cbAccessLevel.ClearValue(Border.BorderBrushProperty);
            dpStartDate.ClearValue(Border.BorderBrushProperty);
            if (!_isEditMode || _isSelfEdit)
            {
                pbPassword.ClearValue(Border.BorderBrushProperty);
                pbConfirmPassword.ClearValue(Border.BorderBrushProperty);
            }
        }

        void MarkError(Control control)
        {
            control.BorderBrush = Brushes.Red;
        }

        ClearBorders();

        // Перевірка обов'язкових полів
        if (string.IsNullOrWhiteSpace(tbFirstName.Text)) { MarkError(tbFirstName); hasErrors = true; }
        if (string.IsNullOrWhiteSpace(tbLastName.Text)) { MarkError(tbLastName); hasErrors = true; }
        if (string.IsNullOrWhiteSpace(tbMiddleName.Text)) { MarkError(tbMiddleName); hasErrors = true; }
        if (string.IsNullOrWhiteSpace(cbPosition.Text)) { MarkError(cbPosition); hasErrors = true; }
        if (string.IsNullOrWhiteSpace(dpBirthDate.Text)) { MarkError(dpBirthDate); hasErrors = true; }
        if (string.IsNullOrWhiteSpace(tbEmail.Text)) { MarkError(tbEmail); hasErrors = true; }
        if (string.IsNullOrWhiteSpace(tbPhone.Text) || tbPhone.Text == "+38(___)___-__-__") { MarkError(tbPhone); hasErrors = true; }
        if (cbAccessLevel.SelectedItem == null) { MarkError(cbAccessLevel); hasErrors = true; }
        if (dpStartDate.SelectedDate == null) { MarkError(dpStartDate); hasErrors = true; }

        if (!_isEditMode || _isSelfEdit)
        {
            if (string.IsNullOrWhiteSpace(pbPassword.Password)) { MarkError(pbPassword); hasErrors = true; }
            if (string.IsNullOrWhiteSpace(pbConfirmPassword.Password)) { MarkError(pbConfirmPassword); hasErrors = true; }
            if (pbPassword.Password != pbConfirmPassword.Password)
            {
                MarkError(pbPassword);
                MarkError(pbConfirmPassword);
                MessageBox.Show("Паролі не співпадають.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        if (hasErrors)
        {
            MessageBox.Show("Будь ласка, заповніть усі обов'язкові поля.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (_isEditMode)
        {
            _editingDoctor.FirstName = tbFirstName.Text.Trim();
            _editingDoctor.LastName = tbLastName.Text.Trim();
            _editingDoctor.MiddleName = tbMiddleName.Text.Trim();
            _editingDoctor.Phone = tbPhone.Text.Trim();
            if (_isSelfEdit)
            {
                var exists = await _mongoService.CheckDoctorExistsByEmailAsync(tbEmail.Text.Trim());
                if (exists)
                {
                    MessageBox.Show("Користувач з таким Email вже існує.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MarkError(tbEmail);
                    return;
                }

                _editingDoctor.Email = tbEmail.Text.Trim();
                _editingDoctor.PasswordHash = PasswordHelper.HashPassword(pbPassword.Password);
            }
            _editingDoctor.Address = tbAddress.Text.Trim();
            _editingDoctor.Position = cbPosition.Text.Trim();
            _editingDoctor.StartDate = dpStartDate.SelectedDate ?? DateTime.Today;
            var selectedAccessItem = cbAccessLevel.SelectedItem as AccessLevelItem;
            if (selectedAccessItem != null)
            {
                _editingDoctor.AccessLevel = selectedAccessItem.Value;
            }


            await _mongoService.UpdateDoctorAsync(_editingDoctor);
            MessageBox.Show("Дані лікаря оновлено!", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        else
        {
            var newDoctor = new Doctor
            {
                FirstName = tbFirstName.Text.Trim(),
                LastName = tbLastName.Text.Trim(),
                MiddleName = tbMiddleName.Text.Trim(),
                Email = tbEmail.Text.Trim(),
                Phone = tbPhone.Text.Trim(),
                Address = tbAddress.Text.Trim(),
                Position = cbPosition.Text.Trim(),
                BirthDate = dpBirthDate.SelectedDate ?? DateTime.MinValue,
                StartDate = dpStartDate.SelectedDate ?? DateTime.Today,
                Username = tbEmail.Text.Trim(),
                PasswordHash = PasswordHelper.HashPassword(pbPassword.Password)
            };
            var selectedAccessItem = cbAccessLevel.SelectedItem as AccessLevelItem;
            if (selectedAccessItem != null)
            {
                newDoctor.AccessLevel = selectedAccessItem.Value;
            }

            var exists = await _mongoService.CheckDoctorExistsByEmailAsync(newDoctor.Email);
            if (exists)
            {
                MessageBox.Show("Користувач з таким Email вже існує.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                MarkError(tbEmail);
                return;
            }

            await _mongoService.AddDoctorAsync(newDoctor);
            MessageBox.Show("Користувача успішно зареєстровано!", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        DialogResult = true;
        Close();
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

    public class AccessLevelItem
    {
        public string Value { get; set; }      // внутрішнє значення, що зберігається
        public string Display { get; set; }    // текст, який бачить користувач

        public override string ToString() => Display; // Для відображення в ComboBox
    }
}