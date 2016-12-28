using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Tags separated by a delimiter.
    /// </summary>
    public static class MultiTag
    {
        public const char DELIMITER = ',';

        /// <summary>
        /// Set the tags of the current GameObject, concatenated with the delimiter.
        /// </summary>
        /// <typeparam name="GameObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tags"></param>
        public static void SetTags(this UnityEngine.GameObject obj, params string[] tags)
        {
            var tag = "";
            for (int i = 0; i < tags.Length; i++)
            {
                tag += tags[i];

                if (i >= 0 && i < (tags.Length - 1))
                {
                    tag += DELIMITER;
                }
            }

            var taggable = obj.GetComponent<Taggable>();
            if (taggable != null)
            {
                taggable.tags = tag;
            }
            else
            {
                var newTaggable = obj.AddComponent<Taggable>();
                newTaggable.tags = tag;
            }
        }

        /// <summary>
        /// Adds tags to the current set of tags on the object.
        /// </summary>
        /// <param name="obj">Object to add tags to.</param>
        /// <param name="tags">Tags to add.</param>
        public static void AddTags(this UnityEngine.GameObject obj, params string[] tags)
        {
            var oldTags = GetTags(obj);
            string[] newTags = new string[oldTags.Count + tags.Length];
            for (int i = 0; i < oldTags.Count; i++)
            {
                newTags[i] = oldTags[i];
            }
            for (int i = oldTags.Count; i < (oldTags.Count + tags.Length); i++)
            {
                newTags[i] = tags[(oldTags.Count + tags.Length) - 1 - i];
            }
            SetTags(obj, newTags);
        }

        /// <summary>
        /// Get the tags of the current GameObject in a list, separated using the delimiter.
        /// </summary>
        /// <typeparam name="GameObject"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<string> GetTags(this UnityEngine.GameObject obj)
        {
            var tagList = new List<string>();

            var taggable = obj.GetComponent<Taggable>();
            if (taggable != null)
            {
                var tags = taggable.tags.Split(DELIMITER);
                for (int i = 0; i < tags.Length; i++)
                {
                    tagList.Add(tags[i]);
                }
            }

            return tagList;
        }

        /// <summary>
        /// Compare the tags in the string array to the tags of the current GameObject and return the
        /// list of strings which overlap the both sets.
        /// </summary>
        /// <typeparam name="GameObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static List<string> GetMatchingTags(this UnityEngine.GameObject obj, params string[] tags)
        {
            var tagList = new List<string>();

            var taggable = obj.GetComponent<Taggable>();
            if (taggable != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    if (taggable.tags.Contains(tags[i]))
                    {
                        tagList.Add(tags[i]);
                    }
                }
            }

            return tagList;
        }

        /// <summary>
        /// Compares the tags in the string array to the tags of the current GameObject and 
        /// returns true if all tags in the string array are present in the GameObject.
        /// </summary>
        /// <typeparam name="GameObject"></typeparam>
        /// <param name="obj"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static bool HasTags(this UnityEngine.GameObject obj, params string[] tags)
        {
            bool bHasTags = true;

            var taggable = obj.GetComponent<Taggable>();
            if (taggable != null)
            {
                if (taggable.tags != System.String.Empty)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (!taggable.tags.Contains(tags[i]))
                        {
                            bHasTags = false;
                            break;
                        }
                    }
                }
                else
                    bHasTags = false;
            }

            return bHasTags;
        }

        /// <summary>
        /// Checks to see if the object has at least one of the tags listed.
        /// </summary>
        /// <param name="obj">Object to check tags on.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns></returns>
        public static bool HasOneTag(this UnityEngine.GameObject obj, params string[] tags)
        {
            var bHasTag = false;

            var taggable = obj.GetComponent<Taggable>();
            if (taggable != null)
            {
                if (taggable.tags != System.String.Empty)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (taggable.tags.Contains(tags[i]))
                        {
                            bHasTag = true;
                            break;
                        }
                    }
                }
            }

            return bHasTag;
        }
    }
}
