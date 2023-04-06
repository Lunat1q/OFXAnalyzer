using System;
using System.ComponentModel;
using System.Reflection;
using OFXAnalyzer.Core;

namespace OFXAnalyzer.ViewModels;

public class AvailableGroupRule
{
    public AvailableGroupRule(Type classType)
    {
        this.DisplayName = classType.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? "Unknown Rule";
        this.ClassType = classType;
    }

    public string DisplayName { get; }

    public Type ClassType { get;  }

    public GroupingRule CreateInstance()
    {
        return ((GroupingRule)Activator.CreateInstance(this.ClassType)!)!;
    }
}