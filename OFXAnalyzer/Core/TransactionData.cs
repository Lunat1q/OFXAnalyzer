using System;
using System.Globalization;
using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class TransactionData
{

    [XmlElement("TRNTYPE")]
    public string TransactionType { get; set; }

    [XmlElement("DTPOSTED")]
    public string DatePosted { get; set; }

    [XmlElement("TRNAMT")]
    public decimal Amount { get; set; }

    [XmlElement("FITID")]
    public string FinancialInstitutionTransactionId { get; set; }

    [XmlElement("NAME")]
    public string Name { get; set; }

    [XmlElement("MEMO")]
    public string Memo { get; set; }
}