using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Controls the movement and zoom of the camera using mouse or touch input.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraControl : MonoBehaviour
    {
        /// <summary>
        /// Boundary for the camera to move within.
        /// </summary>
        public Collider boundary;

        /// <summary>
        /// Camera instance.
        /// </summary>
        public Camera cameraControlled;

        /// <summary>
        /// Determines if the controls affect movement.
        /// </summary>
        public bool canMove = true;
        /// <summary>
        /// Determines if the controls affect zooming.
        /// </summary>
        public bool canZoom = true;
        /// <summary>
        /// Determines if camera controls are enabled for this camera.
        /// </summary>
        public bool isEnabled = true;

        /// <summary>
        /// Zoom speed of camera.
        /// </summary>
        public float zoomSpeed = 0.001f;

        /// <summary>
        /// Minimum and maximum zoom. When camera is orthographic, this is the scale factor. When camera is perspective, this is the field of view.
        /// </summary>
        public Vector2 zoomLimit = new Vector2(0.1f, 0.35f);

        /// <summary>
        /// Pan speed of camera in both the x and y axis.
        /// </summary>
        public Vector2 panSpeed = new Vector2(1f, 1f);

        /// <summary>
        /// The minimum delta required for a pan to begin.
        /// </summary>
        public float panThreshold = 1f;

        /// <summary>
        /// Previous position of the camera.
        /// </summary>
        private Vector3 _oldPosition = Vector3.zero;

        /// <summary>
        /// Change the current zoom by the amount specified.
        /// </summary>
        /// <param name="amount">Amount to change.</param>
        public virtual void DeltaZoom(float amount)
        {
            if (cameraControlled.orthographic)
            {
                SetZoom(cameraControlled.orthographicSize + amount);
            }
            else
            {
                SetZoom(cameraControlled.fieldOfView + amount);
            }
        }

        /// <summary>
        /// Set the zoom to the amount specified.
        /// </summary>
        /// <param name="amount">Amount to set zoom to.</param>
        public virtual void SetZoom(float amount)
        {
            if (cameraControlled.orthographic)
            {
                cameraControlled.orthographicSize = amount;

                if (cameraControlled.orthographicSize > zoomLimit.y)
                {
                    cameraControlled.orthographicSize = zoomLimit.y;
                }
                else if (cameraControlled.orthographicSize < zoomLimit.x)
                {
                    cameraControlled.orthographicSize = zoomLimit.x;
                }
            }
            else
            {
                cameraControlled.fieldOfView = amount;

                if (cameraControlled.fieldOfView > zoomLimit.y)
                {
                    cameraControlled.fieldOfView = zoomLimit.y;
                }
                else if (cameraControlled.fieldOfView < zoomLimit.x)
                {
                    cameraControlled.fieldOfView = zoomLimit.x;
                }
            }      
        }

        /// <summary>
        /// Set the position to the amount specified.
        /// </summary>
        /// <param name="pos">Coordinates to set position to.</param>
        public virtual void SetPosition(Vector3 pos)
        {
            if (boundary != null)
            {
                var newPosition = cameraControlled.transform.position;
                if (boundary.bounds.min.x < pos.x && boundary.bounds.max.x > pos.x)
                {
                    newPosition.x = pos.x;
                }
                if (boundary.bounds.min.y < pos.y && boundary.bounds.max.y > pos.y)
                {
                    newPosition.y = pos.y;
                }
                if (boundary.bounds.min.z < pos.z && boundary.bounds.max.z > pos.z)
                {
                    newPosition.z = pos.z;
                }
                if (boundary.bounds.Contains(newPosition))
                {
                    cameraControlled.transform.position = newPosition;
                }
            }
            else
            {
                cameraControlled.transform.position = pos;
            }
        }

        public void Update()
        {
            if (isEnabled)
            {
#if UNITY_ANDROID
                //Drag-To-Pan
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        var delta = Input.GetTouch(0).deltaPosition;
                        if (Mathf.Abs(delta.magnitude) > panThreshold)
                        {
                            var pos = this.cameraControlled.transform.position + new Vector3(-delta.x * panSpeed.x, -delta.y * panSpeed.y, 0f);

                            if (canMove)
                                SetPosition(pos);
                        }
                    }
                }

                //Pinch-To-Zoom
                if (Input.touchCount == 2)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        Touch touchZero = Input.GetTouch(0);
                        Touch touchOne = Input.GetTouch(1);

                        Vector2 touchZeroPrev = touchZero.position - touchZero.deltaPosition;
                        Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition;

                        float prevTouchDeltaMag = (touchZeroPrev - touchOnePrev).magnitude;
                        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                        if (canZoom)
                            DeltaZoom(deltaMagnitudeDiff * zoomSpeed);                          
                    }
                }
#endif

#if UNITY_EDITOR || UNITY_5
                if (ActionManager.GetActionDown("main", "Alternate"))
                {
                    //Tracked to get position where click started to form delta position
                    _oldPosition = Input.mousePosition;
                }

                if (ActionManager.GetAction("main", "Alternate"))
                {
                    //Camera Pan Control
                    var deltaMousePosition = Input.mousePosition - _oldPosition;
                    if (deltaMousePosition != Vector3.zero)
                    {
                        var delta = new Vector2((deltaMousePosition.x / Screen.width), (deltaMousePosition.y / Screen.height));
                        if (Mathf.Abs(delta.magnitude) > panThreshold)
                        {
                            var pos = this.cameraControlled.transform.position + new Vector3(delta.x * panSpeed.x, delta.y * panSpeed.y, 0f);

                            if (canMove)
                                SetPosition(pos);
                        }
                    }
                }

                //Scroll-To-Zoom
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    if (canZoom)
                        DeltaZoom(Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
                }
#endif
            }
        }

        /// <summary>
        /// Allow camera controllers to accept or reject input.
        /// </summary>
        /// <param name="isEnabled">Determines if the camera accepts input.</param>
        /// <param name="cameras">Cameras to change.</param>
        public static void SetEnable(bool isEnabled, params Camera[] cameras)
        {
            for (int i = 0; i < cameras.Length; i++)
                cameras[i].GetComponent<CameraControl>().isEnabled = isEnabled;
        }
    }
}
