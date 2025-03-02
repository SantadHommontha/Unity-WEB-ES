using UnityEngine;
using Photon.Pun;
public class RoomManager : MonoBehaviourPunCallbacks
{

    private void Start()
    {
         Debug.Log("Connect...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();

       


    }

    public override void OnConnected()
    {
        base.OnConnected();

        Debug.Log("mine");
 
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Room Test", null, null);

        Debug.Log("We're in a Room");
    }


}
