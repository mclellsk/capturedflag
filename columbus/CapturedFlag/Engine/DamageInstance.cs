using System;
using System.Collections.Generic;
using CapturedFlag.Engine.Scriptables;
using UnityEngine;
using Game.XML.DamageTypes;
using Game.XML.PropertyTypes;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Represents an instance of damage to be applied to an object which implements <seealso cref="IDamageable"/>.
    /// </summary>
    public class DamageInstance
    {
        /// <summary>
        /// Damage type
        /// </summary>
        public DamageType id;

        /// <summary>
        /// Unique identification string used to determine if damage instances are the same. Related
        /// to damage that can stack and time since last hit by this instance of damage. Newly initialized 
        /// instances are unique to one another.
        /// </summary>
        private string _uniqueID = "";

        /// <summary>
        /// Determines if this instance of damage applies its own damage over time effects regardless of
        /// other unique instances of damage currently applying their own damage over time effects even
        /// if both damage over time instances share the same <seealso cref="DamageID"/>.
        /// </summary>
        private bool _canStack = false;
        /// <summary>
        /// Flag to notify damage over time instance to refresh the tick counter.
        /// </summary>
        private bool _bRefresh = false;

        /// <summary>
        /// Which health id types does this damage effect.
        /// </summary>
        public List<int> healthTypes = new List<int>();

        /// <summary>
        /// Amount of damage this instance will apply on hit.
        /// </summary>
        public int damageAmount = 0;

        /// <summary>
        /// Minimum time between hits from the same DamageID.
        /// </summary>
        public float timeBetweenHits = 1f;
        /// <summary>
        /// Chance to successfully apply a hit.
        /// </summary>
        public float chanceToHit = 1f;
        /// <summary>
        /// Chance to successfully apply a critical hit.
        /// </summary>
        public float chanceToCrit = 0f;
        /// <summary>
        /// Critical hit multiplier of this instance of damage.
        /// </summary>
        public float critBonusMult = 1f;

        /// <summary>
        /// Damage over time instance to apply.
        /// </summary>
        public DamageInstance dot;
        /// <summary>
        /// Chance to successfully apply a hit of the damage over time effect if one exists.
        /// </summary>
        public float chanceToApplyDot = 1f;
        /// <summary>
        /// The number of ticks of damage that have occurred for this damage instance so far.
        /// </summary>
        public int ticks = 0;
        /// <summary>
        /// The maximum number of ticks of damage that occur.
        /// </summary>
        private int _maxTicks = 0;
        /// <summary>
        /// Time between ticks.
        /// </summary>
        private float _timeBetweenTicks = 0f;

        /// <summary>
        /// Properties to apply to objects that accept property changes.
        /// </summary>
        public List<Modifier<PropertyType>> properties = new List<Modifier<PropertyType>>();
        /// <summary>
        /// Multiplicative resistance effects.
        /// </summary>
        public List<Modifier<DamageType>> resistMult = new List<Modifier<DamageType>>();
        /// <summary>
        /// Additive resistance effects.
        /// </summary>
        public List<Modifier<DamageType>> resistAdd = new List<Modifier<DamageType>>();
        /// <summary>
        /// Object sending damage.
        /// </summary>
        public Entity sender;

        public string UniqueID
        {
            get { return _uniqueID; }
        }

        public bool CanStack
        {
            get { return _canStack; }
        }
        public bool bRefresh
        {
            get { return _bRefresh; }
        }

        public float TimeBetweenTicks
        {
            get { return _timeBetweenTicks; }
        }

        public int MaxTicks
        {
            get { return _maxTicks; }
        }

        public void FlagRefresh()
        {
            _bRefresh = true;
        }
        public void Refresh()
        {
            ticks = 0;
            _bRefresh = false;
        }

        /// <summary>
        /// Useful for when you need all the properties of the damage instance, and want to reuse the unique id (which is linked to individual entities generally).
        /// It creates a completely new instance of the DamageInstance with the same unique id, this way when ticks are refreshed in the event of a damage-over-time 
        /// instance, all other dots with the same unique id are not refreshed across all actors that reference this damage instance. If you do not use this, seeing as 
        /// dots are passed by reference, refreshing a dot on one actor may unintentionally refresh all dots of the same type on completely separate actors, causing their lifespan
        /// to change abnormally.
        /// </summary>
        /// <param name="instance">The instance to copy the properties from including the unique id.</param>
        /// <returns>A new instance with all the same properties as the original.</returns>
        public static DamageInstance GetCopy(DamageInstance instance)
        {
            DamageInstance newInstance = new DamageInstance();
            newInstance.Copy(instance);
            return newInstance;
        }

        private void Copy(DamageInstance instance)
        {
            this._uniqueID = instance.UniqueID;
            this.id = instance.id;
            this.healthTypes = instance.healthTypes;
            this.properties = instance.properties;
            this.resistMult = instance.resistMult;
            this.resistAdd = instance.resistAdd;
            this.damageAmount = instance.damageAmount;
            this.chanceToHit = instance.chanceToHit;
            this.chanceToCrit = instance.chanceToCrit;
            this.critBonusMult = instance.critBonusMult;
            this.timeBetweenHits = instance.timeBetweenHits;
            this._canStack = instance.CanStack;
            this._maxTicks = instance.MaxTicks;
            this._timeBetweenTicks = instance.TimeBetweenTicks;
            if (instance.dot != null)
            {
                DamageInstance newDot = new DamageInstance();
                newDot.Copy(instance.dot);
                this.dot = newDot;
            }
            this.chanceToApplyDot = instance.chanceToApplyDot;
        }

        public DamageInstance() { }

        public DamageInstance ( DamageType id, 
                                List<int> healthTypes,
                                List<Modifier<PropertyType>> properties,
                                List<Modifier<DamageType>> resistMult,
                                List<Modifier<DamageType>> resistAdd,
                                int damageAmount,
                                float chanceToHit = 0f,
                                float chanceToCrit = 0f,
                                float critBonusMult = 1f,
                                float timeBetweenHits = 0f,
                                bool canStack = false,
                                int maxTicks = 0,
                                float timeBetweenTicks = 0f,
                                DamageInstance dot = null,
                                float chanceToApplyDot = 0f )
        {
            _uniqueID = Guid.NewGuid().ToString();
            this.id = id;
            this.healthTypes = healthTypes;
            this.properties = properties;
            this.resistMult = resistMult;
            this.resistAdd = resistAdd;
            this.damageAmount = damageAmount;
            this.chanceToHit = chanceToHit;
            this.chanceToCrit = chanceToCrit;
            this.critBonusMult = critBonusMult;
            this.timeBetweenHits = timeBetweenHits;
            _canStack = canStack;
            _timeBetweenTicks = timeBetweenTicks;
            _maxTicks = maxTicks;
            this.dot = dot;
            this.chanceToApplyDot = chanceToApplyDot;
        }
    }
}

