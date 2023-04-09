using System;
using System.Drawing;
using OFXAnalyzer.Core;
using TiqUtils.Wpf.AbstractClasses;

namespace OFXAnalyzer.ViewModels;

public class TransactionDataBucketed : Notified
{
    private string _name;
    private decimal _amount;
    private TransactionGroup _group;
    private bool _isIgnored;

    public TransactionDataBucketed(TransactionData data)
    {
        this.Amount = data.Amount;
        this.Name = data.Memo;
        this.Date = DateOnly.ParseExact(data.DatePosted[..8], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
    }

    public string Name
    {
        get => this._name;
        set
        {
            if (value == this._name) return;
            this._name = value;
            this.OnPropertyChanged();
        }
    }

    public bool IsIgnored
    {
        get => this._isIgnored;
        set
        {
            if (value == this._isIgnored) return;
            this._isIgnored = value;
            this.OnPropertyChanged();
        }
    }

    public decimal Amount
    {
        get => this._amount;
        set
        {
            if (value == this._amount) return;
            this._amount = value;
            this.OnPropertyChanged();
        }
    }

    public DateOnly Date { get; set; }

    public TransactionGroup Group
    {
        get => this._group;
        set
        {
            if (Equals(value, this._group)) return;
            this._group = value;
            this.OnPropertyChanged();
        }
    }

    public void GroupRecolorUpdate()
    {
        this.OnPropertyChangedByName(nameof(this.Group));
    }
}