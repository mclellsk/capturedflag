using System.Collections;
using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Constantly rotate a transform at a specific speed.
    /// </summary>
    public class RotateTransform : MonoBehaviour
    {
        /// <summary>
        /// Speed to rotate.
        /// </summary>
        public float speed = 2f;

        /// <summary>
        /// Axis to rotate around.
        /// </summary>
        public Vector3 axis = new Vector3(0f, 0f, 1f);

        public void Start()
        {
            StartCoroutine(Rotate());
        }

        public void OnEnable()
        {
            StartCoroutine(Rotate());
        }

        private IEnumerator Rotate()
        {
            for (;;)
            {
                this.transform.Rotate(axis * speed * Time.deltaTime);
                yield return 0;
            }
        }
    }
}
