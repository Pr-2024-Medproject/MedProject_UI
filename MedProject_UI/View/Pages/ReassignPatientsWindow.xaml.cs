using MedProject_UI.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for ReassignPatientsWindow.xaml
/// </summary>
public partial class ReassignPatientsWindow : Window
{
    private readonly List<Patient> _patients;
    private readonly List<Doctor> _availableDoctors;
    private readonly Dictionary<string, ComboBox> _patientDoctorSelectors = new();

    public Dictionary<string, string> ReassignedMap { get; private set; } = new(); // PatientId => NewDoctorId

    public ReassignPatientsWindow(List<Patient> patients, List<Doctor> availableDoctors)
    {
        InitializeComponent();
        _patients = patients;
        _availableDoctors = availableDoctors;
        PopulatePatients();
    }

    private void PopulatePatients()
    {
        foreach (var patient in _patients)
        {
            var grid = new Grid
            {
                Margin = new Thickness(0)
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });  // Номер картки
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });  // ПІБ
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });  // КомбоБокс

            var fullNameBlock = new TextBlock
            {
                Text = $"{patient.LastName} {patient.FirstName} {patient.MiddleName}",
                FontSize = 16,
                FontFamily = new FontFamily("Roboto"),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };
            Grid.SetColumn(fullNameBlock, 0);

            var dbTextBlock = new TextBlock
            {
                Text = patient.BirthDate.ToString("dd/MM/yyyy"),
                FontSize = 16,
                FontFamily = new FontFamily("Roboto"),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };
            Grid.SetColumn(dbTextBlock, 1);

            var comboBox = new ComboBox
            {
                FontSize = 16,
                FontFamily = new FontFamily("Roboto"),
                Margin = new Thickness(10, 5, 10, 5),
                DisplayMemberPath = "FullName",
                SelectedValuePath = "Id",
                ItemsSource = _availableDoctors,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(comboBox, 2);

            grid.Children.Add(dbTextBlock);
            grid.Children.Add(fullNameBlock);
            grid.Children.Add(comboBox);

            var border = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(5),
                Margin = new Thickness(0, 5, 0, 5),
                Child = grid,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 270,
                    ShadowDepth = 2,
                    Opacity = 0.2,
                    BlurRadius = 6
                }
            };

            PatientsPanel.Children.Add(border);

            _patientDoctorSelectors[patient.Id] = comboBox;
        }
    }


    private void BtnTakeAll_Click(object sender, RoutedEventArgs e)
    {
        var currentUserId = App.CurrentUser?.Id;
        if (string.IsNullOrEmpty(currentUserId)) return;

        foreach (var selector in _patientDoctorSelectors.Values)
        {
            selector.SelectedValue = currentUserId;
        }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        ReassignedMap.Clear();
        foreach (var kvp in _patientDoctorSelectors)
        {
            var selectedDoctorId = kvp.Value.SelectedValue as string;
            if (string.IsNullOrEmpty(selectedDoctorId))
            {
                MessageBox.Show("Усі пацієнти повинні бути переназначені.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            ReassignedMap[kvp.Key] = selectedDoctorId;
        }

        DialogResult = true;
        Close();
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    public List<(Patient, Doctor)> GetReassignments()
    {
        var result = new List<(Patient, Doctor)>();

        foreach (var patient in _patients)
        {
            if (ReassignedMap.TryGetValue(patient.Id, out var newDoctorId))
            {
                var doctor = _availableDoctors.Find(d => d.Id == newDoctorId);
                if (doctor != null)
                {
                    result.Add((patient, doctor));
                }
            }
        }

        return result;
    }
}