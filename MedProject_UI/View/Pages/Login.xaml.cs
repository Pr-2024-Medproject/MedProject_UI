using MedProject_UI.Helpers;
using MedProject_UI.Services;
using MedProject_UI.View.Pages;
using System.Windows;
using System.Windows.Input;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for Login.xaml
/// </summary>
public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();

        //!!!FOR TEST PURPOSE ONLY!!!!
        tbEmail.Text = "some.email@email.com";
        tbPass.Password = "123456";
    }

    private async void BTN_Login_Click(object sender, RoutedEventArgs e)
    {
        var email = tbEmail.Text.Trim();
        var password = tbPass.Password;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show("Будь ласка, заповніть обидва поля для входу.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var config = AppConfig.Load();
            var mongoService = new MongoDbService(config.MongoDbConnection, config.DatabaseName);

            var doctor = await mongoService.GetDoctorByEmailAsync(email);
            if (doctor == null)
            {
                MessageBox.Show("Користувача з таким email не знайдено.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!PasswordHelper.VerifyPassword(password, doctor.PasswordHash))
            {
                MessageBox.Show("Неправильний пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (doctor.AccessLevel == "doctor" || doctor.AccessLevel == "chief_doctor" || doctor.AccessLevel == "admin")
            {
                App.CurrentUser = doctor;
                
                var doctorNav = new DoctorNavigationWindow(doctor);
                Close();
                doctorNav.ShowDialog();
            }
            else
            {
                var main = new MainWindow();
                Hide();
                main.ShowDialog();
                tbPass.Password = "";
                Show();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Сталася помилка при вході: " + ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RegisterLink_Click(object sender, MouseButtonEventArgs e)
    {
        var registerWindow = new RegisterDoctorWindow();
        registerWindow.ShowDialog();
    }
}