using System.Collections.Generic;
using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class BankData
{
    [XmlElement("STMTTRNRS")]
    public List<BankAccounts> BankAccounts { get; set; }
}