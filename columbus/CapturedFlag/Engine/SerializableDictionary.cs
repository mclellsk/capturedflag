using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

//http://stackoverflow.com/questions/3671259/how-to-xml-serialize-a-dictionary
//http://weblogs.asp.net/pwelter34/444961

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Represents an XML serializable collection of keys and values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [Serializable]
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            reader.ReadStartElement("dictionary");

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");

                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();

                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            writer.WriteStartElement("dictionary");

            foreach (TKey key in this.Keys)
            {
                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
        #endregion

        public void LoadDictionary(string fileName, string path = null)
        {
            //Read data from file
            string data = DataSerializer.GetData(fileName, path);
            //Decrypt
            string decryptedText = DataSerializer.Decrypt(data);
            //Convert to String from Binary
            string xmlString = DataSerializer.ConvertToString(Convert.FromBase64String(decryptedText));

            //Deserialize data
            //Debug.Log (xmlString);
            XmlDeserializeDictionaryData(xmlString);
        }

        private void XmlDeserializeDictionaryData(string xmlString)
        {
            //Deserialize XML for data
            XmlReader reader = XmlReader.Create(new StringReader(xmlString));
            this.ReadXml(reader);
            reader.Close();
        }

        private string XmlSerializeDictionaryData()
        {
            //Write to XML String
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sw);

            if (xw != null)
            {
                this.WriteXml(xw);
                xw.Flush();
                xw.Close();
            }

            sw.Close();

            return sw.ToString();
        }

        public void SaveDictionary(string fileName, string path = null)
        {
            //Serialize data
            string xmlString = XmlSerializeDictionaryData();
            //Convert to Binary from String
            byte[] bytes = DataSerializer.ConvertToBinary(xmlString);
            //Encrypt
            string encryption = DataSerializer.Encrypt(Convert.ToBase64String(bytes));

            //Write data to file
            DataSerializer.StoreData(encryption, fileName, path);
        }
    }
}