using Moto.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PMotoWpf.Converters
{
    public class LapTimeToBackgroundConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LapTime)
            {
                LapTime lap = value as LapTime;
                if (lap.IsPitStop)
                    return Brushes.Gray;
                else
                    return Brushes.Transparent;
            }
            return Brushes.Transparent;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
