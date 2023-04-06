using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class SignOnMsg
{
    [XmlElement("SONRS")]
    public Sonrs Sonrs { get; set; }
}