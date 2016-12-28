using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Adds a letterbox to the top and bottom or the left and right sides of the screen depending on orientation. This is used when the camera view space 
    /// needs to remain the same regardless of the actual resolution (i.e. more screen space is not given/taken at higher/lower resolutions, 
    /// it is simply scaled differently).
    /// </summary>
    public class tk2dResolutionFix : MonoBehaviour
    {
        private Vector2 _oldTargetResolution;
        private Vector2 _oldNativeResolution;

        private GameObject _borderTop;
        private GameObject _borderBottom;
        private GameObject _borderLeft;
        private GameObject _borderRight;

        void RebuildScreen()
        {
            if (_borderTop != null)
                GameObject.Destroy(_borderTop);
            if (_borderBottom != null)
                GameObject.Destroy(_borderBottom);
            if (_borderLeft != null)
                GameObject.Destroy(_borderLeft);
            if (_borderRight != null)
                GameObject.Destroy(_borderRight);

            var cam = Camera.main.GetComponent<tk2dCamera>();
            var pRes = cam.TargetResolution;
            var nRes = cam.NativeResolution;

            int zDepth = 50;
            float offset = 1;
            float padding = 5f;
            float aspectRatio;

            //Widescreen
            if (pRes.x / pRes.y < 1)
            {
                aspectRatio = nRes.x / pRes.x;

                var newY = aspectRatio * pRes.y;
                var deltaY = (newY - nRes.y) / 2;
                var posY = (nRes.y / 2) + deltaY / 2;

                if (Mathf.Abs(deltaY) > 0)
                {
                    _borderTop = Actor.Instantiate(Resources.Load("Letterbox"), new Vector3(this.transform.position.x, this.transform.position.y + posY / cam.CameraSettings.orthographicPixelsPerMeter, this.transform.position.z + zDepth), Quaternion.identity, null, "Letterbox_Wide");
                    _borderTop.GetComponent<tk2dSlicedSprite>().dimensions = new Vector2(nRes.x + padding, deltaY + offset);
                    _borderTop.GetComponent<tk2dSlicedSprite>().SortingOrder = zDepth;
                    _borderBottom = Actor.Instantiate(Resources.Load("Letterbox"), new Vector3(this.transform.position.x, this.transform.position.y + -posY / cam.CameraSettings.orthographicPixelsPerMeter, this.transform.position.z + zDepth), Quaternion.identity, null, "Letterbox_Wide");
                    _borderBottom.GetComponent<tk2dSlicedSprite>().dimensions = new Vector2(nRes.x + padding, deltaY + offset);
                    _borderBottom.GetComponent<tk2dSlicedSprite>().SortingOrder = zDepth;
                }
            }
            else if (pRes.x / pRes.y > 1)
            {
                aspectRatio = nRes.y / pRes.y;

                var newX = aspectRatio * pRes.x;
                var deltaX = (newX - nRes.x) / 2;
                var posX = (nRes.x / 2) + deltaX / 2;

                if (Mathf.Abs(deltaX) > 0)
                {
                    _borderLeft = Actor.Instantiate(Resources.Load("Letterbox"), new Vector3(this.transform.position.x + posX / cam.CameraSettings.orthographicPixelsPerMeter, this.transform.position.y, this.transform.position.z + zDepth), Quaternion.identity, null, "Letterbox_Narrow");
                    _borderLeft.GetComponent<tk2dSlicedSprite>().dimensions = new Vector2(deltaX + offset, nRes.y + padding);
                    _borderLeft.GetComponent<tk2dSlicedSprite>().SortingOrder = zDepth;
                    _borderRight = Actor.Instantiate(Resources.Load("Letterbox"), new Vector3(this.transform.position.x + -posX / cam.CameraSettings.orthographicPixelsPerMeter, this.transform.position.y, this.transform.position.z + zDepth), Quaternion.identity, null, "Letterbox_Narrow");
                    _borderRight.GetComponent<tk2dSlicedSprite>().dimensions = new Vector2(deltaX + offset, nRes.y + padding);
                    _borderRight.GetComponent<tk2dSlicedSprite>().SortingOrder = zDepth;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            var cam = Camera.main.GetComponent<tk2dCamera>();
            var pRes = cam.TargetResolution;
            var nRes = cam.NativeResolution;

            if (_oldNativeResolution != nRes || _oldTargetResolution != pRes)
            {
                RebuildScreen();
                _oldNativeResolution = nRes;
                _oldTargetResolution = pRes;
            }
        }
    }
}

