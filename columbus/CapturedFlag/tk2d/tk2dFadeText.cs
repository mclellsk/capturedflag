using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Applies fade effect to the gradient colors of the tk2dTextMesh.
    /// </summary>
    [RequireComponent(typeof(tk2dTextMesh))]
    public class tk2dFadeText : MonoBehaviour
    {
        /// <summary>
        /// Top gradient color.
        /// </summary>
        public FadeColor fade1;
        /// <summary>
        /// Bottom gradient color.
        /// </summary>
        public FadeColor fade2;

        /// <summary>
        /// Text mesh to apply the gradient color to.
        /// </summary>
        private tk2dTextMesh _text;

        public void Awake()
        {
            _text = GetComponent<tk2dTextMesh>();
            SetColors(_text.color, _text.color2);
        }

        public void Update()
        {
            _text.color = fade1.color;
            _text.color2 = fade2.color;
            _text.Commit();
        }

        /// <summary>
        /// Set colors for the fade component of the text gradient.
        /// </summary>
        /// <param name="color1">Color for the top part of the gradient.</param>
        /// <param name="color2">Color for the bottom part of the gradient.</param>
        public void SetColors(Color color1, Color color2)
        {
            fade1.color = color1;
            fade2.color = color2;
            _text.color = color1;
            _text.color2 = color2;
            _text.Commit();
        }

        /// <summary>
        /// Set the time to wait before beginning the fade.
        /// </summary>
        /// <param name="time">Time to wait.</param>
        public void SetDelay(float time)
        {
            fade1.delayTime = time;
            fade2.delayTime = time;
        }

        /// <summary>
        /// Reset the fade component of the text gradient.
        /// </summary>
        public void Reset()
        {
            fade1.Reset();
            fade2.Reset();
        }

        /// <summary>
        /// Activate the fade components.
        /// </summary>
        public void SetActive()
        {
            fade1.isActive = true;
            fade2.isActive = true;
        }
    }
}
