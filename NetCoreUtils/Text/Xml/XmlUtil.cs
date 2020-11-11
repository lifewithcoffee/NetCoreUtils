using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NetCoreUtils.Text.Xml
{
    public class XmlUtil<T>
    {
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "    ",  // 4 spaces for each indention
            OmitXmlDeclaration = true,
            Encoding = Encoding.UTF8
        };

        public XmlUtil(int indentSpaceNumber = 4)
        {
            if (indentSpaceNumber > 0)
            {
                settings.Indent = true; // must be true if need to apply IndentChars
                settings.IndentChars = new string(' ', indentSpaceNumber);
            }
            else
                settings.Indent = false;
        }

        public void WriteToFile(T obj, string filepath)
        {

            Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            using (Stream file = new FileStream(filepath, FileMode.Create))
            using (var writer = XmlWriter.Create(file, settings))
            {
                new XmlSerializer(typeof(T)).Serialize(writer, obj, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
            }
        }

        public T ReadFromFile(string filepath)
        {
            T retval = default(T);

            if (File.Exists(filepath))
            {
                using (StreamReader file = new StreamReader(filepath))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(T));
                    retval = (T)ser.Deserialize(file);
                }
            }

            return retval;
        }

        public string ConvertToString(T obj)
        {
            using (StringWriter sw = new StringWriter())
            using (var writer = XmlWriter.Create(sw, settings))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(writer, obj, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));
                return sw.ToString();
            }
        }

        public T ConvertFromString(string xmlString)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(xmlString);
            using (MemoryStream mem = new MemoryStream(bytes))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(mem);
            }
        }
    }
}
