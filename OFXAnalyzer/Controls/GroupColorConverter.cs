using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;
using OFXAnalyzer.ViewModels;

namespace OFXAnalyzer.Controls;

public class GroupColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var group = (TransactionGroup?)value;
        if (group == null)
        {
            return Colors.Transparent;
        }
        return group.UseCustomColor ? group.GroupColor : Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException("DisplayNameFromClassConverter can only be used OneWay.");
    }
}