using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using CapturedFlag.Engine;

namespace CapturedFlag.Engine.XML
{
    [XmlRoot(ElementName = "controls")]
    public class Controls
    {
        public Controls()
        {
            Schemes = new List<Scheme>();
        }

        [XmlElement(ElementName = "scheme", Form = XmlSchemaForm.Unqualified)]
        public List<Scheme> Schemes { get; set; }
    }

    [XmlRoot(ElementName = "scheme")]
    public class Scheme
    {
        public Scheme()
        {
            Actions = new List<Action>();
        }

        [XmlElement(ElementName = "action", Form = XmlSchemaForm.Unqualified)]
        public List<Action> Actions { get; set; }

        [XmlAttribute(AttributeName = "name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "action")]
    public class Action
    {
        public Action() { }

        [XmlAttribute(AttributeName = "name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        [XmlElement(ElementName = "key", Form = XmlSchemaForm.Unqualified)]
        public List<Key> Keys { get; set; }

        [XmlElement(ElementName = "button", Form = XmlSchemaForm.Unqualified)]
        public List<Button> Buttons { get; set; }
    }

    [XmlRoot(ElementName = "key")]
    public class Key
    {
        [XmlAttribute(AttributeName = "value", Form = XmlSchemaForm.Unqualified)]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "button")]
    public class Button
    {
        [XmlAttribute(AttributeName = "value", Form = XmlSchemaForm.Unqualified)]
        public int ID { get; set; }
    }
}
