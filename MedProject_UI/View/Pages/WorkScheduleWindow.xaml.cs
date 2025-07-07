using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MedProject_UI.Models;
using MedProject_UI.Services;

namespace MedProject_UI.View.Pages;

/// <summary>
///     Interaction logic for WorkScheduleWindow.xaml
/// </summary>
public partial class WorkScheduleWindow : Window
{
    private readonly List<Doctor> _doctors;
    private DateTime SelectedMonth;
    private readonly bool _isReadOnly;

    public WorkScheduleWindow(List<Doctor> doctors, bool isReadOnly = false)
    {
        InitializeComponent();
        Loaded += WorkScheduleWindow_Loaded;
        _doctors = doctors;
        _isReadOnly = isReadOnly;
        SetAutoButtonText();
        InitializeMonthSelector();
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        CollectDoctorWorkSchedules();

        var config = AppConfig.Load();
        var mongoService = new MongoDbService(config.MongoDbConnection, config.DatabaseName);

        var successCount = 0;
        foreach (var doctor in _doctors)
        {
            var updated = await mongoService.UpdateDoctorScheduleAsync(doctor.Id, doctor.WorkSchedule);
            if (updated) successCount++;
            else
                MessageBox.Show(
                    "Графік не було збережено!",
                    "Помилка!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }

        MessageBox.Show(
            $"Графік збережено для {successCount} з {_doctors.Count} лікарів.",
            "Успішно",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CbMonthSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (cbMonthSelector.SelectedItem is DateTime selected)
        {
            SelectedMonth = selected;
            RefreshScheduleGrid();
        }
    }

    private void BtnAutoDistribute_Click_Chief(object sender, RoutedEventArgs e)
{
    AutoDistributeSchedule();
    MessageBox.Show("Графік успішно згенеровано!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
    RefreshScheduleGrid();
}

    private void BtnAutoDistribute_Click_Doctor(object sender, RoutedEventArgs e)
    {
        if (SelectedMonth.Month != DateTime.Now.Month || SelectedMonth.Year != DateTime.Now.Year)
        {
            MessageBox.Show("Можна пропонувати графік лише на поточний місяць.", "Обмеження", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        foreach (var row in ScheduleRowsPanel.Items.OfType<Grid>().Skip(1)) // Пропускаємо заголовок
        {
            var buttons = row.Children.OfType<Button>().ToList();
            foreach (var button in buttons)
            {
                if (Grid.GetColumn(button) == 0) continue;

                var col = Grid.GetColumn(button);
                var date = new DateTime(SelectedMonth.Year, SelectedMonth.Month, col);

                if (date.Date >= DateTime.Today)
                {
                    // Створюємо нову кнопку без можливості обрати None
                    var oldStatus = button.Tag is WorkStatus ws ? ws : WorkStatus.None;
                    var statusToUse = oldStatus == WorkStatus.None ? WorkStatus.Work : oldStatus;

                    var newButton = CreateStatusButton(date, statusToUse, allowNone: false);
                    newButton.IsEnabled = true;
                    newButton.ToolTip = button.ToolTip;
                    Grid.SetColumn(newButton, col);

                    row.Children.Remove(button);
                    row.Children.Add(newButton);
                }
                else
                {
                    button.IsEnabled = false;
                }
            }
        }

        BtnAutoDistribute.IsEnabled = false;
        BtnAutoDistribute.Background = new SolidColorBrush(Color.FromArgb(255, 150, 150, 150));
        BtnSave.Visibility = Visibility.Visible;
        BtnSave.Content = "  Надіслати  ";
        BtnSave.Click -= BtnSave_Click;
        BtnSave.Click += BtnSubmitProposal_Click;
    }

    private void MonthTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is TextBlock textBlock && textBlock.DataContext is DateTime date)
        {
            var culture = new CultureInfo("uk-UA");
            textBlock.Text = date.ToString("MMMM yyyy", culture);
        }
    }

    private void BtnSubmitProposal_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Ваш графік був надісланий для перегляду головному лікарю.", "Пропозиція надіслана", MessageBoxButton.OK, MessageBoxImage.Information);
        Close(); // або просто сховати кнопку
    }

    private void WorkScheduleWindow_Loaded(object sender, RoutedEventArgs e)
    {
        MinWidth = ActualWidth;
        MinHeight = ActualHeight;

        if (_isReadOnly)
        {
            cbMonthSelector.IsEnabled = false;
            BtnSave.Visibility = Visibility.Collapsed;
        }
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

        for (var i = 0; i < daysInMonth; i++)
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
                Padding = new Thickness(5, 0, 5, 0)
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
                var currentDate = new DateTime(SelectedMonth.Year, SelectedMonth.Month, day);
                var matchingPeriod = doctor.WorkSchedule?
                    .FirstOrDefault(p => p.From.Date <= currentDate && p.To.Date >= currentDate);

                var initialStatus = matchingPeriod?.Status ?? WorkStatus.None;
                var cell = CreateStatusButton(currentDate, initialStatus);

                if (matchingPeriod != null)
                {
                    cell.Tag = matchingPeriod.Status;

                    switch (matchingPeriod.Status)
                    {
                        case WorkStatus.Work:
                            cell.Content = "Р";
                            cell.Background = new SolidColorBrush(Color.FromRgb(128, 201, 156)); // Green
                            break;
                        case WorkStatus.Vacation:
                            cell.Content = "В";
                            cell.Background = new SolidColorBrush(Color.FromRgb(248, 168, 181)); // Pink
                            break;
                        case WorkStatus.DayOff:
                            cell.Content = "В";
                            cell.Background = new SolidColorBrush(Color.FromRgb(249, 196, 127)); // Orange
                            break;
                        case WorkStatus.None:
                        default:
                            cell.Content = "Н";
                            cell.Background = new SolidColorBrush(Color.FromRgb(206, 212, 218)); // Gray
                            break;
                    }
                }

                cell.ToolTip = culture.DateTimeFormat.GetDayName(currentDate.DayOfWeek);
                Grid.SetColumn(cell, day);
                row.Children.Add(cell);
            }

            ScheduleRowsPanel.Items.Add(row);
        }
    }

    private void RefreshScheduleGrid()
    {
        ScheduleRowsPanel.Items.Clear();
        GenerateScheduleGrid();
    }

    private Button CreateStatusButton(DateTime currentDate, WorkStatus status = WorkStatus.None, bool allowNone = true)
    {
        var button = new Button
        {
            Tag = status,
            Style = (Style)FindResource("ScheduleStatusButtonStyle")
        };

        ApplyStatusStyle(button, status);

        if (_isReadOnly || (currentDate.Date < DateTime.Today && status != WorkStatus.None))
            button.IsEnabled = false;

        button.Click += (s, e) =>
        {
            if (s is Button btn && btn.Tag is WorkStatus current)
            {
                var next = GetNextWorkStatus(current, allowNone);
                btn.Tag = next;
                ApplyStatusStyle(btn, next);
            }
        };

        return button;
    }

    private void CollectDoctorWorkSchedules()
    {
        var daysInMonth = DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month);
        var monthStart = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        for (var rowIndex = 1; rowIndex < ScheduleRowsPanel.Items.Count; rowIndex++)
            if (ScheduleRowsPanel.Items[rowIndex] is Grid rowGrid)
            {
                var nameBlock = rowGrid.Children
                    .OfType<TextBlock>()
                    .FirstOrDefault(tb => Grid.GetColumn(tb) == 0);

                if (nameBlock == null) continue;

                var doctor = _doctors.FirstOrDefault(d => d.ShortName == nameBlock.Text.Trim());
                if (doctor == null) continue;

                var statusButtons = rowGrid.Children
                    .OfType<Button>()
                    .Where(b => Grid.GetColumn(b) > 0)
                    .OrderBy(b => Grid.GetColumn(b))
                    .ToList();

                // Нові періоди для поточного місяця
                var newMonthlyPeriods = GenerateWorkScheduleForMonth(doctor, statusButtons, monthStart);

                // Фільтруємо старі періоди, що не перетинаються з поточним місяцем
                var preservedPeriods = doctor.WorkSchedule
                    .Where(p => p.To < monthStart || p.From > monthEnd)
                    .ToList();

                // Оновлюємо графік лікаря
                doctor.WorkSchedule = preservedPeriods.Concat(newMonthlyPeriods).ToList();
            }
    }

