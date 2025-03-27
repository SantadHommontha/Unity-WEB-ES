using UnityEngine;

public class Bucket : MonoBehaviour
{
    [SerializeField] private PlayAnimation bucketPR;
    [SerializeField] private PlayAnimation bucketAC;
    void Start()
    {
        
    }

    [ContextMenu("PlayAnimation")]
    public void PlayAnimation()
    {
        bucketPR.Play(() => bucketAC.Play());
    }
   
   
}
