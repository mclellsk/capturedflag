using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace CapturedFlag.Engine.XML
{
    public class BaseType
    {
        [XmlAttribute(AttributeName = "id", Form = XmlSchemaForm.Unqualified)]
        public int ID { get; set; }
    }
}
