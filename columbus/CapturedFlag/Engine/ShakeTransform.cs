using UnityEngine;
using System.Collections;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Periodically alters position and rotation of transform for a specific amount of time.
    /// </summary>
    public class ShakeTransform : MonoBehaviour
    {
        /// <summary>
        /// The position damping curve.
        /// </summary>
        public AnimationCurve positionDamping;
        /// <summary>
        /// The rotation damping curve.
        /// </summary>
        public AnimationCurve rotationDamping;

        /// <summary>
        /// World position of transform.
        /// </summary>
        private Vector3 _originalPos = Vector3.zero;
        /// <summary>
        /// Local rotation of transform.
        /// </summary>
        private Vector3 _localRot = Vector3.zero;

        private Coroutine _cShaking;

        /// <summary>
        /// Start shaking the transform.
        /// </summary>
        /// <param name="xOffset">Minimum and maximum values to offset the x-coordinate.</param>
        /// <param name="yOffset">Minimum and maximum values to offset the y-coordinate.</param>
        /// <param name="zOffset">Minimum and maximum values to offset the z-coordinate.</param>
        /// <param name="xRotOffset">Minimum and maximum degrees to offset the x-rotation.</param>
        /// <param name="yRotOffset">Minimum and maximum degrees to offset the y-rotation.</param>
        /// <param name="zRotOffset">Minimum and maximum degrees to offset the z-rotation.</param>
        /// <param name="timeBetweenShakes">Time to wait between position changes.</param>
        /// <param name="steps">Number of changes.</param>
        public void StartShake(Vector2 xOffset, Vector2 yOffset, Vector2 zOffset, Vector2 xRotOffset, Vector2 yRotOffset, Vector2 zRotOffset, float timeBetweenShakes, int steps)
        {
            Stop();
            _cShaking = StartCoroutine(Shaking(xOffset, yOffset, zOffset, xRotOffset, yRotOffset, zRotOffset, timeBetweenShakes, steps));
        }

        public void Stop()
        {
            if (_cShaking != null)
            {
                StopCoroutine(_cShaking);
                transform.position = _originalPos;
                transform.localRotation = Quaternion.Euler(_localRot);
            }
        }

        /// <summary>
        /// Shakes the transform.
        /// </summary>
        /// <param name="xOffset">Minimum and maximum values to offset the x-coordinate.</param>
        /// <param name="yOffset">Minimum and maximum values to offset the y-coordinate.</param>
        /// <param name="zOffset">Minimum and maximum values to offset the z-coordinate.</param>
        /// <param name="xRotOffset">Minimum and maximum degrees to offset the x-rotation.</param>
        /// <param name="yRotOffset">Minimum and maximum degrees to offset the y-rotation.</param>
        /// <param name="zRotOffset">Minimum and maximum degrees to offset the z-rotation.</param>
        /// <param name="timeBetweenShakes">Time to wait between position changes.</param>
        /// <param name="steps">Number of changes.</param>
        /// <returns>Coroutine</returns>
        private IEnumerator Shaking(Vector2 xOffset, Vector2 yOffset, Vector2 zOffset, Vector2 xRotOffset, Vector2 yRotOffset, Vector2 zRotOffset, float timeBetweenShakes, int steps = 0)
        {
            _originalPos = transform.position;
            _localRot = transform.localRotation.eulerAngles;

            for (int i = 0; i < steps; i++)
            {
                var f = (positionDamping != null) ? positionDamping.Evaluate((float)i / steps) : 1f;
                var posFactor = 1 - f;
                transform.position = new Vector3(   _originalPos.x + UnityEngine.Random.Range(xOffset.x, xOffset.y) * posFactor,
                                                    _originalPos.y + UnityEngine.Random.Range(yOffset.x, yOffset.y) * posFactor,
                                                    _originalPos.z + UnityEngine.Random.Range(zOffset.x, zOffset.y) * posFactor);

                f = (rotationDamping != null) ? rotationDamping.Evaluate((float)i / steps) : 1f;
                var rotFactor = 1 - f;
                transform.localRotation = Quaternion.Euler(new Vector3( _localRot.x + UnityEngine.Random.Range(xRotOffset.x, xRotOffset.y) * rotFactor,
                                                                        _localRot.y + UnityEngine.Random.Range(yRotOffset.x, yRotOffset.y) * rotFactor,
                                                                        _localRot.z + UnityEngine.Random.Range(zRotOffset.x, zRotOffset.y) * rotFactor));
                yield return new WaitForSeconds(timeBetweenShakes);
            }
            transform.position = _originalPos;
            transform.localRotation = Quaternion.Euler(_localRot);
        }
    }
}