using System.Linq;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Each object is assigned a chance to drop. If the roll value is less than or equal to the chance to drop,
    /// the object successfully drops and is returned.
    /// </summary>
    public class Chance
    {
        /// <summary>
        /// Object that contains a result to return if a successful roll occurs based on the chances given.
        /// </summary>
        public class ChanceObject
        {
            /// <summary>
            /// Result of a successful roll.
            /// </summary>
            public object result;
            /// <summary>
            /// Chance to successfully roll.
            /// </summary>
            public float chance;

            public ChanceObject(object result, float chance)
            {
                this.result = result;
                this.chance = chance;
            }
        }

        /// <summary>
        /// List of all possible results and their chances at occurring.
        /// </summary>
        public List<ChanceObject> possibilities = new List<ChanceObject>();

        public Chance(List<ChanceObject> possibilities)
        {
            this.possibilities = possibilities;
        }

        /// <summary>
        /// Randomly selects an outcome based on that outcome's chance. If empty outcomes are not allowed
        /// the most common outcome will be returned.
        /// </summary>
        /// <param name="isEmptyAllowed">Allow null outcomes.</param>
        /// <returns>Outcome selected.</returns>
        public object Roll(bool isEmptyAllowed = true)
        {
            possibilities.Shuffle();
            for (int i = 0; i < possibilities.Count; i++)
            {
                var rand = UnityEngine.Random.Range(0, 1f);
                if (rand <= possibilities[i].chance)
                {
                    return possibilities[i].result;
                }
            }

            if (!isEmptyAllowed)
            {
                var common = possibilities.OrderByDescending(p => p.chance).ToList();
                return common.First().result;
            }

            return null;
        }

        /// <summary>
        /// Rolls a specified number of times, with a minimum guaranteed number of results returning (excludes null).
        /// </summary>
        /// <param name="rolls">Number of rolls to attempt.</param>
        /// <param name="minimum">Minimum outcomes that must come back not null.</param>
        /// <returns></returns>
        public object[] Roll(int rolls, int minimum)
        {
            var minReturns = minimum;
            List<object> outcomes = new List<object>();
            for (int i = 0; i < rolls; i++)
            {
                if (i < minReturns)
                {
                    outcomes.Add(Roll(false));
                }
                else
                    outcomes.Add(Roll());
            }
            return outcomes.ToArray();
        }
    }
}
