using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public int count = 0;

    void Start()
    {
        if (spriteRenderer == null)
        { spriteRenderer.sprite = sprites[0]; }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateSprite();
            count++;

            if(count >= sprites.Length)
            {
                count = 0;
            }
        }
    }
    
    void UpdateSprite()
    {
        if (count >= 0)
        {
            spriteRenderer.sprite = sprites[count];
        }
        Debug.Log(count);
    }
}
