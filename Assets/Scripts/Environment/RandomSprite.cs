
using HackedDesign;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;

    private void Awake() => this.AutoBind(ref spriteRenderer);
    void Start()
    {
        if (sprites == null || sprites.Count <= 0)
        {
            return;
        }
        var sprite = sprites[Random.Range(0, sprites.Count)];
        if (sprite == null)
        {
            return;
        }
        spriteRenderer.sprite = sprite;
    }
}
