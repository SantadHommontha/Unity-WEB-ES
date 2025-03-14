using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instace;
    //  [SerializeField] private GameObject connectCanva;
    //  [SerializeField] private GameObject chooseTeamCanva;
    //  [SerializeField] private GameObject play_Canva;
    // [SerializeField] private GameObject leveRoomCanvaTest;
    [Header("Event")]

    [SerializeField] private GameEvent connectEvent;
    [SerializeField] private GameEvent chooseTeamEvent;
    [SerializeField] private GameEvent playEvent;
    //  [SerializeField] private GameEvent leaveRoomEven;
    //  [SerializeField] private GameEvent KickEvent;
    [SerializeField] private GameEvent gameEndEvent;
    [SerializeField] private GameEvent finishConnectToServerEvent;
    [Header("Value")]

    [SerializeField] private BoolValue isMaster;
    [SerializeField] private BoolValue finishConnectToServer;



    //  [SerializeField] private bool isMaster;

    void Awake()
    {
        if (instace != null && instace != this)
            Destroy(this.gameObject);
        else
            instace = this;
    }
    private void Start()
    {
        finishConnectToServer.Value = false;
        Debug.Log("Connect...");
        connectEvent.Raise(this, this);

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


    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Room Test", null, null);
        //  play_Canva.SetActive(false);
        //  connectCanva.SetActive(false);
        //  chooseTeamCanva.SetActive(true);
        chooseTeamEvent.Raise(this, 0);
        Debug.Log("We're in a Room");
    }

    // public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    // {
    //     base.OnRoomPropertiesUpdate(propertiesThatChanged);
    // }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        isMaster.Value = PhotonNetwork.IsMasterClient;
        finishConnectToServer.Value = true;
        finishConnectToServerEvent.Raise(this, isMaster.Value);
        Debug.Log("Room");
    }
    // public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    // {
    //     base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    // }


    public void KICKROOM()
    {
        //   PhotonNetwork.LeaveRoom();
        //  PhotonNetwork.Disconnect();
        StartCoroutine(AfterLeveaServer());
    }

    private IEnumerator AfterLeveaServer()
    {
        yield return new WaitForSeconds(2);
        chooseTeamEvent.Raise(this, 0);
    }

    public void DisconnectServer()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player Left Room: {otherPlayer.UserId}");
    }
}
