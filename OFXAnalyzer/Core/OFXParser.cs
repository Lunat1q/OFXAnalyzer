using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OFXAnalyzer.Core
{
    public class OfxParser
    {
        public OfxData ParseFromFile(string filePath)
        {
            OfxData ret = null;
            using (var f = new FileStream(filePath, FileMode.Open))
            {
                OfxDocumentParser p = new OfxDocumentParser();
                ret = p.Import(f);
            }

            return ret;
        }
    }
}
