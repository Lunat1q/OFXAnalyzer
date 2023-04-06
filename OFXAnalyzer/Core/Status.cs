using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class Status
{
    [XmlAttribute("CODE")]
    public string Code { get; set; }

    [XmlAttribute("SEVERITY")]
    public string Severity { get; set; }
}