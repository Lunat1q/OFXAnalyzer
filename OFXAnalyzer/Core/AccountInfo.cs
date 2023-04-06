using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class AccountInfo
{
    [XmlElement("BANKID")]
    public string BankId { get; set; }

    [XmlElement("ACCTID")]
    public string Id { get; set; }

    [XmlElement("ACCTTYPE")]
    public string Type { get; set; }
}