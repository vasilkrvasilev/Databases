using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

class XmlToHtml
{
    static void Main()
    {
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load("../../catalogue.xslt");
        xslt.Transform("../../catalogue.xml", "../../catalogue.html");
    }
}