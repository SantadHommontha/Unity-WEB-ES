using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class KickBTN : MonoBehaviourPunCallbacks
{
     [SerializeField] private Button startBtn;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        startBtn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

   
}
