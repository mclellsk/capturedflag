using UnityEngine;

namespace CapturedFlag.Localization
{
    public class LocalizeText : MonoBehaviour
    {
        /// <summary>
        /// Key to lookup in localization table.
        /// </summary>
        public string key = "";

        private string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
        }

        /// <summary>
        /// Text changed callback.
        /// </summary>
        public System.Action OnChange;

        public void Start()
        {
            SetLocalText();
            Localization.instance.OnLanguageChange += SetLocalText;
        }

        /// <summary>
        /// Sets the text using the specified phrase key.
        /// </summary>
        private void SetLocalText()
        {
            _text = Localization.GetPhrase(key);
            if (OnChange != null)
                OnChange();
        }
    }
}
