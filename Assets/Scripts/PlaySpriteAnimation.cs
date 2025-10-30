using UnityEngine;
using System.Collections;

public class PlaySpriteAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] allSprtite;

    [SerializeField] private bool SetFirstSpriteWhenEndofPlay = false;
    [SerializeField] private float playTime = 1f;

    [SerializeField] private bool waitAnimationEnd = false;
    [SerializeField] private bool playloop = false;
    private Coroutine ct_playsprite;

    private float time;
    private bool play;
    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        spriteRenderer.sprite = allSprtite[0];
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void SetUp()
    {
        spriteRenderer.sprite = allSprtite[0];
        time = playTime / allSprtite.Length;
        //  currentSprite = 0;
      
        play = false;
    }


    private IEnumerator PlayAnimation(float _time)
    {

        int currentSprite = 0;
        while (play)
        {
            spriteRenderer.sprite = allSprtite[currentSprite];
            currentSprite++;
            yield return new WaitForSeconds(_time);
            if (currentSprite >= allSprtite.Length)
            {
                play = false;
                currentSprite = 0;
            }
            if (playloop)
                play = true;
        }

        if (SetFirstSpriteWhenEndofPlay)
            spriteRenderer.sprite = allSprtite[0];


        ct_playsprite = null;
    }


    [ContextMenu("Play")]
    public void Play()
    {
        if (!waitAnimationEnd)
        {
            if (ct_playsprite != null)
                StopCoroutine(ct_playsprite);

            SetUp();
            play = true;
            ct_playsprite = StartCoroutine(PlayAnimation(time));
        }
        else
        {
            if (ct_playsprite == null)
            {
                SetUp();
                play = true;
                ct_playsprite = StartCoroutine(PlayAnimation(time));
            }
        }
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        ct_playsprite = StartCoroutine(PlayAnimation(time));
        play = false;
        if (SetFirstSpriteWhenEndofPlay)
        {
            spriteRenderer.sprite = allSprtite[0];
        }
    }
}
