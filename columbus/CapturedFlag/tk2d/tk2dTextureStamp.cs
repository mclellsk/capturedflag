using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Script extender for TextureStamp methods with support for ToolKit2D library.
    /// </summary>
    public static class tk2dTextureStamp
    {
        /// <summary>
        /// Extends TextureStamp.Stamp specifically for tk2dBaseSprites.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="oldColor"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
        public static Texture2D StampSprite(this tk2dBaseSprite sprite, Color oldColor, Color newColor)
        {
            Texture2D texture = (Texture2D)MonoBehaviour.Instantiate(sprite.GetCurrentSpriteDef().material.mainTexture);

            if (texture != null)
            {
                if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial != null)
                {
                    if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture != texture)
                    {
                        sprite.gameObject.GetComponent<Renderer>().material.mainTexture = texture;
                    }
                }

                texture.Stamp(oldColor, newColor);
            }

            return texture;
        }

        /// <summary>
        /// Extends TextureStamp.StampExclude specifically for tk2dBaseSprites.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="excludeColor"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
        public static Texture2D StampSpriteExclude(this tk2dBaseSprite sprite, Color excludeColor, Color newColor)
        {
            Texture2D texture = (Texture2D)MonoBehaviour.Instantiate(sprite.GetCurrentSpriteDef().material.mainTexture);

            if (texture != null)
            {
                if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial != null)
                {
                    if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture != texture)
                    {
                        sprite.gameObject.GetComponent<Renderer>().material.mainTexture = texture;
                    }
                }

                texture.StampExclude(excludeColor, newColor);
            }

            return texture;
        }

        /// <summary>
        /// Extends TextureStamp.StampExclude specifically for tk2dBaseSprites.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="maxAlpha"></param>
        /// <param name="newColor"></param>
        /// <returns></returns>
        public static Texture2D StampSpriteAlpha(tk2dBaseSprite sprite, float maxAlpha, Color newColor)
        {
            Texture2D texture = (Texture2D)MonoBehaviour.Instantiate(sprite.GetCurrentSpriteDef().material.mainTexture);

            if (texture != null)
            {
                if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial != null)
                {
                    if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture != texture)
                    {
                        sprite.gameObject.GetComponent<Renderer>().material.mainTexture = texture;
                    }
                }

                texture.StampAlpha(maxAlpha, newColor);
            }

            return texture;
        }

        /// <summary>
        /// It's important that the specific sprite in the sheet is set before the Texture2D is changed.
        /// It does not work correctly if the sprite in the sheet is changed after the Texture2D has been 
        /// changed.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="texture"></param>
        public static void SetTexture(this tk2dBaseSprite sprite, Texture2D texture)
        {
            if (texture != null)
            {
                if (sprite.gameObject.GetComponent<Renderer>().sharedMaterial != null)
                {
                    sprite.gameObject.GetComponent<Renderer>().material.mainTexture = texture;
                }
            }
        }
    }
}
