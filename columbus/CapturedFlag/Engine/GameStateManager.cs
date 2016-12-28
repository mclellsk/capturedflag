using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// This is used to manage the game state controllers. Only one state controller can be active at a time.
    /// </summary>
    public class GameStateManager : MonoBehaviour
    {
        /// <summary>
        /// Active instance of GameStateManager.
        /// </summary>
        public static GameStateManager instance;

        /// <summary>
        /// Determines if the manager has been flagged to change states.
        /// </summary>
        private static bool _bChange = false;

        /// <summary>
        /// The initial state of the manager.
        /// </summary>
        public GameStateController initialState;

        /// <summary>
        /// Current state of the manager.
        /// </summary>
        public GameStateController currentState;

        /// <summary>
        /// The state in the queue to be pushed to the new current state.
        /// </summary>
        private static GameStateController _nextState;

        /// <summary>
        /// Game states available.
        /// </summary>
        public static Dictionary<string, GameStateController> states = new Dictionary<string, GameStateController>();

        public void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            states.Clear();
            var s = FindObjectsOfType<GameStateController>();
            for (int i = 0; i < s.Length; i++)
            {
                states.Add(s[i].StateID, s[i]);
            }

            if (initialState != null)
            {
                ChangeState(initialState.StateID);
            }
        }

        /// <summary>
        /// Change the current state to the new state. Ignores the call if the new state and the old state are the same.
        /// </summary>
        /// <param name="stateid">State to change to.</param>
        public static void ChangeState(string stateid)
        {
            var state = states[stateid];
            if (state != null)
            {
                #if UNITY_EDITOR
                LogTool.LogDebug("Game State Changed: " + state.ToString());
                #endif

                if (state != instance.currentState)
                {
                    _nextState = state;
                    if (instance.currentState != null)
                    {
                        instance.currentState.Exit();
                    }

                    _bChange = true;
                }
            }
        }

        public void Update()
        {
            //Load next state if one exists, and the change flag is true
            if (_bChange)
            {
                if (currentState != null)
                {
                    if (currentState.GetSubState == GameStateController.SubState.IDLE)
                    {
                        if (_nextState != null)
                        {
                            currentState = _nextState;
                            currentState.Enter();
                            _nextState = null;
                            _bChange = false;
                        }
                    }
                }
                else if (_nextState != null)
                {
                    currentState = _nextState;
                    currentState.Enter();
                    _nextState = null;
                    _bChange = false;
                }
            }
        }
    }
}
