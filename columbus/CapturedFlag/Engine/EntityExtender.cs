using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    public static class EntityExtender
    {
        public static List<Entity> FindByPlayer(this List<Entity> list, int playerID)
        {
            List<Entity> objects = new List<Entity>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].MyID.playerID == playerID)
                {
                    objects.Add(list[i]);
                }
            }
            return objects;
        }

        public static List<Entity> FindByFaction(this List<Entity> list, int factionID)
        {
            List<Entity> objects = new List<Entity>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].MyID.factionID == factionID)
                {
                    objects.Add(list[i]);
                }
            }
            return objects;
        }

        /// <summary>
        /// Determines if the target is valid based on whether the entity's standing is under or over the standing listed.
        /// When friendly is flagged, the standing represents the highest limit before being considered a friend, otherwise the standing represents
        /// the lowest limit before being considered an enemy.
        /// </summary>
        /// <param name="self">Entity that target is checked against.</param>
        /// <param name="target">Target being checked against entity.</param>
        /// <param name="standing">Minimum or Maximum standing required to pass check.</param>
        /// <param name="friendly">Flags the check for friend or enemy.</param>
        /// <param name="bSelf">Flags the target as valid even if belonging to the same playerID.</param>
        /// <returns></returns>
        public static bool IsTarget(this Entity self, Entity target, int standing = 0, bool friendly = false, bool bSelf = false)
        {
            if (!friendly)
            {
                //Player can target any entity that is aligned as an enemy faction and itself depending on flag.
                return ((self.MyID.playerID != target.MyID.playerID) && (self.IsEnemy(target.MyID.factionID, standing))) || ((self.MyID.playerID == target.MyID.playerID) && bSelf);
            }
            else
            {
                //Player can target any entity that is aligned as an ally and itself depending on flag.
                return ((self.MyID.playerID != target.MyID.playerID) && (self.IsFriend(target.MyID.factionID, standing))) || ((self.MyID.playerID == target.MyID.playerID) && bSelf);
            }
        }

        /// <summary>
        /// Determines if the target is valid based on whether the entity's standing is within the range.
        /// </summary>
        /// <param name="self">Entity that target is checked against.</param>
        /// <param name="target">Target being checked against entity.</param>
        /// <param name="lower">The lowest standing accepted for this to be true.</param>
        /// <param name="upper">The highest standing accepted for this to be true.</param>
        /// <param name="bSelf">Flags the target as valid even if belonging to the same playerID.</param>
        /// <returns></returns>
        public static bool IsTarget(this Entity self, Entity target, int lower, int upper, bool bSelf = false)
        {
            return ((self.MyID.playerID != target.MyID.playerID) && (self.IsNeutral(target.MyID.factionID, upper, lower))) || ((self.MyID.playerID == target.MyID.playerID) && bSelf);
        }
    }
}