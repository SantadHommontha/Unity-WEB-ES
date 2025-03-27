using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SanAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer GetSpriteRenderer => spriteRenderer;
    [SerializeField] private Sprite[] allSprtite;
    public SanAnimation sanHeadAnimation;
    [SerializeField] private float framePerSecond;
    [SerializeField] private float second = 1f;
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


    public void SetUp()
    {
        spriteRenderer.sprite = allSprtite[0];
        time = second / framePerSecond;
        spriteIndex = 0;
        if (sanHeadAnimation)
        {
            sanHeadAnimation.SetUp();
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    #region UP
    // -----------------------------------------UP----------------
    [ContextMenu("Play Animation Up")]
    public void PlayAnimationUP()
    {
        PlayAnimationUP(() => { });
    }
    public void PlayAnimationUP(Action _callback)
    {
        SetUp();
        if (timeAnimation != null)
        {
            StopCoroutine(timeAnimation);
            timeAnimation = null;
        }
        spriteIndex = 0;
        spriteRenderer.enabled = true;
        timeAnimation = StartCoroutine(TimeAnimationUP(time, _callback));
    }
    IEnumerator TimeAnimationUP(float _time, Action _callback)
    {
        spriteRenderer.enabled = true;
        while (spriteIndex < allSprtite.Length)
        {
            yield return new WaitForSeconds(_time);
            ShowSpriteUP();
        }
        timeAnimation = null;
        _callback?.Invoke();

        if (sanHeadAnimation)
        {
            // spriteRenderer.sortingOrder = 2;
            CreateHead();
        }
    }
    private int NextSprite()
    {
        int rt = spriteIndex;
        spriteIndex++;
        return rt;
    }
    private void ShowSpriteUP()
    {
        spriteRenderer.sprite = allSprtite[NextSprite()];
    }
    #endregion

    #region Down
    // -----------------------------------------Down----------------

    [ContextMenu("Play Animation Down")]
    public void PlayAnimationDown()
    {
        AnimationDown(() => { StartDown(() => { }); });
    }
    public void PlayAnimationDown(Action _callback)
    {
        AnimationDown(() => { StartDown(() => { _callback?.Invoke(); }); });
    }
    private void AnimationDown(Action _callback)
    {
        if (sanHeadAnimation)
        {
            sanHeadAnimation.AnimationDown(() => _callback?.Invoke());
        }
        else
        {
            StartDown(_callback);
        }
    }
    public void StartDown(Action _callback)
    {
        spriteRenderer.enabled = true;
        if (timeAnimation != null)
        {
            StopCoroutine(timeAnimation);
            timeAnimation = null;
        }
        timeAnimation = StartCoroutine(TimeAnimationDown(time, _callback));
    }
    IEnumerator TimeAnimationDown(float _time, Action _callback)
    {
        while (spriteIndex > 0)
        {
            yield return new WaitForSeconds(_time);
            ShowSpriteDown();
        }
        timeAnimation = null;
        spriteRenderer.enabled = false;
        _callback?.Invoke();
    }
    private int PreviousSprite()
    {
        spriteIndex--;
        spriteIndex = Mathf.Clamp(spriteIndex, 0, allSprtite.Length - 1);
        return spriteIndex;
    }
    private void ShowSpriteDown()
    {
        spriteRenderer.sprite = allSprtite[PreviousSprite()];
    }
    #endregion



    // -----------------------------------------CreateHead----------------
    private void CreateHead()
    {
        float height = spriteRenderer.bounds.size.y;
        Vector3 newPosition = spriteRenderer.transform.position + new Vector3(0, height / 2f, 0);

        var sp = sanHeadAnimation.GetComponent<SpriteRenderer>().bounds.size.y;

        sanHeadAnimation.transform.position = newPosition -= new Vector3(0, (sp / 2f) - 2, 0);
        sanHeadAnimation.PlayAnimationUP();
    }

    public void SkipToLstSprite()
    {
        if (timeAnimation != null)
        {
            StopCoroutine(timeAnimation);
            timeAnimation = null;
        }
        spriteRenderer.sprite = allSprtite[allSprtite.Length - 1];
    }
    public void SkipToFristSprite()
    {
        if (timeAnimation != null)
        {
            StopCoroutine(timeAnimation);
            timeAnimation = null;
        }
        spriteRenderer.sprite = allSprtite[0];

    }

}
