using UnityEngine;

namespace CapturedFlag.Engine.Scriptables
{
    /// <summary>
    /// ScriptableObject which holds a unique id.
    /// </summary>
    public abstract class BaseID : ScriptableObject
    {
        public int id = -1;
    }
}

