using UnityEngine;
using CapturedFlag.Engine;

namespace CapturedFlag.tk2d
{
    [RequireComponent(typeof(tk2dBaseSprite))]
    public class tk2dFadeSprite : MonoBehaviour
    {
        public FadeColor fade;

        private tk2dBaseSprite _sprite;

        public void Awake()
        {
            _sprite = GetComponent<tk2dBaseSprite>();

            fade.color = _sprite.color;
        }

        public void Update()
        {
            _sprite.color = fade.color;
        }

        public void Reset()
        {
            fade.Reset();
        }

        public void SetActive()
        {
            fade.isActive = true;
        }

        public void SetDelay(float time)
        {
            fade.delayTime = time;
        }
    }
}