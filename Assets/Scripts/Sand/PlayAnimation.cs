using UnityEngine;

using System;
using System.Collections;
public class PlayAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] allSprtite;



    private int spriteIndex = 0;
    private Coroutine co_timeAnimation;
    private float time;
    [SerializeField] private float framePerSecond;
    [SerializeField] private float second = 1f;


    void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteRDToFirstSprite()
    {
        spriteRenderer.sprite = allSprtite[0];
    }
    public void SetSpriteRDToNone()
    {
        spriteRenderer.sprite = null;
    }
    private void SetUp()
    {
        spriteRenderer.sprite = allSprtite[0];
        time = second / framePerSecond;
        spriteIndex = 0;
    }
    [ContextMenu("Play")]
    public void Play()
    {
        Play(() => { });
    }
    public void Play(Action _callback)
    {
        SetUp();
        if (co_timeAnimation != null)
        {
            StopCoroutine(co_timeAnimation);
            co_timeAnimation = null;
        }
        co_timeAnimation = StartCoroutine(TimeAnimationUP(time, _callback));
    }

    IEnumerator TimeAnimationUP(float _time, Action _callback)
    {
        spriteRenderer.enabled = true;
        while (spriteIndex < allSprtite.Length)
        {
            yield return new WaitForSeconds(_time);
            spriteRenderer.sprite = allSprtite[NextSprite()];
        }
        co_timeAnimation = null;
        _callback?.Invoke();
    }

    private int NextSprite()
    {
        int rt = spriteIndex;
        spriteIndex++;
        return rt;
    }
}
