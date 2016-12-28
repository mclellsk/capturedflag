using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System;

namespace CapturedFlag.Localization
{
    /// <summary>
    /// This component helps manage localization of all text and images.
    /// </summary>
    public class Localization : MonoBehaviour
    {
        private const string ERROR_LANGUAGE_MISSING = "[ERROR_MISSING_LANG]";
        private const string ERROR_PHRASE_MISSING = "[ERROR_MISSING_PHRASE]";

        /// <summary>
        /// Selected language tag.
        /// </summary>
        public static string language = "";

        /// <summary>
        /// Language key and related xml phrase file.
        /// </summary>
        [Serializable]
        public class LanguageFiles
        {
            public string language;
            public TextAsset xml;
        }

        /// <summary>
        /// Phrase key and localized phrase.
        /// </summary>
        public class LocalPhrase
        {
            public string key = "";
            public string phrase = "";

            public LocalPhrase(string key, string phrase)
            {
                this.key = key;
                this.phrase = phrase;
            }
        }

        /// <summary>
        /// List of language files.
        /// </summary>
        public List<LanguageFiles> files = new List<LanguageFiles>();

        /// <summary>
        /// Dictionary containing the language keys and the associated phrase lookup list.
        /// </summary>
        public static Dictionary<string, List<LocalPhrase>> languages = new Dictionary<string, List<LocalPhrase>>();

        /// <summary>
        /// Language changed callback.
        /// </summary>
        public event System.Action OnLanguageChange;

        /// <summary>
        /// Localization management instance. Only one active at a time.
        /// </summary>
        public static Localization instance;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                DestroyObject(this.gameObject);
            }
        }

        /// <summary>
        /// Must be called before any languages are actually used. Sets the language phrase lookup.
        /// </summary>
        /// <param name="lang">Language to use.</param>
        public void SetLocalization(string lang)
        {
            var file = files.Find(p => p.language == lang);
            if (file != null)
            {
                if (!languages.ContainsKey(lang))
                {
                    List<LocalPhrase> phrases = new List<LocalPhrase>();

                    using (XmlReader reader = XmlReader.Create(new StringReader(file.xml.text)))
                    {
                        while (reader.Read() && !reader.EOF)
                        {
                            if (reader.Name == "phrase" && reader.IsStartElement())
                            {
                                reader.MoveToAttribute("key");
                                string key = reader.Value;
                                reader.ReadToFollowing("content");
                                string phrase = reader.ReadElementContentAsString();
                                phrases.Add(new LocalPhrase(key, phrase));
                            }
                        }
                    }

                    languages.Add(lang, phrases);
                }

                language = lang;

                if (OnLanguageChange != null)
                {
                    OnLanguageChange();
                }
            }
        }

        /// <summary>
        /// Returns a phrase from the phrase lookup for the selected language based on the phrase key.
        /// </summary>
        /// <param name="key">Phrase key.</param>
        /// <param name="obj">StringBuilder parameters.</param>
        /// <returns>Phrase with parameters used to build string.</returns>
        public static string GetPhrase(string key, params object[] obj)
        {
            if (languages.ContainsKey(language))
            {
                var lp = languages[language].Find(p => p.key == key);
                if (lp != null)
                {
                    StringBuilder sb = new StringBuilder();

                    //Check if parameters are empty even if format specifier is present in string
                    var count = (lp.phrase.Split('{')).Length - 1;

                    if (count > 0 && obj.Length < count)
                    {
                        List<object> o = new List<object>();
                        var d = count - obj.Length;
                        for (int i = 0; i < d; i++)
                        {
                            o.Add(string.Empty);
                        }

                        object[] objects = new object[obj.Length + o.Count];
                        for (int i = 0; i < obj.Length; i++)
                        {
                            objects[i] = obj[i];
                        }
                        for (int i = objects.Length; i < o.Count; i++)
                        {
                            objects[i] = o[i];
                        }

                        sb.AppendFormat(lp.phrase, objects);
                    }
                    else
                        sb.AppendFormat(lp.phrase, obj);

                    return sb.ToString();
                }
                else
                    return ERROR_PHRASE_MISSING;
            }
            else
                return ERROR_LANGUAGE_MISSING;
        }
    }
}