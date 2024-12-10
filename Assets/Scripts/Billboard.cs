
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    [SerializeField] public List<Sprite> billboards;

    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        SetSprite();
    }

    void SetSprite()
    {
        if (billboards.Count == 0)
        {
            spriteRenderer.enabled = false;
            return;
        }

        var sprite = billboards[Random.Range(0, billboards.Count)];

        if (sprite != null)
        {
            spriteRenderer.sprite = billboards[Random.Range(0, billboards.Count)];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
