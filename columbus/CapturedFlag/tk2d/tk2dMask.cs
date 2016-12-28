using UnityEngine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Creates masking mesh the appropriate width and height on both top and bottom (or left and right) of a designated view area at the specified depth.
    /// </summary>
    public class tk2dMask : MonoBehaviour
    {
        /// <summary>
        /// View position is measured in local coordinates.
        /// </summary>
        public Vector3 viewPosition = Vector3.zero;
        /// <summary>
        /// Size of the viewing area.
        /// </summary>
        public Vector2 viewDimensions = Vector2.zero;
        /// <summary>
        /// Height of one masking mesh. Only used if masking vertical overflow.
        /// </summary>
        public float maskHeight = 500f;
        /// <summary>
        /// Width of one masking mesh. Only used if masking horizontal overflow.
        /// </summary>
        public float maskWidth = 500f;
        /// <summary>
        /// Depth to mask.
        /// </summary>
        public float zDepth = 0f;
        /// <summary>
        /// Determines if the view area has overflow horizontally or vertically.
        /// </summary>
        public bool isHorizontal = false;
        /// <summary>
        /// Container for all masks.
        /// </summary>
        private GameObject _maskContainer;

        /// <summary>
        /// Creates a masking mesh based on the position and size.
        /// </summary>
        /// <param name="position">Position of mask.</param>
        /// <param name="size">Size of mask.</param>
        private void CreateMask(Vector3 position, Vector2 size)
        {
            var mask = ((GameObject)Instantiate(Resources.Load("Mask"))).GetComponent<tk2dUIMask>();

            mask.depth = zDepth;
            mask.transform.parent = _maskContainer.transform;
            mask.anchor = tk2dBaseSprite.Anchor.MiddleCenter;
            mask.transform.position = _maskContainer.transform.position + position;//mask1.transform.localPosition = ;
            mask.size = size;
            mask.Build();
            mask.GetComponent<BoxCollider>().size = mask.size;
        }

        private void Start()
        {
            _maskContainer = new GameObject("Masks");
            _maskContainer.transform.parent = this.gameObject.transform;
            _maskContainer.transform.localPosition = Vector3.zero;

            if (!isHorizontal)
            {
                CreateMask(new Vector3(viewPosition.x, viewPosition.y + (viewDimensions.y / 2) / 100 + (maskHeight / 2) / 100, viewPosition.z), new Vector2((viewDimensions.x / 100), (maskHeight / 100)));
                CreateMask(new Vector3(viewPosition.x, viewPosition.y - (viewDimensions.y / 2) / 100 - (maskHeight / 2) / 100, viewPosition.z), new Vector2((viewDimensions.x / 100), (maskHeight / 100)));
            }
            else
            {
                CreateMask(new Vector3(viewPosition.x - (viewDimensions.x / 2) / 100 - (maskWidth / 2) / 100, viewPosition.z), new Vector2((maskWidth / 100), (viewDimensions.y / 100)));
                CreateMask(new Vector3(viewPosition.x + (viewDimensions.x / 2) / 100 + (maskWidth / 2) / 100, viewPosition.z), new Vector2((maskWidth / 100), (viewDimensions.y / 100)));
            }
        }
    }
}