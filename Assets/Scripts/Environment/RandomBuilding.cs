
using HackedDesign;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RandomBuilding : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public List<Sprite> buildingSprites;

    private float timer; 

    private void Awake()
    {
        this.AutoBind(ref spriteRenderer);
    }
    void Start()
    {
        SetSprite();
    }

    void SetSprite()
    {
        if (buildingSprites.Count == 0)
        {
            spriteRenderer.enabled = false;
            return;
        }

        var sprite = buildingSprites[Random.Range(0, buildingSprites.Count)];

        if (sprite != null)
        {
            spriteRenderer.sprite = buildingSprites[Random.Range(0, buildingSprites.Count)];
        }
    }
}
