using System;
using System.Collections.Generic;
using System.ComponentModel;
using OFXAnalyzer.ViewModels;
using TiqUtils.SettingsController;
using TiqUtils.Wpf.UIBuilders;

namespace OFXAnalyzer.Core;

public class Settings
{
    public static readonly SettingsController<Settings> Default = new("", true);

    public List<TranGroupSettings>? Groups { get; set; }

    public TranGroupSettings? TransactionIgnoreGroup { get; set; }

    public void Init()
    {
        if (this.Groups == null)
        {
            this.Groups = new List<TranGroupSettings>
            {
                new()
                {
                    Name = "Other",
                    Rules = new List<GroupingRule> { new GroupingRuleAny() }
                }
            };
        }

        if (this.TransactionIgnoreGroup == null)
        {
            this.TransactionIgnoreGroup = new TranGroupSettings
            {
                Name = "Ignored Transaction",
                Rules = new List<GroupingRule>()
            };
        }
    }
}

public class TranGroupSettings
{
    public string Name { get; set; }

    public List<GroupingRule> Rules { get; set; } = new();
    public int Order { get; set; }
}

public abstract class GroupingRule
{
    [PropertyMember]
    [SliderLimits(-100, 100, 1, 1, 1)]
    public int Priority { get; set; }

    public abstract bool IsMatch(TransactionDataBucketed tranData);
}

public class GroupingRuleAny : GroupingRule
{
    public GroupingRuleAny()
    {
        this.Priority = -100;
    }

    public override bool IsMatch(TransactionDataBucketed tranData)
    {
        return true;
    }
}

[DisplayName("Name starts with")]
public class GroupingRuleStartsWith : GroupingRule
{
    public GroupingRuleStartsWith()
    {
        this.Priority = 5;
    }

    [PropertyMember] public string StartsWith { get; set; }

    public override bool IsMatch(TransactionDataBucketed tranData)
    {
        return tranData.Name.StartsWith(this.StartsWith, StringComparison.OrdinalIgnoreCase);
    }
}

[DisplayName("Name ends with")]
public class GroupingRuleEndsWith : GroupingRule
{
    public GroupingRuleEndsWith()
    {
        this.Priority = 5;
    }

    [PropertyMember] public string EndsWith { get; set; }

    public override bool IsMatch(TransactionDataBucketed tranData)
    {
        return tranData.Name.EndsWith(this.EndsWith, StringComparison.OrdinalIgnoreCase);
    }
}

[DisplayName("Name match")]
public class GroupingRuleByName : GroupingRule
{
    public GroupingRuleByName()
    {
        this.Priority = 10;
    }

    [PropertyMember] public string NameToMatch { get; set; }

    public override bool IsMatch(TransactionDataBucketed tranData)
    {
        return tranData.Name.Equals(this.NameToMatch, StringComparison.OrdinalIgnoreCase);
    }
}