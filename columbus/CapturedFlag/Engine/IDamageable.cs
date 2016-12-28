using System.Collections;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Implemented by any object that expects to take damage.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Hit by damage callback.
        /// </summary>
        event Health.OnHitHandler OnHit;
        /// <summary>
        /// Dying callback.
        /// </summary>
        event Health.OnDyingHandler OnDying;
        /// <summary>
        /// First update of dying state callback.
        /// </summary>
        event System.Action OnBeginDying;

        /// <summary>
        /// Health types that belong to this object. Order represents the hierarchy, first in the list is
        /// of lowest importance and is reduced first.
        /// </summary>
        List<Health> HealthTypes { get; set; }
        /// <summary>
        /// Damage that is currently being taken, cooldown timers before damage from specific instance
        /// can be taken again.
        /// </summary>
        List<DamageInstance> DamageTaken { get; set; }
        /// <summary>
        /// Damage over time effects that are currently active on the object.
        /// </summary>
        List<DamageInstance> DamageTicks { get; set; }

        /// <summary>
        /// Overall health state of the object.
        /// </summary>
        Health.HealthState HealthState { get; set; }

        /// <summary>
        /// Time to wait until the object reaches the dead state after entering the dying state.
        /// </summary>
        float TimeUntilDead { get; }
        /// <summary>
        /// Critical chance bonus on self.
        /// </summary>
        float CritChanceOnSelf { get; }
        /// <summary>
        /// Critical damage bonus on self.
        /// </summary>
        float CritBonusOnSelf { get; }

        /// <summary>
        /// Determines if the object can take damage or if its invulnerable.
        /// </summary>
        bool CanTakeDamage { get; set; }
        /// <summary>
        /// Determines if the object can die.
        /// </summary>
        bool CanDie { get; set; }
        /// <summary>
        /// Determines if the object is alive.
        /// </summary>
        bool IsAlive { get; }
        /// <summary>
        /// Determines if the object is dead.
        /// </summary>
        bool IsDead { get; }
        /// <summary>
        /// Determines if the object is dying.
        /// </summary>
        bool IsDying { get; }

        /// <summary>
        /// Function to handle the event of a hit, after the damage calculations have been handled. 
        /// This is the result.
        /// </summary>
        /// <param name="damage">Instance of damage that hit.</param>
        /// <param name="amount">Amount of damage that was dealt.</param>
        /// <param name="bMissed">Determines if the hit was missed.</param>
        /// <param name="bCrit">Determines if the hit was a critical.</param>
        void Hit(DamageInstance damage, int amount, bool bMissed, bool bCrit);

        /// <summary>
        /// Apply initial damage based on a damage instance object, checking for damage over time effects as well.
        /// </summary>
        /// <param name="damage">Instance of damage to apply.</param>
        void TakeDamage(DamageInstance damage);

        /// <summary>
        /// Only apply damage of the type and specified amount. No damage over time effects are checked.
        /// </summary>
        /// <param name="damage">Instance of damage to apply.</param>
        /// <param name="amount">Amount of damage to apply.</param>
        void ApplyDamage(DamageInstance damage, int amount);

        /// <summary>
        /// Coroutine to track damage over time effects.
        /// </summary>
        /// <param name="dot">Damage over time to apply.</param>
        /// <returns></returns>
        IEnumerator TakeDamagePerTick(DamageInstance dot);

        /// <summary>
        /// Coroutine to track the cooldown between damage received of the same damage type.
        /// </summary>
        /// <param name="damage">Instance of damage to track cooldown for.</param>
        /// <returns></returns>
        IEnumerator DelayDamage(DamageInstance damage);

        /// <summary>
        /// Alive state.
        /// </summary>
        void Alive();
        /// <summary>
        /// Started dying state.
        /// </summary>
        void BeginDying();
        /// <summary>
        /// Dying state, waiting for death timer to complete.
        /// </summary>
        void Dying();
        /// <summary>
        /// Dead state.
        /// </summary>
        void Dead();
    }
}
