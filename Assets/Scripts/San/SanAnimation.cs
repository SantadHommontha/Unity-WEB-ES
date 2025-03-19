using System.Collections;
using UnityEngine;

public class SanAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] allSprtite;
    [SerializeField] private SanAnimation sanHeadAnimation;
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
        SetUp();
    }


    private void SetUp()
    {
        spriteRenderer.sprite = allSprtite[0];
        time = 1f / framePerSecond;
        spriteIndex = 0;
    }
    [ContextMenu("Play Animation")]
    public void PlayAnimation()
    {
        SetUp();
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
        if (sanHeadAnimation) CreateHead();
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

    private void CreateHead()
    {
        float height = spriteRenderer.bounds.size.y;
        Vector3 newPosition = spriteRenderer.transform.position + new Vector3(0, height / 2f, 0);
        var sh = Instantiate(sanHeadAnimation, newPosition, Quaternion.identity);
        var sp = sh.GetComponent<SpriteRenderer>().bounds.size.y;
        sh.transform.position += new Vector3(0, sp / 2f, 0);
        if (sh.TryGetComponent<SanAnimation>(out var component))
        {
            component.PlayAnimation();
        }
    }

}
