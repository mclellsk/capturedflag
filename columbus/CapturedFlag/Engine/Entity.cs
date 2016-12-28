using System.Collections.Generic;
using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Entity is a base component of almost any game object. It contains information regarding the ownership of the game object
    /// and the faction that an entity belongs to. 
    /// </summary>
    public class Entity : Actor, IIdentifiable
    {
        /* Source is passed to TakeDamage() method in target actor
         * TakeDamage() checks to see if:
         * 1. Friendly Fire is on (allow damage regardless of all other information)
         * 2. If owner of target is not the same as the owner of the source
         * 3. Are factions different?
         * 4. Are factions enemies?
         */

        /// <summary>
        /// Entity identification containing who it belongs to and which factions it belongs to.
        /// </summary>
        [HideInInspector]
        public Identification myID = new Identification(0, 0);

        public Identification MyID
        {
            get { return myID; }
        }

        /// <summary>
        /// Copy the identification provided.
        /// </summary>
        /// <param name="id"></param>
        public void CopyID(Identification id)
        {
            this.myID.playerID = id.playerID;
            this.myID.factionID = id.factionID;
        }

        /// <summary>
        /// Find entities belonging to a specific player.
        /// </summary>
        /// <param name="playerID">Player to look for.</param>
        /// <returns>List of entities that match the criteria.</returns>
        public static List<Entity> FindByPlayer(int playerID)
        {
            List<Entity> objects = new List<Entity>();
            var list = FindObjectsOfType<Entity>();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].myID.playerID == playerID)
                {
                    objects.Add(list[i]);
                }
            }
            return objects;
        }

        /// <summary>
        /// Find entities by their faction.
        /// </summary>
        /// <param name="factionID">Faction to look for.</param>
        /// <returns>List of entities that match the criteria.</returns>
        public static List<Entity> FindByFaction(int factionID)
        {
            List<Entity> objects = new List<Entity>();
            var list = FindObjectsOfType<Entity>();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].myID.factionID == factionID)
                {
                    objects.Add(list[i]);
                }
            }
            return objects;
        }
    }
}

