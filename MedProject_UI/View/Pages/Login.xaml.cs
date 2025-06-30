using System.Windows;
using System.Windows.Input;
using MedProject_UI.View.Pages;

namespace MedProject_UI;

/// <summary>
///     Interaction logic for Login.xaml
/// </summary>
public partial class Login : Window
{
    public Login()
    {
        InitializeComponent();
    }

    private void BTN_Login_Click(object sender, RoutedEventArgs e)
    {
        //if (TB_Login.Text == "Anastasiia" && TB_Pass.Password == "1806")
        //{
        //    MainWindow main = new MainWindow();
        //    this.Close();
        //    main.Show();
        //}
        //else {
        //    MessageBox.Show("Введений пароль або логін не правильні!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
        //}

        var main = new MainWindow();
        Close();
        main.Show();
    }

    private void RegisterLink_Click(object sender, MouseButtonEventArgs e)
    {
        var registerWindow = new RegisterDoctorWindow();
        registerWindow.ShowDialog();
    }
}