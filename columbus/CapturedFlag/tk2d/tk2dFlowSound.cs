using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Add sound to selection of scrollable flow.
    /// </summary>
    [RequireComponent(typeof(tk2dUIScrollableFlow))]
    public class tk2dFlowSound : MonoBehaviour
    {
        /// <summary>
        /// Sound made on selection of content within the scrollable flow.
        /// </summary>
        public AudioClip clipChange;

        void Awake()
        {
            GetComponent<tk2dUIScrollableFlow>().OnSelectChanged += PlaySound;
        }

        /// <summary>
        /// Play one shot selection sound.
        /// </summary>
        private void PlaySound()
        {
            if (clipChange != null)
                Sound.PlayAtPoint(clipChange, Camera.main.transform.position, 1f, Sound.SoundType.SFX);
        }
    }
}
