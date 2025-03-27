using UnityEngine;

public class WaterGun : MonoBehaviour
{
    [SerializeField] private PlayAnimation waterGun;
   
    void Start()
    {
        waterGun.SetSpriteRDToFirstSprite();
    }

    [ContextMenu("PlayAnimation")]
    public void PlayAnimation()
    {
        waterGun.SetSpriteRDToFirstSprite();
        waterGun.Play();
    }
}
