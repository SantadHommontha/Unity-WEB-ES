using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] private PlayAnimation bucketPR;
    [SerializeField] private PlayAnimation bucketAC;

    private bool isPlaying = false;
    void Start()
    {
        bucketPR.SetSpriteRDToFirstSprite();
        bucketAC.SetSpriteRDToNone();
    }

    [ContextMenu("PlayAnimation")]
    public void PlayAnimation()
    {
        if (isPlaying) return;
        bucketPR.SetSpriteRDToFirstSprite();
        bucketAC.SetSpriteRDToNone();
        isPlaying = true;
        bucketPR.Play(AfterPlay);
    }
    private void AfterPlay()
    {
        bucketPR.SetSpriteRDToNone();
        bucketAC.Play(AfterACPlay);
    }
    private void AfterACPlay()
    {
        bucketPR.SetSpriteRDToFirstSprite();
        bucketAC.SetSpriteRDToNone();
        isPlaying = false;
    }

}
