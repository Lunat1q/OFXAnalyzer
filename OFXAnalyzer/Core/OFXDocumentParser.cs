using System;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace OFXAnalyzer.Core
{
    public class OfxDocumentParser
    {
        public OfxData Import(FileStream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.Default))
            {
                return this.Import(reader.ReadToEnd());
            }
        }

        public OfxData Import(string ofx)
        {
            return this.ParseOfxDocument(ofx);
        }

        private OfxData ParseOfxDocument(string ofxString)
        {
            //If OFX file in SGML format, convert to XML
            if (!this.IsXmlVersion(ofxString))
            {
                ofxString = this.SgmlToXml(ofxString);
            }

            return this.Parse(ofxString);
        }

        private OfxData Parse(string ofxString)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(OfxData));

            // Deserialize the XML string into an OfxData object
            OfxData ofxData;
            using (StringReader reader = new(ofxString))
            {
                ofxData = (OfxData)serializer.Deserialize(reader)!;
            }

            return ofxData;
        }


        /// <summary>
        /// Check if OFX file is in SGML or XML format
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsXmlVersion(string file)
        {
            return file.IndexOf("OFXHEADER:100") == -1;
        }

        /// <summary>
        /// Converts SGML to XML
        /// </summary>
        /// <param name="file">OFX File (SGML Format)</param>
        /// <returns>OFX File in XML format</returns>
        public string SgmlToXml(string file)
        {
            var payloadData = this.ParseHeader(file);

            var conv = new SgmlToXmlConverter();

            var ret = conv.Convert(payloadData);

            return ret;
        }

        /// <summary>
        /// Checks that the file is supported by checking the header. Removes the header.
        /// </summary>
        /// <param name="file">OFX file</param>
        /// <returns>File, without the header</returns>
        private string ParseHeader(string file)
        {
            //Select header of file and split into array
            //End of header worked out by finding first instance of '<'
            //Array split based of new line & carrige return
            var header = file.Substring(0, file.IndexOf('<'))
               .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            //Check that no errors in header
            this.CheckHeader(header);

            //Remove header
            return file.Substring(file.IndexOf('<')).Trim();
        }

        /// <summary>
        /// Checks that all the elements in the header are supported
        /// </summary>
        /// <param name="header">Header of OFX file in array</param>
        private void CheckHeader(string[] header)
        {
            if (header[0] == "OFXHEADER:100DATA:OFXSGMLVERSION:102SECURITY:NONEENCODING:USASCIICHARSET:1252COMPRESSION:NONEOLDFILEUID:NONENEWFILEUID:NONE")//non delimited header
                return;
            if (header[0] != "OFXHEADER:100")
                throw new OfxParseException("Incorrect header format");

            if (header[1] != "DATA:OFXSGML")
                throw new OfxParseException("Data type unsupported: " + header[1] + ". OFXSGML required");

            if (header[2] != "VERSION:102")
                throw new OfxParseException("OFX version unsupported. " + header[2]);

            if (header[3] != "SECURITY:NONE")
                throw new OfxParseException("OFX security unsupported");

            if (header[4] != "ENCODING:USASCII")
                throw new OfxParseException("ASCII Format unsupported:" + header[4]);

            if (header[5] != "CHARSET:1252")
                throw new OfxParseException("Charecter set unsupported:" + header[5]);

            if (header[6] != "COMPRESSION:NONE")
                throw new OfxParseException("Compression unsupported");

            if (header[7] != "OLDFILEUID:NONE")
                throw new OfxParseException("OLDFILEUID incorrect");
        }
    }
}