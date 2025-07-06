using MedProject_UI.Models;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for WorkScheduleWindow.xaml
/// </summary>
public partial class WorkScheduleWindow : Window
{
    private readonly List<Doctor> _doctors;
    private DateTime SelectedMonth;

    public enum WorkStatus
    {
        None,   // Н (сірий)
        Work,   // Р (зелений)
        Vacation, // В (рожевий)
        Holiday   // В (помаранчевий)
    }

    public WorkScheduleWindow(List<Doctor> doctors)
    {
        InitializeComponent();
        Loaded += WorkScheduleWindow_Loaded;
        _doctors = doctors;
        InitializeMonthSelector(); 
    }

    private void GenerateScheduleGrid()
    {
        var daysInMonth = DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month);
        var month = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1);

        CultureInfo culture = new("uk-UA");

        // Додати заголовок таблиці
        var headerRow = new Grid
        {
            Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)),
            Margin = new Thickness(0, 0, 0, 4)
        };

        headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) }); // "Співробітник"

        for (var i = 0; i < daysInMonth; i++)
            headerRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

        var nameHeader = new TextBlock
        {
            Text = "Лікар",
            FontWeight = FontWeights.Bold,
            FontSize = 16,
            FontFamily = new FontFamily("Roboto"),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(5)
        };
        Grid.SetColumn(nameHeader, 0);
        headerRow.Children.Add(nameHeader);

        for (int i = 0; i < daysInMonth; i++)
        {
            var dayDate = month.AddDays(i);
            var isWeekend = dayDate.DayOfWeek == DayOfWeek.Saturday || dayDate.DayOfWeek == DayOfWeek.Sunday;

            var header = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = isWeekend ? new SolidColorBrush(Color.FromRgb(230, 230, 230)) : Brushes.Transparent,
                Margin = new Thickness(0)
            };

            header.Children.Add(new TextBlock
            {
                Text = dayDate.Day.ToString(),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                FontFamily = new FontFamily("Roboto"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(5,0,5,0)
            });

            header.Children.Add(new TextBlock
            {
                Text = culture.DateTimeFormat.GetAbbreviatedDayName(dayDate.DayOfWeek),
                FontSize = 10,
                Foreground = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            Grid.SetColumn(header, i + 1);
            headerRow.Children.Add(header);
        }

        ScheduleRowsPanel.Items.Add(headerRow);

        foreach (var doctor in _doctors)
        {
            var row = new Grid
            {
                Margin = new Thickness(0, 5, 0, 5),
                Background = Brushes.White
            };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });

            for (var i = 0; i < daysInMonth; i++)
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });

            var nameBlock = new TextBlock
            {
                Text = doctor.ShortName,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5),
                FontSize = 16,
                FontFamily = new FontFamily("Roboto")
            };
            Grid.SetColumn(nameBlock, 0);
            row.Children.Add(nameBlock);

            for (var day = 1; day <= daysInMonth; day++)
            {
                var cell = CreateStatusButton();

                cell.ToolTip = culture.DateTimeFormat.GetDayName(month.AddDays(day - 1).DayOfWeek);

                Grid.SetColumn(cell, day);
                row.Children.Add(cell);
            }

            ScheduleRowsPanel.Items.Add(row);
        }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        // TODO: Зберегти графік у _doctors[i].WorkSchedule
        MessageBox.Show("Графік збережено", "Успішно", MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void InitializeMonthSelector()
    {
        var currentYear = DateTime.Now.Year;
        var months = Enumerable.Range(0, 12)
            .Select(offset => new DateTime(currentYear, 1, 1).AddMonths(offset))
            .ToList();

        cbMonthSelector.ItemsSource = months;

        var currentMonth = months.FirstOrDefault(m => m.Month == DateTime.Now.Month);
        cbMonthSelector.SelectedItem = currentMonth;
    }

    private void CbMonthSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbMonthSelector.SelectedItem is DateTime selected)
        {
            SelectedMonth = selected;
            RefreshScheduleGrid();
        }
    }

    private void MonthTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is TextBlock textBlock && textBlock.DataContext is DateTime date)
        {
            var culture = new CultureInfo("uk-UA");
            textBlock.Text = date.ToString("MMMM yyyy", culture);
        }
    }

    private void WorkScheduleWindow_Loaded(object sender, RoutedEventArgs e)
    {
        MinWidth = ActualWidth;
        MinHeight = ActualHeight;
    }

    private void RefreshScheduleGrid()
    {
        ScheduleRowsPanel.Items.Clear();
        GenerateScheduleGrid();
    }
    private Button CreateStatusButton()
    {
        var button = new Button
        {
            Content = "Н",
            Tag = WorkStatus.None,
            Background = new SolidColorBrush(Color.FromRgb(206, 212, 218)), // Сірий
            Style = (Style)FindResource("ScheduleStatusButtonStyle")
        };

        button.Click += (s, e) =>
        {
            if (s is Button btn && btn.Tag is WorkStatus current)
            {
                var next = (WorkStatus)(((int)current + 1) % Enum.GetValues(typeof(WorkStatus)).Length);
                btn.Tag = next;

                switch (next)
                {
                    case WorkStatus.None:
                        btn.Content = "Н";
                        btn.Background = new SolidColorBrush(Color.FromRgb(206, 212, 218)); // Gray
                        break;
                    case WorkStatus.Work:
                        btn.Content = "Р";
                        btn.Background = new SolidColorBrush(Color.FromRgb(128, 201, 156)); // Green
                        break;
                    case WorkStatus.Vacation:
                        btn.Content = "В";
                        btn.Background = new SolidColorBrush(Color.FromRgb(248, 168, 181)); // Pink
                        break;
                    case WorkStatus.Holiday:
                        btn.Content = "В";
                        btn.Background = new SolidColorBrush(Color.FromRgb(249, 196, 127)); // Orange
                        break;
                }
            }
        };

        return button;
    }
}