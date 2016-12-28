using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapturedFlag.Engine
{
    [Serializable]
    public class ActionInput
    {
        /// <summary>
        /// Name of the action. Used for lookup purposes.
        /// </summary>
        public string name = "";
        /// <summary>
        /// Keys that correspond to the action.
        /// </summary>
        public List<KeyCode> keys = new List<KeyCode>();
        /// <summary>
        /// Buttons that correspond to the action.
        /// </summary>
        public List<int> buttons = new List<int>();

        public ActionInput(string name, List<KeyCode> keys, List<int> buttons)
        {
            this.name = name;
            this.keys = keys;
            this.buttons = buttons;
        }
    }
}
