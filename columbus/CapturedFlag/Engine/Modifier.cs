using System.Collections.Generic;
using UnityEngine;
using CapturedFlag.Engine.Scriptables;
using CapturedFlag.Engine.XML;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// A property that has a value and is tracked by an ID scriptable object.
    /// </summary>
    public class Modifier<T> where T : BaseType
    {
        /// <summary>
        /// Property type.
        /// </summary>
        public T id;

        /// <summary>
        /// Value of property.
        /// </summary>
        private float _value;

        /// <summary>
        /// Maximum value allowed for property.
        /// </summary>
        private float _max;
        /// <summary>
        /// Minimum value allowed for property.
        /// </summary>
        private float _min;

        public Modifier(T id, float value = 0f, float min = 0f, float max = 1f)
        {
            this.id = id;
            this._min = min;
            this._max = max;
            this._value = value;
        }

        public void Merge(Modifier<T> other)
        {
            _value += other.Value;
            if (_value > _max)
                _value = _max;
            else if (_value < _min)
                _value = _min;
        }

        public void Set(Modifier<T> other)
        {
            _value = other.Value;
            if (_value > _max)
                _value = _max;
            else if (_value < _min)
                _value = _min;
        }

        public float Value
        {
            get { return _value; }
        }

        public float Max
        {
            get
            {
                return _max;
            }
        }
        public float Min
        {
            get
            {
                return _min;
            }
        }
    }
}
