using System.Collections;
using UnityEngine;

public class SanAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] allSprtite;

    [SerializeField] private float framePerSecond;
    private int spriteIndex = 0;

    private float time;
    private Coroutine timeAnimation;

    void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        spriteRenderer.sprite = allSprtite[0];
        time = 1f / framePerSecond;
        spriteIndex = 0;
    }
    [ContextMenu("Player Animation")]
    private void PlayerAnimation()
    {
        if (timeAnimation != null)
        {
            StopCoroutine(timeAnimation);
            timeAnimation = null;
        }
        spriteIndex = 0;
        timeAnimation = StartCoroutine(TimeAnimation(time));



    }
    IEnumerator TimeAnimation(float _time)
    {
        while (spriteIndex < allSprtite.Length)
        {
            yield return new WaitForSeconds(_time);
            ShowSprite();
        }
        timeAnimation = null;
    }

    private int NextSprite()
    {
        int rt = spriteIndex;
        spriteIndex++;
        return rt;
    }

    private void ShowSprite()
    {
        spriteRenderer.sprite = allSprtite[NextSprite()];
    }

}
