using System.Windows;
using System.Windows.Controls;
using MedProject_UI.Models;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for DoctorNavigationWindow.xaml
/// </summary>
public partial class DoctorNavigationWindow : Window
{
    private readonly Doctor _doctor;

    public DoctorNavigationWindow(Doctor doctor)
    {
        InitializeComponent();
        _doctor = doctor;
        txtGreeting.Text = $"Вітаю, {_doctor.FirstName} {_doctor.LastName}!";

        RenderButtonsBasedOnRole();
    }

    private void RenderButtonsBasedOnRole()
    {
        // Створюємо кнопки
        var btnDoctors = CreateButton("Переглянути список докторів", BtnViewDoctors_Click);
        var btnPatients = CreateButton("Переглянути список пацієнтів", BtnViewPatients_Click);
        var btnExit = CreateButton("Вихід", BtnExit_Click);

        // Графік: назва змінюється в залежності від ролі
        Button btnSchedule;
        if (_doctor.AccessLevel == "chief_doctor" || _doctor.AccessLevel == "admin")
            btnSchedule = CreateButton("Редагувати графік", BtnEditSchedule_Click);
        else
            btnSchedule = CreateButton("Переглянути графік", BtnViewSchedule_Click);

        // Додаємо кнопки до панелі
        mainPanel.Children.Add(btnDoctors);
        mainPanel.Children.Add(btnPatients);
        mainPanel.Children.Add(btnSchedule);
        mainPanel.Children.Add(btnExit);
    }

    private Button CreateButton(string content, RoutedEventHandler clickHandler)
    {
        var button = new Button
        {
            Content = content,
            Height = 45,
            Style = FindResource("BootstrapButtonPrimary") as Style
        };
        button.Click += clickHandler;
        return button;
    }

    // Обробники
    private void BtnViewDoctors_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Форма перегляду лікарів у розробці.", "Інформація");
    }

    private void BtnViewPatients_Click(object sender, RoutedEventArgs e)
    {
        var patientsWindow = new MainWindow();
        Hide();
        patientsWindow.ShowDialog();
        Show();
    }

    private void BtnViewSchedule_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Форма перегляду графіка у розробці.", "Інформація");
    }

    private void BtnEditSchedule_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Форма редагування графіка у розробці.", "Інформація");
    }

    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}