using UnityEngine.UI;
using Photon.Pun;
using UnityEngine;

public class LoadingBar : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image imageBar;
    [SerializeField] private FloatValue floatValue;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        floatValue.OnValueChange += OnValueChange;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        floatValue.OnValueChange -= OnValueChange;
    }

    private void OnValueChange(float _data)
    {
        imageBar.fillAmount = Mathf.Clamp01(_data);
    }
}
