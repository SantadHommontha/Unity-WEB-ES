using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
public class PlayerNameBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject kick_Btn;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        kick_Btn.SetActive(PhotonNetwork.IsMasterClient);
    }


    private void SetEvent()
    {
        GameManager.instance.ResetGameEvent += () => kick_Btn.SetActive(PhotonNetwork.IsMasterClient);
    }


    void Awake()
    {
        SetEvent();
    }
}
