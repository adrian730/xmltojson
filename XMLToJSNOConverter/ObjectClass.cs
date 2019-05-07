using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLToJSNOConverter
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class ObjectClass
    {
        [XmlElement(ElementName = "obj_name")]
        public string obj_name { get; set; }

        [XmlElement(ElementName = "field")]
        public List<FieldClass> fields { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class FieldClass
    {
        [XmlElement(ElementName = "name")]
        public string name { get; set; }

        [XmlElement(ElementName = "type")]
        public string type { get; set; }

        [XmlElement(ElementName = "value")]
        public string value { get; set; }
    }
}
