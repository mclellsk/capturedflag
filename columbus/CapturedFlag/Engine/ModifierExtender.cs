using System.Collections.Generic;
using CapturedFlag.Engine.XML;

namespace CapturedFlag.Engine
{
    // <summary>
    /// Extender for the Modifier class.
    /// </summary>
    public static class ModifierExtender
    {
        public static void MergeAll<T>(this List<Modifier<T>> self, List<Modifier<T>> other) where T : BaseType
        {
            for (int i = 0; i < self.Count; i++)
            {
                var propertyid = self[i].id.ID;
                var mergeProperty = other.FindAll(p => p.id.ID == propertyid);
                for (int j = 0; j < mergeProperty.Count; j++)
                    self[i].Merge(mergeProperty[j]);
            }
        }

        public static void SetAll<T>(this List<Modifier<T>> self, List<Modifier<T>> other) where T : BaseType
        {
            for (int i = 0; i < self.Count; i++)
            {
                var propertyid = self[i].id.ID;
                var mergeProperty = other.FindAll(p => p.id.ID == propertyid);
                for (int j = 0; j < mergeProperty.Count; j++)
                    self[i].Set(mergeProperty[j]);
            }
        }

        /// <summary>
        /// Update values of the modifier of type T with the given id, if one doesn't exist add it to the 
        /// list of modifiers.
        /// </summary>
        /// <typeparam name="T">BaseID type.</typeparam>
        /// <param name="self">List of modifiers.</param>
        /// <param name="other">Modifier to merge.</param>
        public static void Merge<T>(this List<Modifier<T>> self, Modifier<T> other) where T : BaseType
        {
            var modifier = self.Find(p => p.id.ID == other.id.ID);
            if (modifier != null)
                modifier.Merge(other);
            else
            {
                self.Add(new Modifier<T>(other.id, other.Value, other.Min, other.Max));
            }
        }

        public static void Set<T>(this List<Modifier<T>> self, Modifier<T> other) where T : BaseType
        {
            var modifier = self.Find(p => p.id.ID == other.id.ID);
            if (modifier != null)
                modifier.Set(other);
            else
            {
                self.Add(new Modifier<T>(other.id, other.Value, other.Min, other.Max));
            }
        }
    }
}