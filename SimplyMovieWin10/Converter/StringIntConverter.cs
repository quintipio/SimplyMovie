using System;
using System.Globalization;
using Windows.UI.Xaml.Data;

namespace SimplyMovieWin10.Converter
{
    public class StringIntConverter : IValueConverter
    {
        /// <summary>
        /// double to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var val = (int)value;
                return val.ToString(CultureInfo.InvariantCulture);
            }
            return null;

        }

        /// <summary>
        /// string to double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var val = value as string;
            if (val != null)
            {
                var retour = 0;
                var test = int.TryParse(val, out retour);
                if (test)
                {
                    return retour;
                }
            }
            return null;

        }
    }
}
