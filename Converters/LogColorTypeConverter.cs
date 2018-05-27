using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ImageService.UI.Converters
{
    public class LogColorTypeConverter : IValueConverter
    {
        /// <summary>
        /// Converts the input string to a Brushes object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Brushes ibject</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string logType = (string)value;
            switch (logType)
            {
                case "INFO":
                    return Brushes.LawnGreen;
                case "WARNING":
                    return Brushes.Yellow;
                case "FAIL":
                    return Brushes.IndianRed;
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