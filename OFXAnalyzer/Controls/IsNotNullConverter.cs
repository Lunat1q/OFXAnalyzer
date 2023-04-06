using System.Globalization;
using System.Windows.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace OFXAnalyzer.Controls;

public class IsNotNullConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value != null);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException("IsNotNullConverter can only be used OneWay.");
    }
}

public class DisplayNameFromClassConverter : IValueConverter
{
    private static Dictionary<string, string> displayNameByClassName = new Dictionary<string, string>();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var type = value.GetType();

        var typeName = type.Name;

        if (!displayNameByClassName.TryGetValue(typeName, out var result))
        {
            var nameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            result = nameAttribute?.DisplayName ?? typeName;
            displayNameByClassName.TryAdd(typeName, result);
        }

        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException("DisplayNameFromClassConverter can only be used OneWay.");
    }
}