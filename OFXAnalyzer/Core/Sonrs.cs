using System.Xml.Serialization;

namespace OFXAnalyzer.Core;

public class Sonrs
{
    [XmlElement("STATUS")]
    public Status Status { get; set; }
}