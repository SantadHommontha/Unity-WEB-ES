using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject connectCanva;
    [SerializeField] private GameObject chooseTeamCanva;
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
}
