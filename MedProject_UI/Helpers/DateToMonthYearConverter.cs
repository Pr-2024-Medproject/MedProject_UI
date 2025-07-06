using System.Globalization;
using System.Windows.Data;

namespace MedProject_UI.Helpers;

internal class DateToMonthYearConverter : IValueConverter
{
    private readonly CultureInfo _culture = new("uk-UA");

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
            return date.ToString("MMMM yyyy", _culture);
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}