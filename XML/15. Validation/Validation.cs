using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

class Validation
{
    private static bool isValid = true;

    static void Main()
    {
        string xsdFilePathInvalid = "../../catalogueInvalid.xsd";
        string xmlFilePathInvalid = "../../catalogueInvalidxml.xml";
        string xsdFilePathValid = "../../catalogue.xsd";
        string xmlFilePathValid = "../../catalogue.xml";

        ValidateXmlAgainstXSD(xsdFilePathValid, xmlFilePathValid);
        ValidateXmlAgainstXSD(xsdFilePathInvalid, xmlFilePathInvalid);
    }

    private static void ValidateXmlAgainstXSD(string xsdFilePath, string xmlFilePath)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Schemas.Add(null, xsdFilePath);
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += Handler;
        XmlDocument document = new XmlDocument();
        document.Load(xmlFilePath);
        XmlReader rdr = XmlReader.Create(new StringReader(document.InnerXml), settings);

        while (rdr.Read())
        {

        }

        if (isValid)
        {
            Console.WriteLine("The document {0} validated against {1} is valid.", xmlFilePath, xsdFilePath);
        }
        else
        {
            Console.WriteLine("The document {0} validated against {1} is not valid.", xmlFilePath, xsdFilePath);
        }
    }

    private static void Handler(object sender, ValidationEventArgs e)
    {
        isValid = false;
        if (e.Severity == XmlSeverityType.Error || e.Severity ==
            XmlSeverityType.Warning)
            System.Diagnostics.Trace.WriteLine(
                String.Format("Line: {0}, Position: {1} \"{2}\"",
                    e.Exception.LineNumber, e.Exception.LinePosition,
                    e.Exception.Message));
    }
}