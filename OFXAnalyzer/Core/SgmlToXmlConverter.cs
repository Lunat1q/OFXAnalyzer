using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Xml;
using TiqUtils.TypeSpecific;

namespace OFXAnalyzer.Core;

internal class SgmlToXmlConverter
{
    public string Convert(string sgml)
    {
        // Replace SGML comments with XML comments
        sgml = Regex.Replace(sgml, "<!--(.*?)-->", match => $"<!--{match.Groups[1].Value}-->");

        // Replace SGML shorttags with XML empty elements
        sgml = Regex.Replace(sgml, "<([A-Za-z0-9_:-]+)[^>]*?/>", match => $"<{match.Groups[1].Value}/>");



        using (var stringWriter = new StringWriter())
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
                NewLineHandling = NewLineHandling.Replace
            };

            using (var writer = XmlWriter.Create(stringWriter, settings))
            {
                using (var reader = new StringReader(sgml))
                {
                    while (reader.ReadLine() is { } line)
                    {
                        var tagBracketsOpen = line.IndexOf("<", StringComparison.Ordinal);
                        if (tagBracketsOpen < 0)
                        {
                            continue;
                        }

                        var isClosingTag = line[tagBracketsOpen + 1] == '/';
                        var tagBracketsClose = line.IndexOf(">", StringComparison.Ordinal);
                        var offset = isClosingTag ? 2 : 1;
                        var tagName = line[(tagBracketsOpen + offset)..tagBracketsClose];

                        var rowValue = line[(tagBracketsClose + 1)..];

                        if (!isClosingTag)
                        {
                            WriteTagData(writer, tagName, rowValue, !rowValue.Empty());
                        }
                        else
                        {
                            writer.WriteEndElement();
                        }
                    }
                }
            }

            stringWriter.Flush();
            return stringWriter.ToString();
        }
    }

    private static void WriteTagData(XmlWriter writer, string tagName, string rowValue, bool withClosure = false)
    {
        writer.WriteStartElement(tagName);
        if (!rowValue.Empty())
        {
            writer.WriteString(rowValue);
        }

        if (withClosure)
        {
            writer.WriteEndElement();
        }
    }
}