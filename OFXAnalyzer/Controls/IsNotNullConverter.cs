using System.Globalization;
using System.Windows.Data;
using System;

namespace OFXAnalyzer.Controls;

public class IsNotNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value != null);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
    }
}