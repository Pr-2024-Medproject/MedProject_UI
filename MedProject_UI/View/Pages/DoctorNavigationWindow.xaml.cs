﻿using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using MedProject_UI.Models;
using MedProject_UI.Services;
using Microsoft.Win32;
using Newtonsoft.Json;

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
        var btnSelfEdit = CreateButton("Редагувати власні дані", BtnEditSelf_Click);
        var btnExit = CreateButton("Вихід", BtnExit_Click);


        mainPanel.Children.Add(btnDoctors);
        mainPanel.Children.Add(btnPatients);
        mainPanel.Children.Add(btnSelfEdit);

        // Графік: назва змінюється в залежності від ролі
        if (_doctor.AccessLevel == "doctor" || _doctor.AccessLevel == "chief_doctor")
        {
            var btnExport = CreateButton("Експортувати пацієнтів", BtnExportPatients_Click);
            mainPanel.Children.Add(btnExport);
        }

        if (_doctor.AccessLevel == "chief_doctor")
        {
            var btnImport = CreateButton("Імпортувати пацієнтів", BtnImportPatients_Click);
            mainPanel.Children.Add(btnImport);
        }


        // Додаємо кнопки до панелі
        if (_doctor.AccessLevel.Contains("doctor")) 
        {
            Button btnSchedule;
            if (_doctor.AccessLevel == "chief_doctor")
                btnSchedule = CreateButton("Редагувати графік", BtnEditSchedule_Click);
            else
                btnSchedule = CreateButton("Переглянути графік", BtnViewSchedule_Click);
            mainPanel.Children.Add(btnSchedule);
        }
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
        var doctorListWindow = new DoctorsWindow(); // назва  вікна перегляду докторів
        Hide();
        doctorListWindow.ShowDialog();
        Show();
    }

    private void BtnViewPatients_Click(object sender, RoutedEventArgs e)
    {
        var patientsWindow = new MainWindow();
        Hide();
        patientsWindow.ShowDialog();
        Show();
    }

    private void BtnEditSelf_Click(object sender, RoutedEventArgs e)
    {
        var editWindow = new RegisterDoctorWindow(_doctor, isSelfEdit: true);
        Hide();
        if (editWindow.ShowDialog() == true)
        {
            txtGreeting.Text = $"Вітаю, {_doctor.FirstName} {_doctor.LastName}!";
        }
        Show();
    }

    private void BtnViewSchedule_Click(object sender, RoutedEventArgs e)
    {
        var doctorOnlyList = new List<Doctor> { _doctor };

        var scheduleWindow = new WorkScheduleWindow(doctorOnlyList, isReadOnly: true);
        Hide();
        scheduleWindow.ShowDialog();
        Show();
    }


    private async void BtnEditSchedule_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var editableList = await GetEditableDoctors() ?? new List<Doctor>();
            if (editableList.Count > 0)
            {
                var scheduleWindow = new WorkScheduleWindow(editableList);
                Hide();
                scheduleWindow.ShowDialog();
                Show();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка при завантаженні графіка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void BtnExportPatients_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            ExportData();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка експорту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private async void BtnImportPatients_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Оберіть ZIP-файл для імпорту",
            Filter = "ZIP files (*.zip)|*.zip"
        };

        if (openFileDialog.ShowDialog() != true)
            return;

        try
        {
            ImportData(openFileDialog.FileName);
        } 
        catch(Exception ex)
        {
            MessageBox.Show($"Помилка при імпорті: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        
    }


    private void BtnExit_Click(object sender, RoutedEventArgs e)
    {
        var loginWindowBack = new Login();
        Close();
        loginWindowBack.ShowDialog();
    }

    private async void ExportData()
    {
        try
        {
            using (var _mongoService = new MongoDbService(AppConfig.Load().MongoDbConnection, AppConfig.Load().DatabaseName)) {
                // Отримання пацієнтів згідно з роллю
                var patients = _doctor.AccessLevel == "chief_doctor"
                    ? await _mongoService.GetAllPatientsAsync()
                    : await _mongoService.GetPatientsByDoctorIdAsync(_doctor.Id);

                if (patients.Count == 0)
                {
                    MessageBox.Show("Немає пацієнтів для експорту.", "Інформація", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }

                // Мінімізований формат: зберігаємо лише потрібні поля
                var minimalPatients = patients.Select(p => new
                {
                    p.Id,
                    p.CardNumber,
                    p.FirstName,
                    p.LastName,
                    p.MiddleName,
                    p.BirthDate,
                    p.Age,
                    p.Address,
                    p.Profession,
                    p.Phone,
                    p.Email,
                    p.Gender,
                    p.Doctor,
                    p.DoctorId,
                    p.Visits
                });

                var json = JsonConvert.SerializeObject(minimalPatients, Formatting.None);

                // Діалог збереження
                var saveDialog = new SaveFileDialog
                {
                    Title = "Зберегти архів пацієнтів",
                    Filter = "ZIP files (*.zip)|*.zip",
                    FileName = "exported_patients.zip"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    using (var zipStream = new FileStream(saveDialog.FileName, FileMode.Create))
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                    {
                        var entry = archive.CreateEntry("patients.json", CompressionLevel.Optimal);
                        using var entryStream = entry.Open();
                        using var writer = new StreamWriter(entryStream);
                        writer.Write(json);
                    }

                    MessageBox.Show("Пацієнтів експортовано у архів успішно.", "Успіх", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка експорту: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void ImportData(string zipFileName)
    {
        try
        {
            using (var _mongoService = new MongoDbService(AppConfig.Load().MongoDbConnection, AppConfig.Load().DatabaseName))
            { 
                using var zip = ZipFile.OpenRead(zipFileName);

                // Перевірка, що всередині лише один файл і це .json
                if (zip.Entries.Count != 1 || !zip.Entries[0].Name.EndsWith(".json"))
                {
                    MessageBox.Show("Архів має містити один JSON-файл.", "Помилка структури", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

                // Зчитування JSON
                string jsonContent;
                using (var stream = zip.Entries[0].Open())
                using (var reader = new StreamReader(stream))
                {
                    jsonContent = reader.ReadToEnd();
                }

                // Десеріалізація
                var importedPatients = JsonConvert.DeserializeObject<List<Patient>>(jsonContent);

                if (importedPatients == null || importedPatients.Count == 0)
                {
                    MessageBox.Show("Файл не містить даних пацієнтів.", "Помилка", MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                // Перевірити скільки нових/оновлюваних
                var existingPatients = await _mongoService.GetAllPatientsAsync();
                var existingIds = new HashSet<string>(existingPatients.Select(p => p.Id));

                var toUpdate = importedPatients.Where(p => existingIds.Contains(p.Id)).ToList();
                var toInsert = importedPatients.Where(p => !existingIds.Contains(p.Id)).ToList();

                // Запит підтвердження
                var msg = $"Буде оновлено {toUpdate.Count} пацієнтів та додано {toInsert.Count}. Продовжити?";
                var result = MessageBox.Show(msg, "Підтвердження імпорту", MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    return;

                // Вставлення та оновлення
                foreach (var patient in toInsert)
                    await _mongoService.AddPatientAsync(patient);

                foreach (var patient in toUpdate)
                    await _mongoService.UpdatePatientAsync(patient);

                MessageBox.Show("Імпорт завершено успішно.", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка при імпорті: {ex.Message}", "Помилка", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private async Task<List<Doctor>?> GetEditableDoctors()
    {
        try
        {
            using var mongoService = new MongoDbService(AppConfig.Load().MongoDbConnection, AppConfig.Load().DatabaseName);
            var allDoctors = await mongoService.GetAllDoctorsAsync();

            // Отримати список лікарів, яких можна відображати (без admin, chief_doctor, visitor)
            var editableDoctors = allDoctors
                .Where(d => d.AccessLevel == "doctor")
                .ToList();

            if (editableDoctors.Count == 0)
            {
                MessageBox.Show("Немає лікарів для відображення.", "Інформація", MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            return editableDoctors;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка при завантаженні графіка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            return null;
        }
    }
}