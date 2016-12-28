using UnityEngine;
using CapturedFlag.Engine.Scriptables;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Background manager controls which background to instantiate for 2D games. Only one active at a time.
    /// </summary>
    public class BackgroundManager : MonoBehaviour
    {
        /// <summary>
        /// Instance of BackgroundManager.
        /// </summary>
        public static BackgroundManager instance;

        /// <summary>
        /// Sorted list of all available backgrounds.
        /// </summary>
        public GameObjectLookup backgrounds;

        /// <summary>
        /// Position to display background based on pivot.
        /// </summary>
        public Transform position;

        /// <summary>
        /// Starting reference position to compare camera position to.
        /// </summary>
        public Vector3 referencePosition;

        /// <summary>
        /// Background game object currently being used.
        /// </summary>
        private GameObject _background;

        /// <summary>
        /// Determines if background is randomly selected.
        /// </summary>
        public bool isRandom = true;

        public void Awake()
        {
            instance = this;

            if (isRandom)
                SetBackground(UnityEngine.Random.Range(0, backgrounds.lookup.Count));
        }

        /// <summary>
        /// Set the background to the object located at index i.
        /// </summary>
        /// <param name="index">Index of background.</param>
        public void SetBackground(int index)
        {
            if (index >= 0 && index < backgrounds.lookup.Count)
            {
                if (_background != null)
                {
                    DestroyObject(_background);
                }

                _background = (GameObject)Instantiate((GameObject)backgrounds.lookup[index].value, position.position, Quaternion.identity);
                _background.name = "Background";
                _background.transform.parent = position;
                var parallax = _background.GetComponentsInChildren<Parallax>();
                foreach (Parallax p in parallax)
                {
                    p.referencePosition = referencePosition;
                }
            }
        }

        /// <summary>
        /// Set the background to the object with the specified key.
        /// </summary>
        /// <param name="key">Key of background.</param>
        public void SetBackground(string key)
        {
            var background = backgrounds.lookup.Find(p => p.key == key);
            if (background != null)
            {
                if (_background != null)
                {
                    DestroyObject(_background);
                }

                _background = (GameObject)Instantiate((GameObject)background.value, position.position, Quaternion.identity);
                _background.name = "Background";
                _background.transform.parent = position;
                var parallax = _background.GetComponentsInChildren<Parallax>();
                foreach (Parallax p in parallax)
                {
                    p.referencePosition = referencePosition;
                }
            }
        }
    }
}
