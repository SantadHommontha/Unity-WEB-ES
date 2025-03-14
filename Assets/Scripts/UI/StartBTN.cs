using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class StartBTN : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameEvent startEvent;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void Click()
    {
        startEvent.Raise(this, -999);
    }

}
