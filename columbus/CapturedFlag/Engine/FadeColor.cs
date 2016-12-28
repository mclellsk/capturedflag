using UnityEngine;
using System.Collections;

namespace CapturedFlag.Engine
{
    public class FadeColor : MonoBehaviour
    {
        /// <summary>
        /// Calculated amount to increase/decrease alpha by per change.
        /// </summary>
        private float _alphaStep = 0f;
        /// <summary>
        /// Determines whether the behaviour has already been initialized.
        /// </summary>
        private bool _bInitialized = false;

        private Coroutine _cFadeRoutine;

        /// <summary>
        /// Color that is controlled by fade component.
        /// </summary>
        public Color color = Color.white;

        /// <summary>
        /// Starting alpha for fade in.
        /// </summary>
        public float startAlphaIn = 0f;
        /// <summary>
        /// Starting alpha for fade out.
        /// </summary>
        public float startAlphaOut = 1f;
        /// <summary>
        /// Time to wait before fade occurs.
        /// </summary>
        public float delayTime = 2f;
        /// <summary>
        /// Time until fade is complete.
        /// </summary>
        public float totalTime = 2f;
        /// <summary>
        /// Time to wait between changes.
        /// </summary>
        public float timeBetweenChanges = 0.1f;

        /// <summary>
        /// Determines if fading is paused.
        /// </summary>
        public bool isActive = false;
        /// <summary>
        /// Determines if fading is complete for the lifecycle.
        /// </summary>
        public bool isComplete = false;
        /// <summary>
        /// Determines if fading should restart on completion.
        /// </summary>
        public bool bResetOnComplete = true;
        /// <summary>
        /// Determines if object goes inactive when fade is complete.
        /// </summary>
        public bool bInactiveOnComplete = true;
        /// <summary>
        /// Determines if fading starts as a fade out or a fade in.
        /// </summary>
        public bool bFadeOut = true;
        /// <summary>
        /// Determines if fading switches between fade out and fade in when fade is complete.
        /// </summary>
        public bool bOscillate = false;
        /// <summary>
        /// Determines if this behaviour initializes every time the gameobject is activated.
        /// </summary>
        public bool bOnEnable = true;
        /// <summary>
        /// Determines if fading uses real-time as opposed to update time, prevents fade from being effected due to time scaling.
        /// </summary>
        public bool bUseTimeScale = false;

        /// <summary>
        /// Fade out complete callback.
        /// </summary>
        public event System.Action OnFadeOut;
        /// <summary>
        /// Fade in complete callback.
        /// </summary>
        public event System.Action OnFadeIn;

        /// <summary>
        /// Initialize fade based on settings.
        /// </summary>
        private void Initialize()
        {
            _cFadeRoutine = null;

            isComplete = false;
            isActive = true;

            _alphaStep = Mathf.Abs(startAlphaOut - startAlphaIn) / totalTime;

            if (bFadeOut)
                color = new Color(color.r, color.g, color.b, startAlphaOut);
            else
                color = new Color(color.r, color.g, color.b, startAlphaIn);

            _cFadeRoutine = StartCoroutine(FadeRoutine());
        }

        void Start()
        {
            Initialize();
            _bInitialized = true;
        }

        void OnEnable()
        {
            if (bOnEnable && _bInitialized)
                Initialize();
        }

        public void Stop()
        {
            if (_cFadeRoutine != null)
                StopCoroutine(_cFadeRoutine);
        }

        public void SetColor(Color color)
        {
            this.color = new Color(color.r, color.g, color.b, this.color.a);
        }

        public void Reset()
        {
            Stop();
            Initialize();
        }

        private IEnumerator FadeRoutine()
        {
            if (bUseTimeScale)
            {
                yield return new WaitForSeconds(delayTime);

                for (;;)
                {
                    Fade(_alphaStep * Time.deltaTime);

                    if (isActive)
                        yield return 0;
                    else
                        break;
                }
            }
            else
            {
                yield return StartCoroutine(CoroutineUtilities.WaitForRealTime(delayTime));

                for (;;)
                {
                    Fade(_alphaStep * Time.unscaledDeltaTime);

                    if (isActive)
                        yield return 0;
                    else
                        break;
                }
            }
        }

        void Fade(float alphaStep)
        {
            var sign = (bFadeOut) ? 1 : -1;
            var startAlpha = (bFadeOut) ? startAlphaIn : startAlphaOut;
            var alpha = (bFadeOut) ? Mathf.Max(startAlpha, color.a - alphaStep) : Mathf.Min(startAlpha, color.a + alphaStep);

            if (sign * color.a > sign * startAlpha)
                color = new Color(color.r, color.g, color.b, alpha);

            if ((int)(sign * color.a * 1000) <= (int)(sign * startAlpha * 1000))
                isComplete = true;

            if (bInactiveOnComplete && isComplete)
                isActive = false;

            if (bResetOnComplete && isComplete)
                Reset();

            if (isComplete)
            {
                isComplete = false;

                if (bFadeOut)
                {
                    if (OnFadeOut != null)
                        OnFadeOut();
                }
                else
                {
                    if (OnFadeIn != null)
                        OnFadeIn();
                }

                if (bOscillate)
                {
                    bFadeOut = !bFadeOut;
                    Reset();
                }
            }
        }
    }
}