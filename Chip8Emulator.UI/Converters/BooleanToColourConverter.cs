using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Chip8Emulator.UI.Converters
{
    public class BooleanToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
            {
                return new SolidColorBrush(Colors.Black);    
            }

            return new SolidColorBrush(Colors.White);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
