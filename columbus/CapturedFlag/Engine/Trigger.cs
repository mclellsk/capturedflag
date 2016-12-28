using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : Actor
    {
        /// <summary>
        /// List of other triggers which activate this trigger on enter.
        /// </summary>
        public List<Trigger> childTriggers = new List<Trigger>();
        /// <summary>
        /// List of game object tags that trigger on enter.
        /// </summary>
        public List<string> tags = new List<string>();
        /// <summary>
        /// Determines if the trigger can be fired infinitely.
        /// </summary>
        public bool isRepeating = false;
        /// <summary>
        /// Determines if the triggers related to this trigger share the same isTriggered flag. Sharing the same flag does not mean
        /// that the ActivateTrigger function is called for the child when the isTriggered flag is set to true even though the parent 
        /// trigger has called ActivateTrigger. It simply ensures that ActivateTrigger does nothing for all child triggers the next time 
        /// it is called unless repeating triggers is allowed.
        /// </summary>
        public bool isShared = false;
        /// <summary>
        /// Determines if the trigger has already been triggered.
        /// </summary>
        public bool isTriggered = false;

        /// <summary>
        /// Trigger activated callback (external activation).
        /// </summary>
        public event EventCallbacks.ColliderHandler OnTriggerActivated;
        /// <summary>
        /// Trigger entered callback. Also called when a child trigger is activated.
        /// </summary>
        public event EventCallbacks.ColliderHandler OnTriggerEntered;
        /// <summary>
        /// Trigger exited callback.
        /// </summary>
        public event EventCallbacks.ColliderHandler OnTriggerExited;

        public override void Awake()
        {
            base.Awake();

            for (int i = 0; i < childTriggers.Count; i++)
            {
                if (childTriggers[i] != null)
                {
                    childTriggers[i].tags = tags;
                    childTriggers[i].AddEnterListener(ActivateTrigger);
                }
            }
        }

        /// <summary>
        /// This trigger is fired by itself or any child triggers which have an OnTriggerEnter event.
        /// </summary>
        /// <param name="collider"></param>
        private void ActivateTrigger(Collider collider)
        {
            if (!isRepeating && isTriggered)
                return;

            if (isRepeating)
                isTriggered = false;

            if (!isTriggered)
            {
                if (tags.Count > 0)
                {           
                    if (collider.gameObject.HasOneTag(tags.ToArray()))
                    {
                        isTriggered = true;
                    }
                }
                else
                {
                    isTriggered = true;
                }

                if (OnTriggerActivated != null)
                {
                    OnTriggerActivated(collider);
                }

                if (isShared)
                {
                    foreach (Trigger t in childTriggers)
                        t.isTriggered = true;
                }
            }
        }

        /// <summary>
        /// Enter trigger event, activates trigger.
        /// </summary>
        /// <param name="collider"></param>
        public override void OnTriggerEnter(Collider collider)
        {
            base.OnTriggerEnter(collider);

            if (OnTriggerEntered != null)
                OnTriggerEntered(collider);

            ActivateTrigger(collider);
        }
        
        public override void OnTriggerExit(Collider collider)
        {
            base.OnTriggerExit(collider);

            if (OnTriggerExited != null)
                OnTriggerExited(collider);
        }

        public void AddEnterListener(EventCallbacks.ColliderHandler listener)
        {
            OnTriggerEntered += listener;
        }

        public void RemoveEnterListener(EventCallbacks.ColliderHandler listener)
        {
            OnTriggerEntered -= listener;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            for (int i = 0; i < childTriggers.Count; i++)
            {
                if (childTriggers[i] != null)
                {
                    childTriggers[i].RemoveEnterListener(ActivateTrigger);
                }
            }
        }
    }
}
