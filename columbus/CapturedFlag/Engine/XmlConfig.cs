using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;
using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Reads an XML file and deserializes the contents into the specified class type.
    /// </summary>
    public static class XmlConfig
    {
        public static T GetXmlConfig<T>(TextAsset asset)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(asset.text);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(doc.InnerXml))
            {
                return (T)(serializer.Deserialize(reader));
            }
        }

        public static T GetXmlConfig<T>(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(doc.InnerXml))
            {
                return (T)(serializer.Deserialize(reader));
            }
        }

        public static float GetFloat(string value)
        {
            var v = value;
            return float.Parse(v, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int GetInt(string value)
        {
            var v = value;
            return int.Parse(v);
        }

        public static string[] GetStringArray(string value)
        {
            var values = value.Split(',');
            return values;
        }

        public static float[] GetFloatArray(string value)
        {
            var values = value.Split(',');
            float[] array = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                array[i] = float.Parse(values[i], System.Globalization.CultureInfo.InvariantCulture);
            }
            return array;
        }

        public static int[] GetIntArray(string value)
        {
            var values = value.Split(',');
            int[] array = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                array[i] = int.Parse(values[i]);
            }
            return array;
        }

        public static void SaveXmlConfig<T>(this T obj, string filename, string path = "")
        {
            if (obj != null)
            {
                try
                {
                    var xs = new XmlSerializer(typeof(T));
                    using (var sw = new StringWriter())
                    {
                        using (XmlTextWriter writer = new XmlTextWriter(sw) { Formatting = Formatting.Indented })
                        {
                            xs.Serialize(writer, obj);

                            var filePath = "";
                            if (string.IsNullOrEmpty(path))
                                filePath = Application.persistentDataPath + "/" + filename;
                            else
                                filePath = path + "/" + filename;

                            using (var streamWriter = new StreamWriter(filePath, false))
                            {
                                streamWriter.Write(sw.ToString());
                                streamWriter.Flush();
                                streamWriter.Close();
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception("An error occurred", ex);
                }
            }
        }
    }
}
