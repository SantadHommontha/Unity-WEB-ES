using UnityEngine;

public class WaterGun : MonoBehaviour
{
    [SerializeField] private PlayAnimation waterGun;
    private bool isPlaying = false;
    void Start()
    {
        waterGun.SetSpriteRDToFirstSprite();
    }

    [ContextMenu("PlayAnimation")]
    public void PlayAnimation()
    {
        if (isPlaying && waterGun.gameObject.activeSelf) return;
        waterGun.SetSpriteRDToFirstSprite();
        isPlaying = true;
        waterGun.Play(AfterPlay);
    }
    private void AfterPlay()
    {
        isPlaying = false;
        waterGun.SetSpriteByIndex(0);
    }
}
