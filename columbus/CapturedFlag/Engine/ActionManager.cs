using UnityEngine;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using CapturedFlag.Engine.Scriptables;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Contains and manages all controls for the game. Uses names to track input instead of individual buttons/key presses.
    /// Allows multiple input types to trigger the same action. Allows for input changes at runtime.
    /// </summary>
    public class ActionManager : MonoBehaviour
    {
        /// <summary>
        /// Instance of ActionManager.
        /// </summary>
        public static ActionManager instance;

        /// <summary>
        /// Control scheme containing all actions and their associated inputs.
        /// </summary>
        [Serializable]
        public class StoredActions
        {
            [OptionalField]
            public List<Schemes> schemes = new List<Schemes>();

            private void SetDefault(StreamingContext sc)
            {
                if (schemes == null)
                {
                    schemes = new List<Schemes>();
                }
            }
        }

        /// <summary>
        /// Controls for specific scheme (ex. Car, Movement, Plane)
        /// </summary>
        [Serializable]
        public class Schemes
        {
            [OptionalField]
            public string name;

            [OptionalField]
            public List<ActionInput> actions = new List<ActionInput>();

            private void SetDefault(StreamingContext sc)
            {
                if (name == null)
                {
                    name = "";
                }
                if (actions == null)
                {
                    actions = new List<ActionInput>();
                }
            }
        }

        /// <summary>
        /// Saved controls.
        /// </summary>
        public static StoredActions controls = new StoredActions();

        public class ControlEvents
        {
            public string schemeName = "";
            public List<ActionEvents> actionEvents = new List<ActionEvents>();
        }

        /// <summary>
        /// Control events populated from the saved controls file. Tracks additional input events, like time being held.
        /// </summary>
        public static List<ControlEvents> controlEvents = new List<ControlEvents>();

        /// <summary>
        /// List of all key codes in the form of a string.
        /// </summary>
        public List<KeyCode> keyCodes = new List<KeyCode>();

        /// <summary>
        /// File name to store control scheme.
        /// </summary>
        private static readonly string _fileName = "controls.sav";

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else
                Destroy(this);

            GetKeys();

            try
            {
                Load();

                //If the previously saved control scheme has a descrepency in the number of actions when compared
                //to the default control scheme, simply load the defaults.
                if (controls.schemes.Count != Global.controls.Schemes.Count)
                {
                    LoadDefaults();
                }
            }
            catch
            {
                LoadDefaults();   
            }

            Refresh();
        }

        /// <summary>
        /// Save controls.
        /// </summary>
        public static void Save()
        {
            DataSerializer.SaveBinary(controls, _fileName);
        }

        /// <summary>
        /// Load controls.
        /// </summary>
        public static void Load()
        {
            controls = (StoredActions)DataSerializer.LoadBinary(_fileName);
        }

        public void GetKeys()
        {
            var keys = (KeyCode[])Enum.GetValues(typeof(KeyCode));
            for (int i = 0; i < keys.Length; i++)
            {
                keyCodes.Add(keys[i]);
                //Debug.Log(keys[i].ToString());
            }
        }

        public KeyCode GetKey(string key)
        {
            return keyCodes.Find(p => p.ToString() == key);
        }

        /// <summary>
        /// Load default control scheme.
        /// </summary>
        public void LoadDefaults()
        {
            //Load default action controls
            var schemeList = Global.controls.Schemes;
            for (int k = 0; k < schemeList.Count; k++)
            {
                if (schemeList[k] != null)
                {
                    var scheme = new Schemes();
                    scheme.name = schemeList[k].Name;
                    for (int i = 0; i < schemeList[k].Actions.Count; i++)
                    {
                        var keys = new List<KeyCode>();
                        for (int j = 0; j < schemeList[k].Actions[i].Keys.Count; j++)
                        {
                            keys.Add(GetKey(schemeList[k].Actions[i].Keys[j].ID));
                        }
                        var buttons = new List<int>();
                        for (int j = 0; j < schemeList[k].Actions[i].Buttons.Count; j++)
                        {
                            buttons.Add(schemeList[k].Actions[i].Buttons[j].ID);
                        }
                        var action = new ActionInput(schemeList[k].Actions[i].Name, keys, buttons);
                        scheme.actions.Add(action);
                    }       
                    controls.schemes.Add(scheme);
                }
            }

            Save();
        }

        /// <summary>
        /// Refresh all controls.
        /// </summary>
        public static void Refresh()
        {
            for (int i = 0; i < controls.schemes.Count; i++)
            {
                var scheme = controlEvents.Find(p => p.schemeName == controls.schemes[i].name);
                if (scheme != null)
                {
                    //If control events already exist, simply update them.
                    for (int j = 0; j < controls.schemes[i].actions.Count; j++)
                    {
                        var action = scheme.actionEvents.Find(p => p.action.name == controls.schemes[i].actions[j].name);
                        if (action != null)
                        {
                            action.action.keys = controls.schemes[i].actions[j].keys;
                            action.action.buttons = controls.schemes[i].actions[j].buttons;
                        }
                        else
                        {
                            scheme.actionEvents.Add(new ActionEvents(controls.schemes[i].actions[j].name, controls.schemes[i].actions[j].keys, controls.schemes[i].actions[j].buttons));
                        }
                    }
                }
                else
                {
                    //Generate list of control events.
                    var newScheme = new ControlEvents();
                    newScheme.schemeName = controls.schemes[i].name;
                    for (int j = 0; j < controls.schemes[i].actions.Count; j++)
                    {
                        newScheme.actionEvents.Add(new ActionEvents(controls.schemes[i].actions[j].name, controls.schemes[i].actions[j].keys, controls.schemes[i].actions[j].buttons));
                    }
                    controlEvents.Add(newScheme);
                }
            }
        }

        /// <summary>
        /// Check if action is being pressed.
        /// </summary>
        /// <param name="name">Action to check.</param>
        /// <returns>Result of check.</returns>
        public static bool GetAction(string scheme, string name)
        {
            bool result = false;

            var action = FindAction(scheme, name);

            if (action != null)
            {
                for (int i = 0; i < action.action.keys.Count; i++)
                {
                    var r = Input.GetKey(action.action.keys[i]);
                    result = result || r;
                }

                for (int i = 0; i < action.action.buttons.Count; i++)
                {
                    var r = Input.GetMouseButton(action.action.buttons[i]);
                    result = result || r;
                }

                if (result)
                {
                    action.UpdateTime();
                }
                else
                {
                    //If action is not held, clear time.
                    action.ClearTime();
                }

                return result;
            }
            else
                return false;
        }

        /// <summary>
        /// Check if action is being pressed down initially.
        /// </summary>
        /// <param name="name">Action to check.</param>
        /// <returns>Result of check.</returns>
        public static bool GetActionDown(string scheme, string name)
        {
            bool result = false;

            var action = FindAction(scheme, name);

            if (action != null)
            {
                for (int i = 0; i < action.action.keys.Count; i++)
                {
                    var r = Input.GetKeyDown(action.action.keys[i]);
                    result = result || r;
                }

                for (int i = 0; i < action.action.buttons.Count; i++)
                {
                    var r = Input.GetMouseButtonDown(action.action.buttons[i]);
                    result = result || r;
                }

                if (result)
                {
                    action.UpdateTime();
                }

                return result;
            }
            else
                return false;
        }

        /// <summary>
        /// Check if action is being released finally.
        /// </summary>
        /// <param name="name">Action to check.</param>
        /// <returns>Result of check.</returns>
        public static bool GetActionUp(string scheme, string name)
        {
            bool result = false;

            var action = FindAction(scheme, name);

            if (action != null)
            {
                for (int i = 0; i < action.action.keys.Count; i++)
                {
                    var r = Input.GetKeyUp(action.action.keys[i]);
                    result = result || r;
                }

                for (int i = 0; i < action.action.buttons.Count; i++)
                {
                    var r = Input.GetMouseButtonUp(action.action.buttons[i]);
                    result = result || r;
                }

                if (result)
                {
                    action.ClearTime();
                }

                return result;
            }
            else
                return false;
        }

        /// <summary>
        /// Find action by name.
        /// </summary>
        /// <param name="name">Name of action.</param>
        /// <returns>ActionEvent object associated with name.</returns>
        public static ActionEvents FindAction(string schemeName, string name)
        {
            if (controlEvents != null)
            {
                var scheme = controlEvents.Find(p => p.schemeName == schemeName);
                if (scheme != null)
                {
                    var action = scheme.actionEvents.Find(p => p.action.name == name);
                    if (action != null)
                    {
                        return action;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Changes the key controls of an action.
        /// </summary>
        /// <remarks>
        /// After all controls have been changed, you need to call Save() followed by Refresh().
        /// </remarks>
        /// <param name="name">Action name.</param>
        /// <param name="keys">Keys to control action.</param>
        public static void ChangeControl(string schemeName, string name, List<KeyCode> keys)
        {
            var scheme = controls.schemes.Find(p => p.name == schemeName);
            if (scheme != null)
            {
                var action = scheme.actions.Find(p => p.name == name);
                if (action != null)
                {
                    action.keys = keys;
                }
            }
        }

        /// <summary>
        /// Changes the button controls of an action.
        /// </summary>
        /// <remarks>
        /// After all controls have been changed, you need to call Save() followed by Refresh().
        /// </remarks>
        /// <param name="name">Action name.</param>
        /// <param name="buttons">Buttons to control action.</param>
        public static void ChangeControl(string schemeName, string name, List<int> buttons)
        {
            var scheme = controls.schemes.Find(p => p.name == schemeName);
            if (scheme != null)
            {
                var action = scheme.actions.Find(p => p.name == name);
                if (action != null)
                {
                    action.buttons = buttons;
                }
            }
        }
    }
}

