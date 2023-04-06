using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class BankAccounts
{
    [XmlElement("STMTRS")]
    public StatementResponse Statements { get; set; }
}