using System.Collections.Generic;
using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class TransactionList
{
    [XmlElement("DTSTART")]
    public string DtStart { get; set; }

    [XmlElement("DTEND")]
    public string DtEnd { get; set; }

    [XmlElement("STMTTRN")]
    public List<TransactionData> Transactions { get; set; }
}