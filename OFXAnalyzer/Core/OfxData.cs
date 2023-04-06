using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

[XmlRoot("OFX")]
public class OfxData
{
    [XmlElement("SIGNONMSGSRSV1")]
    public SignOnMsg SignOnMsg { get; set; }

    [XmlElement("BANKMSGSRSV1")]
    public BankData BankData { get; set; }
}