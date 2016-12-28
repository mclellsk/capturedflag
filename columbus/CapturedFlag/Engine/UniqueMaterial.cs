using UnityEngine;

namespace CapturedFlag.Engine
{
    public class UniqueMaterial : MonoBehaviour
    {
        public Shader shader;

        public void SetMaterial()
        {
            var renderer = GetComponent<Renderer>();
            renderer.materials[0].shader = shader;
        }
    }
}
