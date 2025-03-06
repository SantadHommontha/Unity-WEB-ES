using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject connectCanva;
    [SerializeField] private GameObject chooseTeamCanva;

    [SerializeField] private  bool isMaster;
    private void Start()
    {
        connectCanva.SetActive(true);
        chooseTeamCanva.SetActive(false);
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
        connectCanva.SetActive(false);
        chooseTeamCanva.SetActive(true);
        Debug.Log("We're in a Room");
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        isMaster = PhotonNetwork.IsMasterClient;
        Debug.Log("Room");
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    }
}
