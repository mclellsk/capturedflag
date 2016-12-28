using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine.Scriptables
{
    public class ActionLookup : ScriptableObject
    {
        public List<ActionInput> actions = new List<ActionInput>();
    }
}