    private List<WorkPeriod> GenerateWorkScheduleForMonth(Doctor doctor, List<Button> statusButtons,
        DateTime monthStart)
    {
        var schedule = new List<WorkPeriod>();

        WorkStatus? currentStatus = null;
        DateTime? periodStart = null;

        for (var i = 0; i < statusButtons.Count; i++)
        {
            var button = statusButtons[i];
            var status = button.Tag as WorkStatus? ?? WorkStatus.None;

            if (!periodStart.HasValue)
            {
                currentStatus = status;
                periodStart = monthStart.AddDays(i).Date;
            }
            else if (status != currentStatus)
            {
                schedule.Add(new WorkPeriod
                {
                    Status = currentStatus.Value,
                    From = periodStart.Value,
                    To = monthStart.AddDays(i - 1).Date
                });

                currentStatus = status;
                periodStart = monthStart.AddDays(i).Date;
            }
        }

        if (periodStart.HasValue && currentStatus.HasValue)
            schedule.Add(new WorkPeriod
            {
                Status = currentStatus.Value,
                From = periodStart.Value,
                To = monthStart.AddDays(statusButtons.Count - 1).Date
            });

        return schedule;
    }

    private void ApplyStatusStyle(Button button, WorkStatus status)
    {
        button.Tag = status;

        switch (status)
        {
            case WorkStatus.Work:
                button.Content = "Р";
                button.Background = new SolidColorBrush(Color.FromRgb(128, 201, 156)); // Green
                break;
            case WorkStatus.Vacation:
                button.Content = "В";
                button.Background = new SolidColorBrush(Color.FromRgb(248, 168, 181)); // Pink
                break;
            case WorkStatus.DayOff:
                button.Content = "В";
                button.Background = new SolidColorBrush(Color.FromRgb(249, 196, 127)); // Orange
                break;
            default:
                button.Content = "Н";
                button.Background = new SolidColorBrush(Color.FromRgb(206, 212, 218)); // Gray
                break;
        }
    }

