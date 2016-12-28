using UnityEngine;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Add sound to selection of scrollable flow.
    /// </summary>
    [RequireComponent(typeof(tk2dUIScrollableFlow))]
    public class tk2dFlowFade : MonoBehaviour
    {
        /// <summary>
        /// Fade renderers alpha when not selected by flow.
        /// </summary>
        public float alpha = 0.5f;

        /// <summary>
        /// Scrollable flow 
        /// </summary>
        private tk2dUIScrollableFlow _flow;

        void Awake()
        {
            _flow = GetComponent<tk2dUIScrollableFlow>();
            _flow.OnSelectChanged += FadeContent;
        }

        /// <summary>
        /// Fade all content that is not selected.
        /// </summary>
        private void FadeContent()
        {
            for (int i = 0; i < _flow.btns.Count; i++)
            {
                if (_flow.selectedBtn != _flow.btns[i])
                {
                    //Renderers
                    var renderers = _flow.btns[i].GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in renderers)
                    {
                        if (r.material.HasProperty("_Color"))
                            r.material.SetColor("_Color", new Color(r.material.color.r, r.material.color.g, r.material.color.b, alpha));
                    }
                    //tk2d Sprites
                    var sprites = _flow.btns[i].transform.GetComponentsInChildren<tk2dBaseSprite>();
                    foreach (tk2dBaseSprite sprite in sprites)
                    {
                        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                    }
                    //tk2d Text Meshes
                    var texts = _flow.btns[i].transform.GetComponentsInChildren<tk2dTextMesh>();
                    foreach (tk2dTextMesh text in texts)
                    {
                        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
                    }
                }
                else
                {
                    //Renderers
                    var renderers = _flow.btns[i].GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in renderers)
                    {
                        if (r.material.HasProperty("_Color"))
                            r.material.SetColor("_Color", new Color(r.material.color.r, r.material.color.g, r.material.color.b, 1.0f));
                    }
                    //tk2d Sprites
                    var sprites = _flow.btns[i].transform.GetComponentsInChildren<tk2dBaseSprite>();
                    foreach (tk2dBaseSprite sprite in sprites)
                    {
                        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1.0f);
                    }
                    //tk2d Text Meshes
                    var texts = _flow.btns[i].transform.GetComponentsInChildren<tk2dTextMesh>();
                    foreach (tk2dTextMesh text in texts)
                    {
                        text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
                    }
                }
            }
        }
    }
}
