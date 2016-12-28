using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    public class SlideGallery : MonoBehaviour
    {
        /// <summary>
        /// Content to show in the gallery. Each object is a page of the gallery, the center of the page is the pivot of the gameobject.
        /// </summary>
        public List<GameObject> content = new List<GameObject>();

        /// <summary>
        /// Current page index of the gallery.
        /// </summary>
        public int index = 0;

        /// <summary>
        /// Time in seconds to transition from one page to another.
        /// </summary>
        public float transitionTime = 1f;

        /// <summary>
        /// Determines if the gallery is used to move horizontally or vertically.
        /// </summary>
        public bool isHorizontal = true;

        /// <summary>
        /// Approximate size of page, used for distance between pages.
        /// </summary>
        public Vector2 size = Vector2.zero;

        /// <summary>
        /// Container storing all pages in the gallery.
        /// </summary>
        public Move3D container;

        /// <summary>
        /// Starting position of the container, initial page position.
        /// </summary>
        private Vector3 startPosition = Vector3.zero;

        /// <summary>
        /// Determines if the gallery is in the middle of a page transition.
        /// </summary>
        private bool _inTransition = false;

        public void Awake()
        {
            Initialize();   
        }

        public void Initialize()
        {
            startPosition = container.transform.localPosition;

            for (int i = 0; i < content.Count; i++)
            {
                if (isHorizontal)
                    content[i].transform.localPosition = new Vector3(i * size.x, content[i].transform.localPosition.y, content[i].transform.localPosition.z);
                else
                    content[i].transform.localPosition = new Vector3(content[i].transform.localPosition.x, i * size.y, content[i].transform.localPosition.z);
            }
        }

        /// <summary>
        /// Move to the next page.
        /// </summary>
        public void Next()
        {
            if (!container.IsTargetSet)
            {
                if ((index + 1) < content.Count)
                {
                    index++;
                    Move();
                }
            }
        }

        /// <summary>
        /// Move to the previous page.
        /// </summary>
        public void Prev()
        {
            if (!container.IsTargetSet)
            {
                if ((index - 1) >= 0)
                {
                    index--;
                    Move();
                }
            }
        }

        /// <summary>
        /// Move to specific page index.
        /// </summary>
        /// <param name="slideIndex"></param>
        public void GoToSlide(int slideIndex)
        {
            if (!container.IsTargetSet)
            {
                if (slideIndex < content.Count && slideIndex >= 0)
                {
                    index = slideIndex;
                    Move();
                }
            }
        }

        /// <summary>
        /// Move to the index currently selected.
        /// </summary>
        public void Move()
        {
            if (isHorizontal)
            {
                if (transitionTime > 0)
                    container.SetTargetPosition(startPosition - new Vector3(size.x * index, 0f, 0f), transitionTime);
                else
                    container.transform.localPosition = startPosition - new Vector3(size.x * index, 0f, 0f);
            }
            else
            {
                if (transitionTime > 0)
                    container.SetTargetPosition(startPosition - new Vector3(0f, size.y * index, 0f), transitionTime);
                else
                    container.transform.localPosition = startPosition - new Vector3(0f, size.y * index, 0f);
            }
        }
    }
}
