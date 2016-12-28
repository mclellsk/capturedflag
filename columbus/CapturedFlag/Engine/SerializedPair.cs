using System;
using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Exposes really generic key value pairs where the key is a string. Really unnecessary to create a new class everytime you want
    /// to expose a simple key value pair to the inspector.
    /// </summary>
    public class SerializedPair
    {
        [Serializable]
        public class String
        {
            public string key;
            public string value;
        }

        [Serializable]
        public class GameObject
        {
            public string key;
            public UnityEngine.GameObject value;
        }

        [Serializable]
        public class Integer
        {
            public string key;
            public int value;
        }

        [Serializable]
        public class Float
        {
            public string key;
            public float value;
        }
    }
}
