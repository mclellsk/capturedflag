using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Additional event tracking for inputs relating to hold time.
    /// </summary>
    public class ActionEvents
    {
        /// <summary>
        /// Inputs and action name associated with this tracking instance.
        /// </summary>
        public ActionInput action;

        /// <summary>
        /// Time that has elapsed since action was first triggered.
        /// </summary>
        private float _timeElapsed = 0f;

        /// <summary>
        /// Prevents the time from reaching an overflow.
        /// </summary>
        public float TimeElapsed
        {
            get { return _timeElapsed; }
            set
            {
                if (value <= float.MaxValue && value >= float.MinValue)
                {
                    _timeElapsed = value;
                }
            }
        }

        /// <summary>
        /// Set properties of tracking instance.
        /// </summary>
        /// <param name="name">Action name.</param>
        /// <param name="buttons">Buttons which trigger action.</param>
        /// <param name="keys">Keys which trigger action.</param>
        public ActionEvents(string name, List<KeyCode> keys, List<int> buttons)
        {
            action = new ActionInput(name, keys, buttons);
        }

        /// <summary>
        /// Time added since last update.
        /// </summary>
        public void UpdateTime()
        {
            _timeElapsed += Time.deltaTime;
        }

        /// <summary>
        /// Clear time elapsed.
        /// </summary>
        public void ClearTime()
        {
            _timeElapsed = 0f;
        }
    }
}