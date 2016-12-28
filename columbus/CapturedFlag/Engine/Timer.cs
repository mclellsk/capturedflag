using UnityEngine;
using System;
using System.Collections;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// A typical timer class containing different timer callbacks. Useful for repitious events. Each instance keeps track of specific properties of 
    /// the timer like times fired and time elapsed. Should be used over a regular coroutine using WaitForSeconds within a MonoBehaviour when you have multiple timed events that
    /// need to be tracked individually.
    /// </summary>
    public class Timer
    {
        /// <summary>
        /// Timer stopped callback.
        /// </summary>
        public event System.Action OnStop;
        /// <summary>
        /// Timer started callback.
        /// </summary>
        public event System.Action OnStart;
        /// <summary>
        /// Timer paused callback.
        /// </summary>
        public event System.Action OnPause;
        /// <summary>
        /// Timer updated callback.
        /// </summary>
        public event System.Action OnInterval;
        /// <summary>
        /// Timer completed waiting callback.
        /// </summary>
        public event System.Action OnComplete;

        /// <summary>
        /// The time to wait until timer event is triggered.
        /// </summary>
        public float timeToWait = 0;

        /// <summary>
        /// Total time elapsed in lifetime of timer.
        /// </summary>
        public float TotalTimeElapsed
        {
            get { return _totalTimeElapsed; }
            set
            {
                if (value <= float.MaxValue && value >= float.MinValue)
                    _totalTimeElapsed = value;
            }
        }
        /// <summary>
        /// Time remaining until timer is complete.
        /// </summary>
        public float TimeRemaining
        {
            get
            {
                return timeToWait - _timeElapsed;
            }
        }

        /// <summary>
        /// Determines if the timer has completed all cycles in its life.
        /// </summary>
        public bool IsStopped
        {
            get { return _currentCycle == _cycles; }
        }
        public bool IsPaused
        {
            get { return _isPaused; }
        }

        /// <summary>
        /// MonoBehaviour to start timer coroutines on.
        /// </summary>
        private MonoBehaviour _mono;

        /// <summary>
        /// Determines if timer is paused.
        /// </summary>
        private bool _isPaused = true;

        /// <summary>
        /// The time elapsed since the timer was last fired.
        /// </summary>
        private float _timeElapsed = 0;
        /// <summary>
        /// The total time that has elapsed since the timer was started.
        /// </summary>
        private float _totalTimeElapsed = 0f;

        /// <summary>
        /// The number of times this timer occurs, set to -1 for infinite.
        /// </summary>
        private int _cycles;
        /// <summary>
        /// The number of times the timer has completed so far.
        /// </summary>
        private int _currentCycle = 0;

        private Coroutine _cUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Timer"/> class.
        /// </summary>
        /// <param name="timeToWait">Time to wait.</param>
        /// <param name="numberOfTimes">Number of times.</param>
        /// <param name="intervals">Number of update intervals</param> 
        public Timer(MonoBehaviour mono, float timeToWait, int cycles = -1)
        {
            _mono = mono;
            this.timeToWait = timeToWait;
            _cycles = cycles;
        }

        /// <summary>
        /// Update cycle for timer, fires event on completion of a cycle based on the time to wait.
        /// </summary>
        /// <returns></returns>
        private IEnumerator TimerUpdate()
        {
            //Ignores the rest of the update before the first global Update() is called if coroutine is started from Start() or Awake()
            yield return 0;

            for (;;)
            {
                if (!_isPaused)
                {
                    if ((_cycles > 0 && _currentCycle != _cycles) || _cycles == -1)
                    {
                        _totalTimeElapsed += Time.deltaTime;

                        if (OnInterval != null)
                        {
                            OnInterval();
                        }

                        if (_timeElapsed > timeToWait)
                        {
                            if (OnComplete != null)
                            {
                                OnComplete();
                            }

                            _timeElapsed -= timeToWait;

                            if (_currentCycle < _cycles)
                            {
                                _currentCycle++;
                            }
                        }
                        else
                        {
                            _timeElapsed += Time.deltaTime;
                        }
                    }
                    else
                    {
                        Stop();
                    }
                }
                yield return 0;
            }
        }

        public void Stop()
        {
            StopUpdate();

            _isPaused = true;

            if (OnStop != null)
            {
                OnStop();
            }
        }

        public void Pause()
        {
            if (!_isPaused)
            {
                _isPaused = true;

                if (OnPause != null)
                {
                    OnPause();
                }
            }
        }

        public void Start()
        {
            if (_isPaused)
            {
                _isPaused = false;

                if (OnStart != null)
                {
                    OnStart();
                }

                StartUpdate();
            }
        }

        public void Enable()
        {
            StartUpdate();
        }

        private void StartUpdate()
        {
            StopUpdate();
            if (_cUpdate == null)
            {
                _cUpdate = _mono.StartCoroutine(TimerUpdate());
            }
        }

        private void StopUpdate()
        {
            if (_cUpdate != null)
            {
                _mono.StopCoroutine(_cUpdate);
                _cUpdate = null;
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset()
        {
            _currentCycle = 0;
            _timeElapsed = 0f;
            _totalTimeElapsed = 0f;
        }

        /// <summary>
        /// Clear all hooks that are attached to the events in this timer.
        /// </summary>
        public void ClearHooks()
        {
            OnComplete = null;
            OnStop = null;
            OnStart = null;
            OnPause = null;
            OnInterval = null;
        }

        [Flags]
        public enum TimeFormat
        {
            None = 0,
            Seconds = 1,
            Minutes = 2,
            Hours = 4
        };

        /// <summary>
        /// Returns the timer's time remaining in the format of HH:MM:SS
        /// </summary>
        /// <returns>Time as string.</returns>
        public string ToString(TimeFormat format)
        {
            var timeInSeconds = ((int)timeToWait - (int)_timeElapsed);
            System.TimeSpan t = System.TimeSpan.FromSeconds(timeInSeconds);

            string message = "";

            bool secondsIncluded = (format & TimeFormat.Seconds) != TimeFormat.None;
            bool minutesIncluded = (format & TimeFormat.Minutes) != TimeFormat.None;
            bool hoursIncluded = (format & TimeFormat.Hours) != TimeFormat.None;

            if (hoursIncluded)
            {
                message += string.Format("{0:D2}", t.Hours);
                if (minutesIncluded || secondsIncluded)
                    message += ":";
            }
            if (minutesIncluded)
            {
                message += string.Format("{0:D2}", t.Minutes);
                if (secondsIncluded)
                    message += ":";
            }
            if (secondsIncluded)
            {
                message += string.Format("{0:D2}", t.Seconds);
            }

            return message;
        }
    }
}

