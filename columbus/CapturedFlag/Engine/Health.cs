using System.Collections.Generic;
using CapturedFlag.Engine.Scriptables;
using Game.XML.DamageTypes;
using Game.XML.HealthTypes;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// This contains everything required to monitor and deliver damage to a target. Objects can have
    /// multiple health declarations associated to themselves, this sets everything up in a way that allows 
    /// damage to overflow from one health source to the next, to treat damage differently per health source,
    /// and to apply a specific health hierarchy.
    /// </summary>
    public class Health
    {
        /// <summary>
        /// Health state of IDamageable.
        /// </summary>
        public enum HealthState
        {
            ALIVE,
            DYING,
            DEAD
        };

        /// <summary>
        /// Health type.
        /// </summary>
        public HealthType id;

        /// <summary>
        /// Delegate for when the IDamageable is hit.
        /// </summary>
        /// <param name="damage">Instance of damage to apply.</param>
        /// <param name="amount">Amount of damage to apply.</param>
        /// <param name="bMissed">Determines if the hit was missed.</param>
        /// <param name="bCrit">Determines if the hit was a critical.</param>
        public delegate void OnHitHandler(DamageInstance damage, int amount, bool bMissed, bool bCrit);
        /// <summary>
        /// Delegate for when the IDamageable is dying.
        /// </summary>
        /// <param name="damage">Instance of damage to apply.</param>
        /// <param name="health">Amount of health left.</param>
        public delegate void OnDyingHandler(DamageInstance damage, int health);

        /// <summary>
        /// Maximum health amount changed callback.
        /// </summary>
        public event System.Action OnMaxAmountChange;

        /// <summary>
        /// Health remaining for this instance.
        /// </summary>
        public int amount = 100;
        /// <summary>
        /// Maximum health possible for this instance.
        /// </summary>
        private int _maxAmount = 100;
        /// <summary>
        /// Maximum health possible, with callback for changes included.
        /// </summary>
        public int MaxAmount
        {
            get { return _maxAmount; }
            set
            {
                _maxAmount = value;

                if (OnMaxAmountChange != null)
                {
                    OnMaxAmountChange();
                }
            }
        }
        /// <summary>
        /// Percentage of health remaining.
        /// </summary>
        public float Percent
        {
            get
            {
                return ((float)amount) / _maxAmount;
            }
        }

        /// <summary>
        /// Multiplicative resistances applied to this health type.
        /// </summary>
        public List<Modifier<DamageType>> resistMult = new List<Modifier<DamageType>>();
        /// <summary>
        /// Additive resistances applied to this health type.
        /// </summary>
        public List<Modifier<DamageType>> resistAdd = new List<Modifier<DamageType>>(); 

        public Health(HealthType id)
        {
            this.id = id;

            //Populate resistances with all DamageIDs.
            for (int i = 0; i < Global.damagetypes.DamageTypes.Count; i++)
            {
                resistMult.Add(new Modifier<DamageType>(Global.damagetypes.DamageTypes[i]));
                resistAdd.Add(new Modifier<DamageType>(Global.damagetypes.DamageTypes[i], 0f, 0f, 999999f));
            }
        }
    }
}
