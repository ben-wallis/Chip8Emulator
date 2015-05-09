using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Chip8Emulator.UI.Converters
{
    class ByteArrayToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var retList = new List<byte>();
            
            var array = (byte[]) value;

            for (var i = 0; i < array.GetLength(0); i++)
            {
                retList.Add(array[i]);
            }

            return retList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
