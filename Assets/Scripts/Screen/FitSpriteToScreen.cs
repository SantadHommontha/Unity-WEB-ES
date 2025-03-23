using UnityEngine;

public class FitSpriteToScreen : MonoBehaviour
{
    void Start()
    {
        FitToScreen();
    }

    [ContextMenu("FitToScreen")]
    void FitToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

     
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;

       
        float spriteHeight = sr.bounds.size.y;
        float spriteWidth = sr.bounds.size.x;

        
        transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteHeight, 1f);
    }
}
