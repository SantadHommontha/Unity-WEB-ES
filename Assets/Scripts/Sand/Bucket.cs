using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] private PlayAnimation bucketPR;
    [SerializeField] private PlayAnimation bucketAC;
    void Start()
    {
        bucketPR.SetSpriteRDToFirstSprite();
        bucketAC.SetSpriteRDToNone();
    }

    [ContextMenu("PlayAnimation")]
    public void PlayAnimation()
    {
        bucketPR.SetSpriteRDToFirstSprite();
        bucketAC.SetSpriteRDToNone();
        bucketPR.Play(() => bucketAC.Play());
    }
   
   
}
