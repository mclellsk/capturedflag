using UnityEngine;

namespace CapturedFlag.Engine
{
    public static class MonoExtender
    {
        /// <summary>
        /// Used on any MonoBehaviour to create a timer.
        /// </summary>
        /// <param name="mono">Monobehaviour associated with timer.</param>
        /// <param name="timeToWait">Time to wait between firings of the timer.</param>
        /// <param name="numberOfTimes">Number of times the timer fires.</param>
        /// <returns>Instance of timer.</returns>
        public static Timer CreateTimer(this MonoBehaviour mono, float timeToWait, int cycles = -1)
        {
            return new Timer(mono, timeToWait, cycles);
        }
    }
}
