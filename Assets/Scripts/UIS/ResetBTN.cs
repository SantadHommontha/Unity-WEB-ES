using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ResetBTN : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameEvent reSetGameEvent;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void Click()
    {
        reSetGameEvent.Raise(this, -999);
    }
}
