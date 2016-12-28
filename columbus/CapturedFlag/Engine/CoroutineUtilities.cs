using System.Collections;
using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// This independent of update time, time counter solution was found here: 
    /// http://answers.unity3d.com/questions/787180/make-a-coroutine-run-when-timetimescale-0.html
    /// </summary>
    public static class CoroutineUtilities
    {
        public static IEnumerator WaitForRealTime(float delay)
        {
            while (true)
            {
                float pauseEndTime = ((Time.time >= 0f && Time.time <= 0.25f) ? 0f : Time.realtimeSinceStartup) + delay;
                while (Time.realtimeSinceStartup < pauseEndTime)
                {
                    yield return 0;
                }
                break;
            }
        }

        public static IEnumerator WaitForUnscaledTime(float delay)
        {
            var timeElapsed = 0f;
            while (timeElapsed < delay)
            {
                timeElapsed += Time.unscaledDeltaTime;
                yield return 0;
            }
        }
    }
}