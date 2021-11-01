using System;
using System.Globalization;
using System.Windows.Data;
using WPFApp.Models;

namespace WPFApp
{
    public class IndentCalculator : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            int margin = 19;

            if (value != null && value[1] is Dir)
            {
                var parentWidth = (double)value[0] - margin;
                var offset = margin + ((Dir)value[1]).Level * margin;
                var width = parentWidth - offset;
                return width;
            }
            return 0;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
