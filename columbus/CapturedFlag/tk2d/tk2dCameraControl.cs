using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Camera controller specifically for tk2dCamera implementation.
    /// </summary>
    [RequireComponent(typeof(tk2dCamera))]
    public class tk2dCameraControl : CameraControl
    {
        public override void DeltaZoom(float amount)
        {
            if (cameraControlled.orthographic)
            {
                var tk2dcam = cameraControlled.GetComponent<tk2dCamera>();
                SetZoom(tk2dcam.ZoomFactor + amount);
            }
            else
            {
                SetZoom(cameraControlled.fieldOfView + amount);
            }
        }
        public override void SetZoom(float amount)
        {
            if (cameraControlled.orthographic)
            {
                tk2dCamera tk2dcam = cameraControlled.GetComponent<tk2dCamera>();

                tk2dcam.ZoomFactor = amount;

                if (tk2dcam.ZoomFactor > zoomLimit.y)
                {
                    tk2dcam.ZoomFactor = zoomLimit.y;
                }
                else if (tk2dcam.ZoomFactor < zoomLimit.x)
                {
                    tk2dcam.ZoomFactor = zoomLimit.x;
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
    }
}
