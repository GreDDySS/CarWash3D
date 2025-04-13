using System;
using System.Globalization;
using System.Windows.Data;

namespace CarWash3D.Converters
{
    public class TimeSelectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || values[0] == null || values[1] == null)
                return false;

            string buttonContent = values[0].ToString();
            string selectedTime = values[1].ToString();

            return buttonContent == selectedTime;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}