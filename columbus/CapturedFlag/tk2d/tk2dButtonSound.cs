using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Applies sound effects for various tk2d button states using the sound manager.
    /// </summary>
    [RequireComponent(typeof(tk2dUIItem))]
    public class tk2dButtonSound : MonoBehaviour
    {
        /// <summary>
        /// Audio clip for the click state.
        /// </summary>
        public AudioClip clipClick;
        /// <summary>
        /// Audio clip for the down state.
        /// </summary>
        public AudioClip clipDown;
        /// <summary>
        /// Audio clip for the hover state.
        /// </summary>
        public AudioClip clipHover;

        void Awake()
        {
            var item = this.GetComponent<tk2dUIItem>();
            item.OnClick += new System.Action(delegate ()
            {
                if (clipClick != null)
                    Sound.PlaySound(clipClick, 1f, Sound.SoundType.SFX);
            });
            item.OnDown += new System.Action(delegate ()
            {
                if (clipDown != null)
                    Sound.PlaySound(clipDown, 1f, Sound.SoundType.SFX);
            });
            item.OnHoverOver += new System.Action(delegate ()
            {
                if (clipHover != null)
                    Sound.PlaySound(clipHover, 1f, Sound.SoundType.SFX);
            });
        }
    }
}

