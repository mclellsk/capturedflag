using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Apply position changes based on some reference point. By changing the length this object can travel you change the magnitude a change in position of 
    /// the reference point has on this object. Position of the object with parallaxed component will move opposite of the referenced camera.
    /// </summary>
    /// <remarks>
    /// Larger scale values amplify the changes in position while smaller values shrink the change. Smaller changes result in slower moving backgrounds.
    /// </remarks>
    public class Parallax : MonoBehaviour
    {
        /// <summary>
        /// Reference point from which all changes in position are compared to.
        /// </summary>
        public Camera cameraReference;

        /// <summary>
        /// Starting position of parallaxed object.
        /// </summary>
        public Vector3 position = Vector3.zero;
        /// <summary>
        /// Starting position of reference camera.
        /// </summary>
        public Vector3 referencePosition = Vector3.zero;
        /// <summary>
        /// Magnitude of position change on all dimensions of the transform.
        /// </summary>
        public Vector3 scale = Vector3.zero;

        public void Start()
        {
            if (cameraReference == null)
            {
                cameraReference = Camera.main;
            }   
            position = this.transform.position;
        }

        void Update()
        {
            //Change in position of the reference point compared to the reference point's starting position.
            var deltaPosition = referencePosition - cameraReference.transform.position;
            //Parallax movement goes against the direction of the reference movement
            this.transform.position = new Vector3((position.x + (deltaPosition.x * scale.x)), (position.y + (deltaPosition.y * scale.y)), (position.z + (deltaPosition.z * scale.z)));
        }
    }
}
