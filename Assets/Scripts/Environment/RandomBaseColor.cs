
using UnityEngine;
using UnityEngine.Rendering;

namespace HackedDesign
{
    public class RandomBaseColor : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            this.AutoBind(ref spriteRenderer);
        }
        private void Start()
        {
            spriteRenderer.color = Color.HSVToRGB(Random.value, 0.15f, 0.5f);
        }
    }
}
