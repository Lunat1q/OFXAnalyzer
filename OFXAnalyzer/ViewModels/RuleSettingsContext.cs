using System.Collections.Generic;
using TiqUtils.Wpf.AbstractClasses;

namespace OFXAnalyzer.ViewModels;

public class RuleSettingsContext : Notified
{

    private IReadOnlyCollection<AvailableGroupRule> _availableRules = null!;
    private AvailableGroupRule? _selectedRule;
    private AvailableGroupRule? _selectedRuleToCreateInIgnore;

    public RuleSettingsContext()
    {
        this.AvailableRules = ReflectionHelper.GetAvailableRules();
    }

    public IReadOnlyCollection<AvailableGroupRule> AvailableRules
    {
        get => this._availableRules;
        set
        {
            if (Equals(value, this._availableRules)) return;
            this._availableRules = value;
            this.OnPropertyChanged();
        }
    }

    public AvailableGroupRule? SelectedRuleToCreate
    {
        get => this._selectedRule;
        set
        {
            if (Equals(value, this._selectedRule)) return;
            this._selectedRule = value;
            this.OnPropertyChanged();
        }
    }

    public AvailableGroupRule? SelectedRuleToCreateInIgnore
    {
        get => this._selectedRuleToCreateInIgnore;
        set
        {
            if (Equals(value, this._selectedRuleToCreateInIgnore)) return;
            this._selectedRuleToCreateInIgnore = value;
            this.OnPropertyChanged();
        }
    }
}