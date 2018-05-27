using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageService.UI.Converters
{
    public class IsConnectedColorConverter:IValueConverter
    {
        /// <summary>
        /// Converts the input boolean value to Brushes color object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Brushes object</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isConected = value != null && (bool) value;
            switch (isConected)
            {
                case true:
                    return Brushes.Transparent;
                case false:
                    return Brushes.DarkGray;
                default:
                    return Brushes.Transparent;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}