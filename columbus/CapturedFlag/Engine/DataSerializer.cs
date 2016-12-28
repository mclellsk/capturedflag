using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Runtime.InteropServices;

namespace CapturedFlag.Engine
{
    public static class DataSerializer
    {
        static readonly string PasswordHash = "f222ba52d34a24ab0752d0f70e412c6a";
        static readonly string SaltKey = "4246d5050acb924e7c6191b6c673b4dc";

        const long ERROR_SHARING_VIOLATION = 0x20;
        const long ERROR_LOCK_VIOLATION = 0x21;

        public static string XmlSerializeData(object obj)
        {
            var type = obj.GetType();
            var formatter = new XmlSerializer(type);

            //Write to XML String
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sw);

            if (xw != null)
            {
                formatter.Serialize(xw, obj);
                xw.Flush();
                xw.Close();
            }

            sw.Close();

            return sw.ToString();
        }

        public static string BinarySerializeData(object obj)
        {
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            MemoryStream mStream = new MemoryStream();
            formatter.Serialize(mStream, obj);
            byte[] bytes = mStream.ToArray();

            string text = System.Convert.ToBase64String(bytes);
            return text;
        }

        public static object BinaryDeserializeData(string bytesAsString)
        {
            byte[] bytes = System.Convert.FromBase64String(bytesAsString);

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();//BinaryFormatter();

            MemoryStream mStream = new MemoryStream(bytes);
            object obj = formatter.Deserialize(mStream);

            return obj;
        }

        public static object XmlDeserializeData(Type type, string xmlString)
        {
            //Deserialize XML for data
            XmlSerializer serializer = new XmlSerializer(type);
            XmlReader reader = XmlReader.Create(new StringReader(xmlString));
            reader.MoveToContent();
            var obj = serializer.Deserialize(reader);
            reader.Close();

            return obj;
        }

        public static string GetPersistentFile(string fileName)
        {
            string filePath = "";

            if (fileName != null)
                filePath = Application.persistentDataPath + "/" + fileName;

            return filePath;
        }

        public static string GetData(string fileName, string path = null)
        {
            string filePath = "";

            if (path == null)
                filePath = GetPersistentFile(fileName);
            else
                filePath = path + "/" + fileName;

            string text = "";

            try
            {
                StreamReader stream = new StreamReader(filePath);

                if (stream != null)
                {
                    text = stream.ReadToEnd();
                    stream.Close();
                }
            }
            catch
            {
                LogTool.LogWarning("File not found.");
            }

            return text;
        }

        //type obj = (type)Load(type, filename);
        public static object LoadXml(Type type, string fileName, string path = null)
        {
            //Read data from file
            string data = GetData(fileName, path);
            //Decrypt
            string decryptedText = Decrypt(data);
            //Convert to String from Binary
            string xmlString = ConvertToString(Convert.FromBase64String(decryptedText));

            //Deserialize data
            return XmlDeserializeData(type, xmlString);
        }

        //type obj = (type)Load(type, filename);
        public static object LoadBinary(string fileName, string path = null)
        {
            //Read data from file
            string data = GetData(fileName, path);
            //Decrypt
            string decryptedText = Decrypt(data);

            //Deserialize data
            return BinaryDeserializeData(decryptedText);
        }

        public static void SaveXml(object obj, string fileName, string path = null)
        {
            //Serialize data
            string xmlString = XmlSerializeData(obj);
            //Convert to Binary from String
            byte[] bytes = ConvertToBinary(xmlString);
            //Encrypt
            string encryption = Encrypt(Convert.ToBase64String(bytes));

            //Write data to file
            StoreData(encryption, fileName);
        }

        public static void SaveBinary(object obj, string fileName, string path = null)
        {
            //Serialize data
            string text = BinarySerializeData(obj);
            //Encrypt
            string encryption = Encrypt(text);

            //Write data to file
            StoreData(encryption, fileName);
        }

        //Saves files with backups, incase an error happens during file writing
        public static void StoreData(string dataString, string fileName, string path = "")
        {
            string filePath = "";

            if (String.IsNullOrEmpty(path))
                filePath = Application.persistentDataPath + "/" + fileName;
            else
                filePath = path + "/" + fileName;

            try
            {
                if (File.Exists(filePath + "_backup"))
                {
                    File.Delete(filePath + "_backup");
                    LogTool.LogDebug("Deleted previous backup...");
                }

                if (File.Exists(filePath))
                {
                    File.Move(filePath, filePath + "_backup");
                    LogTool.LogDebug("Previous save backed up...");
                }

                StreamWriter stream = new StreamWriter(filePath, false);
                stream.Write(dataString);
                stream.Close();

                LogTool.LogDebug("Save [" + filePath + "] successful...");
            }
            catch (IOException e)
            {
                LogTool.LogError(e.ToString());
            }
        }

        /// <summary>
        /// http://stackoverflow.com/questions/10982104/wait-until-file-is-completely-written
        /// Used to check if a file is being written to, useful for displaying whether the game is still saving or not.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string file)
        {
            //check that problem is not in destination file
            if (File.Exists(file) == true)
            {
                FileStream stream = null;
                try
                {
                    stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (Exception ex2)
                {
                    //_log.WriteLog(ex2, "Error in checking whether file is locked " + file);
                    int errorCode = Marshal.GetHRForException(ex2) & ((1 << 16) - 1);
                    if ((ex2 is IOException) && (errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION))
                    {
                        return true;
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            return false;
        }

        public static byte[] ConvertToBinary(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string ConvertToString(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        /*
         * Data -> XMLString -> Binary -> Encrypt 
         */
        //http://social.msdn.microsoft.com/Forums/vstudio/en-US/d6a2836a-d587-4068-8630-94f4fb2a2aeb/encrypt-and-decrypt-a-string-in-c

        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            Rfc2898DeriveBytes rfcBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey));

            byte[] keyBytes = rfcBytes.GetBytes(32);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, rfcBytes.GetBytes(16));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            Rfc2898DeriveBytes rfcBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey));

            byte[] keyBytes = rfcBytes.GetBytes(32);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, rfcBytes.GetBytes(16));

            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}

