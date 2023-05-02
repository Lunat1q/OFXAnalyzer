using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using OFXAnalyzer.Core;
using TiqUtils.Wpf.AbstractClasses;

namespace OFXAnalyzer.ViewModels;

public class TransactionGroup : Notified
{
    private readonly ObservableCollection<TransactionDataBucketed> _transactions = new();
    private string _groupName;
    private ObservableCollection<GroupingRule> _rules = new();
    private decimal _balance;
    private int _order;
    private Color? _groupColor;
    private bool _useCustomColor;

    public TranGroupSettings GroupInSettings { get; set; }

    public ObservableCollection<GroupingRule> Rules
    {
        get => this._rules;
        set
        {
            if (Equals(value, this._rules)) return;
            this._rules = value;
            this.OnPropertyChanged();
        }
    }

    public string GroupName
    {
        get => this._groupName;
        set
        {
            if (value == this._groupName) return;
            this._groupName = value;
            this.GroupInSettings.Name = value;
            this.OnPropertyChanged();
        }
    }

    public int Order
    {
        get => this._order;
        set
        {
            if (value == this._order) return;
            this._order = value;
            this.GroupInSettings.Order = value;
            this.OnPropertyChanged();
        }
    }

    public bool UseCustomColor
    {
        get => this._useCustomColor;
        set
        {
            if (value == this._useCustomColor) return;
            this._useCustomColor = value;
            this.GroupInSettings.UseCustomColor = value;
            if (this._useCustomColor && this.GroupColor == null)
            {
                this.GroupColor = Color.FromRgb(255, 89, 89);
            }
            this.OnPropertyChanged();
        }
    }

    public Color? GroupColor
    {
        get => this._groupColor ?? Colors.Transparent;
        set
        {
            this._groupColor = value;
            this.GroupInSettings.GroupColor = value;
            this.OnPropertyChanged();
        }
    }

    public IEnumerable<TransactionDataBucketed> Transactions => this._transactions;

    public void AddTransaction(TransactionDataBucketed transaction)
    {
        this._transactions.Add(transaction);
        this.Balance += transaction.Amount;
    }

    public void ClearTransactions()
    {
        this._transactions.Clear();
    }

    public decimal Balance
    {
        get => this._balance;
        set
        {
            if (value == this._balance) return;
            this._balance = value;
            this.OnPropertyChanged();
        }
    }

    public override string ToString()
    {
        return this.GroupName;
    }

    public void TriggerGroupRecolorUpdate()
    {
        foreach (var transaction in this.Transactions)
        {
            transaction.GroupRecolorUpdate();
        }
    }
}