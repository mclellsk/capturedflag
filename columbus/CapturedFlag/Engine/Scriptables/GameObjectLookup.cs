using System.Collections.Generic;
using UnityEngine;

namespace CapturedFlag.Engine.Scriptables
{
    public class GameObjectLookup : ScriptableObject
    {
        public List<SerializedPair.GameObject> lookup = new List<SerializedPair.GameObject>();
    }
}
