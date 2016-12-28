using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Replaces the use of the default single tag system in Unity. Supports multi-tagging.
    /// </summary>
    public class Taggable : MonoBehaviour
    {
        /// <summary>
        /// List of all taggables, used for quick lookup of all objects that match a tag.
        /// </summary>
        public static List<Taggable> taggables = new List<Taggable>();

        /// <summary>
        /// Tags for specific object.
        /// </summary>
        public string tags = "";

        /// <summary>
        /// Adds object to list of taggable objects for quick lookup.
        /// </summary>
        public void Awake()
        {
            if (taggables != null)
            {
                taggables.Add(this);
            }
        }

        /// <summary>
        /// Returns all objects that have the matching tags.
        /// </summary>
        /// <param name="tags">Tags to match.</param>
        /// <returns>Objects that match the tags.</returns>
        public static List<GameObject> GetObjectsWithTags(string[] tags)
        {
            CleanUp();

            var tList = taggables.FindAll(p => p.gameObject.HasTags(tags));
            var oList = new List<GameObject>();
            for (int i = 0; i < tList.Count; i++)
            {
                oList.Add(tList[i].gameObject);
            }

            return oList;
        }

        /// <summary>
        /// Remove all objects in the taggables list that no longer exist.
        /// </summary>
        public static void CleanUp()
        {
            //Cleanup any null objects
            var cleanup = taggables.FindAll(p => p == null);
            for (int i = 0; i < cleanup.Count; i++)
            {
                taggables.Remove(cleanup[i]);
            }
            LogTool.LogDebug("Taggables Deleted: " + cleanup.Count);
        }
    }
}
