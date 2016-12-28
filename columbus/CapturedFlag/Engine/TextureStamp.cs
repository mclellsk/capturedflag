using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// This class replaces specific colors in a texture with other colors. For best use call the stamp methods once at initialization, and cache the result for reference later.
    /// </summary>
    public static class TextureStamp
    {
        /// <summary>
        /// Replace any pixels with an alpha greater than the maximum specified within a texture. 
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="newColor"></param>
        /// <param name="minAlpha"></param>
        public static void StampAlpha(this Texture2D texture, float maxAlpha, Color newColor)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a >= maxAlpha / 255f)
                    pixels[i] = newColor;
            }

            texture.SetPixels(pixels);
            texture.filterMode = FilterMode.Trilinear;
            texture.Apply();
        }

        /// <summary>
        /// Replace any pixels that do not match the exclusion color.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="newColor"></param>
        /// <param name="excludeColor"></param>
        public static void StampExclude(this Texture2D texture, Color excludeColor, Color newColor)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i] != excludeColor)
                    pixels[i] = newColor;
            }

            texture.SetPixels(pixels);
            texture.Apply();
        }

        /// <summary>
        /// Replace any pixels that match the old color.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="oldColor"></param>
        /// <param name="newColor"></param>
        public static void Stamp(this Texture2D texture, Color oldColor, Color newColor)
        {
            Color[] pixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i] == oldColor)
                {
                    pixels[i] = newColor;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
        }
    }
}
