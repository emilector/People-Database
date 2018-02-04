using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Database
{
    public class ProcessData
    {
        public ProcessData()
        {
            // define parameter names

            p_parameters.Add("Family-name");
            p_parameters.Add("Given-name");
            p_parameters.Add("Age");
            p_parameters.Add("Birthdate");
            p_parameters.Add("Hair-color");
            p_parameters.Add("Eye-color");
            p_parameters.Add("Skin-color");
            p_parameters.Add("Height");
            p_parameters.Add("Weight");
            p_parameters.Add("Country");
            p_parameters.Add("State");
            p_parameters.Add("Town");
            p_parameters.Add("Street");
            p_parameters.Add("House-number");
            p_parameters.Add("Mobile-phone");
            p_parameters.Add("Home-phone");
            p_parameters.Add("Email-adress");
            p_parameters.Add("Date-time");
        }

        List<string> p_parameters = new List<string>();

        public void addObject(List<string> objectData, string objName)
        {
            XDocument doc;

            try
            {
                // add object to xml file

                doc = XDocument.Load(Directory.GetCurrentDirectory() + "/objectData/doc.xml");

                string objectName;

                if (objName == "-1")
                {
                    int objectCount = doc.Root.Elements().Count();
                    objectName = "obj" + objectCount.ToString();
                }
                else
                {
                    objectName = objName;
                }

                doc.Root.Add(new XElement(objectName));

                int counter = 0;

                foreach (string parameter in objectData)
                {
                    doc.Root.Element(objectName).Add(new XElement(p_parameters[counter], parameter));

                    counter++;
                }
            }
            catch
            {
                // create xml from scratch and add object

                doc = new XDocument(new XElement("collection", new XElement("obj0")));

                int counter = 0;

                foreach (string parameter in objectData)
                {
                    doc.Root.Element("obj0").Add(new XElement(p_parameters[counter], parameter));

                    counter++;
                }
            }

            if (Directory.Exists(Directory.GetCurrentDirectory() + "/objectData"))
                doc.Save(Directory.GetCurrentDirectory() + "/objectData/doc.xml");
            else
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/objectData");
                doc.Save(Directory.GetCurrentDirectory() + "/objectData/doc.xml");
            }
        }
    }
}
