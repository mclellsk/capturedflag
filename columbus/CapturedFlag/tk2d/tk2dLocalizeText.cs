using UnityEngine;
using CapturedFlag.Localization;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Replaces text in a tk2dTextMesh with the localized text.
    /// </summary>
    [RequireComponent(typeof(tk2dTextMesh))]
    [RequireComponent(typeof(LocalizeText))]
    public class tk2dLocalizeText : MonoBehaviour
    {
        /// <summary>
        /// Localized text.
        /// </summary>
        private LocalizeText _text;
        /// <summary>
        /// tk2d text mesh to display string from localized text component.
        /// </summary>
        private tk2dTextMesh _textMesh;

        private void Awake()
        {
            _text = GetComponent<LocalizeText>();
            _textMesh = GetComponent<tk2dTextMesh>();
            _text.OnChange += Event_TextChange;
        }

        /// <summary>
        /// Update text display.
        /// </summary>
        private void Event_TextChange()
        {
            _textMesh.text = _text.Text;
            _textMesh.Commit();
        }
    }
}