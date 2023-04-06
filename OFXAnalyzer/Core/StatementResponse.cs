using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class StatementResponse
{
    [XmlElement("BANKACCTFROM")]
    public AccountInfo AccountInfo { get; set; }

    [XmlElement("BANKTRANLIST")]
    public TransactionList Transactions { get; set; }
}