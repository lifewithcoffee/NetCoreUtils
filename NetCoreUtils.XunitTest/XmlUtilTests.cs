using NetCoreUtils.Text.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace NetCoreUtils.XunitTest
{
    public class MyXmlUtilObject
    {
        [XmlAttribute]
        public string Name { get; set; }

        public int Length { get; set; }

        [XmlIgnore]
        public int Ignored { get; set; }

        public List<ChildObject> ChildObjects { get; set; } = new List<ChildObject>();

        [XmlElement("AnotherList")]
        public List<ChildObject2> ChildObject2List { get; set; } = new List<ChildObject2>();
    }


    [XmlRoot(nameof(MyXmlUtilObject))]
    public class MyXmlUtilObjectWithNoise : MyXmlUtilObject
    {
        [XmlAttribute]
        public int NoisyProp { get; set; }
    }

    public class ChildObject
    {
        [XmlAttribute]
        public string Prop { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class ChildObject2
    {
        [XmlAttribute]
        public int Value { get; set; }
    }

    public class XmlUtilTests
    {
        MyXmlUtilObject obj = new MyXmlUtilObject
        {
            Name = "test name 123",
            Length = 123,
            Ignored = 56789,
            ChildObjects = new List<ChildObject>
            {
                new ChildObject { Prop = "中文测试，某属性", Value = "value 1" },
                new ChildObject { Prop = "prop abc", Value = "中文测试，某值" },
                new ChildObject { Prop = "prop xyz 1234", Value = "prop xyz 1234" },
            },
            ChildObject2List = new List<ChildObject2>
            {
                new ChildObject2 { Value = 11 },
                new ChildObject2 { Value = 22 },
                new ChildObject2 { Value = 333 },
                new ChildObject2 { Value = 4444 },
            }
        };

        [Fact]
        public void To_from_file()
        {
            new XmlUtil<MyXmlUtilObject>().WriteToFile(obj, @"d:\xml_test_output_file.xml");

            var de = new XmlUtil<MyXmlUtilObject>().ReadFromFile(@"d:\xml_test_output_file.xml");
            Assert.Equal(de.Name, obj.Name);
            Assert.Equal(de.Length, obj.Length);

            // because "Ignored" is an ignored element, which won't be serialized to the file;
            // thus, when deserialize from the file it won't be signed the number it was
            Assert.NotEqual(de.Ignored, obj.Ignored);
            Assert.Equal(0, de.Ignored);    

            Assert.Equal(3, de.ChildObjects.Count);
            Assert.Equal(de.ChildObjects[0].Prop, obj.ChildObjects[0].Prop);

            Assert.Equal(4, de.ChildObject2List.Count);
            Assert.Equal(de.ChildObject2List[0].Value, obj.ChildObject2List[0].Value);
        } 

        [Fact]
        public void To_from_string()
        {
            string xmlstr = "";

            // write to file: only for manual check output
            using (StreamWriter file = new StreamWriter(@"d:\xml_text_output_string.xml"))
            {
                xmlstr = new XmlUtil<MyXmlUtilObject>(8).ConvertToString(obj);
                file.Write(xmlstr); 
            }

            var de = new XmlUtil<MyXmlUtilObject>().ConvertFromString(xmlstr);
            Assert.Equal(de.Name, obj.Name);
            Assert.Equal(de.Length, obj.Length);

            // because "Ignored" is an ignored element, which won't be serialized to the file;
            // thus, when deserialize from the file it won't be signed the number it was
            Assert.NotEqual(de.Ignored, obj.Ignored);
            Assert.Equal(0, de.Ignored);

            Assert.Equal(3, de.ChildObjects.Count);
            Assert.Equal(de.ChildObjects[0].Prop, obj.ChildObjects[0].Prop);

            Assert.Equal(4, de.ChildObject2List.Count);
            Assert.Equal(de.ChildObject2List[0].Value, obj.ChildObject2List[0].Value);
        }

        [Fact]
        public void From_noisy_string()
        {
            string xmlstr = @"
            <MyXmlUtilObject Name=""test name 123"" NoisyProp=""1342"">
                <Length>123</Length>
                <ChildObjects>
                    <ChildObject Prop=""中文测试，某属性"">value 1</ChildObject>
                    <ChildObject Prop=""prop abc"">中文测试，某值</ChildObject>
                    <ChildObject Prop=""prop xyz 1234"">prop xyz 1234</ChildObject>
                    <NoiseElem2 Value=""1234"" />
                </ChildObjects>

                <NoiseElem Value=""1234"" />
            </MyXmlUtilObject>";

            var de = new XmlUtil<MyXmlUtilObjectWithNoise>().ConvertFromString(xmlstr);

            Assert.Equal(de.Name, obj.Name);
            Assert.Equal(de.Length, obj.Length);

            Assert.Equal(1342, de.NoisyProp);

            // because "Ignored" is an ignored element, which won't be serialized to the file;
            // thus, when deserialize from the file it won't be signed the number it was
            Assert.NotEqual(de.Ignored, obj.Ignored);
            Assert.Equal(0, de.Ignored);

            Assert.Equal(3, de.ChildObjects.Count);
            Assert.Equal(de.ChildObjects[0].Prop, obj.ChildObjects[0].Prop);

            Assert.Empty(de.ChildObject2List); // the relevant element <AnotherList> removed from the xml string
        }
    }
}
