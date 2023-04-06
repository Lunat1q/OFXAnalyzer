using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using OFXAnalyzer.Core;

namespace OFXAnalyzer.ViewModels;

public static class ReflectionHelper
{
    public static IReadOnlyCollection<AvailableGroupRule> GetAvailableRules()
    {
        var groupingRuleType = typeof(GroupingRule);
        var assembly = Assembly.GetAssembly(groupingRuleType);
        var availableRules = assembly!.GetTypes().Where(x => groupingRuleType.IsAssignableFrom(x) && !x.IsAbstract && x.GetCustomAttribute<DisplayNameAttribute>() != null);
        return availableRules.Select(x => new AvailableGroupRule(x)).ToList();
    }
}