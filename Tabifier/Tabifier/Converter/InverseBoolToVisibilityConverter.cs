using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Tabifier.Converter
{
    public class InverseBoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visible = Visibility.Hidden;
            bool check = !(bool)value;
            if (check)
                visible = Visibility.Visible;
            else visible = Visibility.Hidden;

            return visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool convertedValue = false;
            try
            {
                convertedValue = !(bool)value;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return convertedValue;
        }



    }
}