    private void SetAutoButtonText()
    {
        BtnAutoDistribute.Content = !_isReadOnly ? "  Авторозподіл графіку  " : "  Запропонувати графік на місяць  ";
        BtnAutoDistribute.Click += !_isReadOnly ? BtnAutoDistribute_Click_Chief : BtnAutoDistribute_Click_Doctor;
    }

    private WorkStatus GetNextWorkStatus(WorkStatus current, bool allowNone)
    {
        var statuses = allowNone
            ? Enum.GetValues(typeof(WorkStatus)).Cast<WorkStatus>().ToList()
            : Enum.GetValues(typeof(WorkStatus)).Cast<WorkStatus>().Where(s => s != WorkStatus.None).ToList();

        var currentIndex = statuses.IndexOf(current);
        var nextIndex = (currentIndex + 1) % statuses.Count;
        return statuses[nextIndex];
    }

    private void AutoDistributeSchedule()
    {
        var daysInMonth = DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month);
        var monthStart = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1);
        var isWeekend = new bool[daysInMonth];
        var dayStatuses = new Dictionary<Doctor, WorkStatus?[]>(); // For each doctor: status for each day

        for (int i = 0; i < daysInMonth; i++)
        {
            var date = monthStart.AddDays(i);
            isWeekend[i] = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        foreach (var doctor in _doctors)
        {
            var statusArray = new WorkStatus?[daysInMonth];
            // Set predefined values
            foreach (var period in doctor.WorkSchedule)
            {
                for (var d = 0; d < daysInMonth; d++)
                {
                    var date = monthStart.AddDays(d).Date;
                    if (date >= period.From.Date && date <= period.To.Date)
                        statusArray[d] = period.Status;
                }
            }
            dayStatuses[doctor] = statusArray;
        }

        if (_doctors.Count == 1)
        {
            var doctor = _doctors[0];
            var statuses = dayStatuses[doctor];
            int weekendsMarked = 0;

            for (int i = 0; i < daysInMonth; i++)
            {
                if (statuses[i] != null) continue;

                if (isWeekend[i] && weekendsMarked < 4)
                {
                    statuses[i] = WorkStatus.DayOff;
                    weekendsMarked++;
                }
                else
                {
                    statuses[i] = WorkStatus.Work;
                }
            }
        }
        else if (_doctors.Count == 2)
        {
            var d1 = _doctors[0];
            var d2 = _doctors[1];
            var s1 = dayStatuses[d1];
            var s2 = dayStatuses[d2];
            var restCount1 = 0;
            var restCount2 = 0;

            for (int i = 0; i < daysInMonth; i++)
            {
                if (s1[i] != null && s2[i] != null) continue;

                // Try alternate rest days
                if (i % 3 == 0 && s1[i] == null && restCount1 < 8)
                {
                    s1[i] = WorkStatus.DayOff;
                    restCount1++;
                    if (s2[i] == null) s2[i] = WorkStatus.Work;
                }
                else if (i % 5 == 0 && s2[i] == null && restCount2 < 8)
                {
                    s2[i] = WorkStatus.DayOff;
                    restCount2++;
                    if (s1[i] == null) s1[i] = WorkStatus.Work;
                }
                else
                {
                    if (s1[i] == null) s1[i] = WorkStatus.Work;
                    if (s2[i] == null) s2[i] = WorkStatus.Work;
                }
            }
        }
        else
        {
            // 3+ doctors
            for (int i = 0; i < daysInMonth; i++)
            {
                var available = _doctors
                    .Where(d => dayStatuses[d][i] == null)
                    .ToList();

                var required = isWeekend[i] ? 1 : 2;
                var assigned = 0;

                foreach (var doctor in available)
                {
                    if (assigned >= required) break;
                    dayStatuses[doctor][i] = WorkStatus.Work;
                    assigned++;
                }

                // Others off
                foreach (var doctor in available.Skip(assigned))
                {
                    dayStatuses[doctor][i] = WorkStatus.DayOff;
                }
            }
        }

        // Save results to WorkSchedule
        foreach (var doctor in _doctors)
        {
            var statuses = dayStatuses[doctor];
            var schedule = new List<WorkPeriod>();
            WorkStatus? currentStatus = null;
            DateTime? periodStart = null;

            for (int i = 0; i < daysInMonth; i++)
            {
                var status = statuses[i] ?? WorkStatus.None;
                var date = monthStart.AddDays(i);

                if (status == WorkStatus.None) continue;

                if (currentStatus == null)
                {
                    currentStatus = status;
                    periodStart = date;
                }
                else if (currentStatus != status)
                {
                    schedule.Add(new WorkPeriod
                    {
                        Status = currentStatus.Value,
                        From = periodStart.Value,
                        To = date.AddDays(-1)
                    });
                    currentStatus = status;
                    periodStart = date;
                }
            }

            if (currentStatus.HasValue && periodStart.HasValue)
            {
                schedule.Add(new WorkPeriod
                {
                    Status = currentStatus.Value,
                    From = periodStart.Value,
                    To = monthStart.AddDays(daysInMonth - 1)
                });
            }

            // Залишаємо старі періоди, які не стосуються цього місяця
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);
            var preserved = doctor.WorkSchedule
                .Where(p => p.To < monthStart || p.From > monthEnd)
                .ToList();

            doctor.WorkSchedule = preserved.Concat(schedule).ToList();
        }
    }

}