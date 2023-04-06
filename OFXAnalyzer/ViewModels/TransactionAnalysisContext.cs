using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;
using System.Windows.Input;
using OFXAnalyzer.Core;
using TiqUtils.Wpf.AbstractClasses;

namespace OFXAnalyzer.ViewModels;

public class TransactionAnalysisContext : Notified
{
    private readonly Settings _settings = null!;
    private decimal _balanceAmount;
    private decimal _expensesAmount;
    private ObservableCollection<TransactionGroup> _groups = null!;
    private decimal _incomeAmount;
    private TransactionGroup _selectedGroup = null!;
    private TransactionGroup _transactionIgnoreGroup = null!;
    private ObservableCollection<TransactionDataBucketed> _transactions = null!;

    public TransactionAnalysisContext(Settings settings) : this()
    {
        this._settings = settings;
    }

    public TransactionAnalysisContext()
    {
        this.RuleSettings = new RuleSettingsContext();
        this.RemoveRuleFromGroupCommand = new ActionCommand(this.RemoveRuleFromGroup);
    }

    public ICommand RemoveRuleFromGroupCommand { get; set; }

    private void RemoveRuleFromGroup(object item)
    {
        var group = (GroupingRule)item;
        this.SelectedGroupForEdit.Rules.Remove(group);
        this.SelectedGroupForEdit.GroupInSettings.Rules.Remove(group);
    }

    public ObservableCollection<TransactionDataBucketed>? Transactions
    {
        get => this._transactions;
        set
        {
            if (Equals(value, this._transactions))
            {
                return;
            }

            this._transactions = value!;
            this.OnPropertyChanged();
        }
    }

    public RuleSettingsContext RuleSettings { get; set; }

    public ObservableCollection<TransactionGroup> Groups
    {
        get => this._groups;
        set
        {
            if (Equals(value, this._groups))
            {
                return;
            }

            this._groups = value;
            this.OnPropertyChanged();
        }
    }

    public TransactionGroup SelectedGroupForEdit
    {
        get => this._selectedGroup;
        set
        {
            if (Equals(value, this._selectedGroup))
            {
                return;
            }

            this._selectedGroup = value;
            this.OnPropertyChanged();
        }
    }

    public decimal BalanceAmount
    {
        get => this._balanceAmount;
        set
        {
            if (value == this._balanceAmount)
            {
                return;
            }

            this._balanceAmount = value;
            this.OnPropertyChanged();
        }
    }

    public decimal IncomeAmount
    {
        get => this._incomeAmount;
        set
        {
            if (value == this._incomeAmount)
            {
                return;
            }

            this._incomeAmount = value;
            this.OnPropertyChanged();
        }
    }

    public decimal ExpensesAmount
    {
        get => this._expensesAmount;
        set
        {
            if (value == this._expensesAmount)
            {
                return;
            }

            this._expensesAmount = value;
            this.OnPropertyChanged();
        }
    }

    public TransactionGroup TransactionIgnoreGroup
    {
        get => this._transactionIgnoreGroup;
        set
        {
            if (Equals(value, this._transactionIgnoreGroup))
            {
                return;
            }

            this._transactionIgnoreGroup = value;
            this.OnPropertyChanged();
        }
    }

    public void FillTransactions(IEnumerable<TransactionData> transactions)
    {
        this.Transactions = new ObservableCollection<TransactionDataBucketed>(transactions.Select(x => new TransactionDataBucketed(x)));
    }

    public void CalculateBalance()
    {
        var notIgnoredTransactions = this.Transactions.Where(x => !this.IsIgnored(x)).ToList();
        this.BalanceAmount = notIgnoredTransactions.Sum(x => x.Amount);
        this.IncomeAmount = notIgnoredTransactions.Where(x => x.Amount > 0).Sum(x => x.Amount);
        this.ExpensesAmount = notIgnoredTransactions.Where(x => x.Amount < 0).Sum(x => x.Amount);
    }

    public void RecalculateGrouping()
    {
        if (this.Transactions == null)
        {
            return;
        }

        this.CalculateBalance();

        foreach (var group in this.Groups)
        {
            group.ClearTransactions();
            group.Balance = 0;
        }



        foreach (var transaction in this.Transactions)
        {
            if (this.IsIgnored(transaction))
            {
                transaction.Group = this.TransactionIgnoreGroup;
                transaction.IsIgnored = true;
                this.TransactionIgnoreGroup.AddTransaction(transaction);
            }
            else
            {
                var maxPriorityGroup = SelectWhenMax(this.Groups, x => MaxIfAny(x.Rules.Where(y => y.IsMatch(transaction)), i => i.Priority, int.MinValue));
                transaction.Group = maxPriorityGroup;
                maxPriorityGroup.AddTransaction(transaction);
            }
        }
    }

    public void Init()
    {
        this.Groups = new ObservableCollection<TransactionGroup>(
            this._settings.Groups!.Select(x => new TransactionGroup
            {
                GroupInSettings = x,
                GroupName = x.Name,
                Order = x.Order,
                Rules = new ObservableCollection<GroupingRule>(x.Rules)
            })
        );

        this.TransactionIgnoreGroup = new TransactionGroup
        {
            GroupInSettings = this._settings.TransactionIgnoreGroup!,
            GroupName = this._settings.TransactionIgnoreGroup!.Name,
            Rules = new ObservableCollection<GroupingRule>(this._settings.TransactionIgnoreGroup.Rules)
        };
    }

    public TransactionGroup AddNewGroup(TranGroupSettings newGroup)
    {
        var group = new TransactionGroup
        {
            GroupInSettings = newGroup,
            GroupName = newGroup.Name,
            Rules = new ObservableCollection<GroupingRule>(newGroup.Rules)
        };
        this.Groups.Add(group);
        return group;
    }

    private static T SelectWhenMax<T>(IEnumerable<T> items, Func<T, IComparable> predicate)
    {
        IComparable curMax = null;
        T curItem = default;
        foreach (var item in items)
        {
            var cur = predicate(item);
            if (curMax == null || curMax.CompareTo(cur) < 0)
            {
                curMax = cur;
                curItem = item;
            }
        }

        return curItem;
    }

    private static IComparable MaxIfAny<T>(IEnumerable<T> items, Func<T, IComparable> predicate, IComparable defVal)
    {
        IComparable curMax = null;
        foreach (var item in items)
        {
            var cur = predicate(item);
            if (curMax == null || curMax.CompareTo(cur) < 0)
            {
                curMax = cur;
            }
        }

        return curMax ?? defVal;
    }

    private bool IsIgnored(TransactionDataBucketed transaction)
    {
        return this.TransactionIgnoreGroup.Rules.Any(x => x.IsMatch(transaction));
    }

    public static void CreateNewRule(TransactionGroup contextSelectedGroupForEdit, AvailableGroupRule selectedNewRule)
    {
        var newRuleObject = selectedNewRule.CreateInstance();
        contextSelectedGroupForEdit.Rules.Add(newRuleObject);
        contextSelectedGroupForEdit.GroupInSettings.Rules.Add(newRuleObject);
    }
}