
using Unity.VisualScripting;
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
            spriteRenderer.color = Color.HSVToRGB(this.transform.position.y / 256, 0.15f, 0.5f);
        }
    }
}
