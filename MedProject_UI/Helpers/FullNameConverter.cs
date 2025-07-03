using System.Globalization;
using System.Windows.Data;
using MedProject_UI.Models;

namespace MedProject_UI.Helpers;

internal class FullNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Doctor doctor)
        {
            string firstInitial = !string.IsNullOrEmpty(doctor.FirstName) ? doctor.FirstName[0] + "." : "";
            string middleInitial = !string.IsNullOrEmpty(doctor.MiddleName) ? doctor.MiddleName[0] + "." : "";
            return $"{firstInitial}{middleInitial} {doctor.LastName}";
        }

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("ConvertBack is not implemented.");
    }
}