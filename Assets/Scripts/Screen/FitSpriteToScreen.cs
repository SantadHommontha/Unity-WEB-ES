using UnityEngine;

public class FitSpriteToScreen : MonoBehaviour
{
    [SerializeField] private Vector2 startSpriteSize;
    private SpriteRenderer sr;
    void Start()
    {
        //  FitToScreen();
        FitToScreen();
    }



    void OnEnable()
    {
        //   transform.localScale = Vector3.one;
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
            startSpriteSize = sr.bounds.size;
        }
        //    FitToScreen();
    }
    [ContextMenu("FitToScreen")]
    void FitToScreen()
    {
        //  transform.localScale = Vector3.one;

        if (sr == null) return;


        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Screen.width / Screen.height;


        float spriteHeight = startSpriteSize.y;
        float spriteWidth = startSpriteSize.x;
      //  Debug.Log($"{screenWidth} {spriteWidth} {screenHeight} {spriteHeight} {Screen.width} {Screen.height} {Camera.main.orthographicSize * 2f}");
        //  spriteRenderer.bounds.size = new Vector3(1080, 1920, 0);

        transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteHeight, 1f);
    }
}
