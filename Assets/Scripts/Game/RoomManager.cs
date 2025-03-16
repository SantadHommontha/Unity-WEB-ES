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
    [SerializeField] private GameEvent KickedOutEvent;
    [SerializeField] private GameEvent gameEndEvent;
    [SerializeField] private GameEvent finishConnectToRoomEvent;
    [SerializeField] private GameEvent afterJoinTeamComplete;
    [SerializeField] private GameEvent masterPanelEvent;
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
      //  Debug.Log("OnConnectedToMaster : "+PhotonNetwork.IsMasterClient );
        PhotonNetwork.JoinLobby();




    }

    public override void OnConnected()
    {
        base.OnConnected();
      //  Debug.Log("OnConnected : " + PhotonNetwork.IsMasterClient);

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Room Test", null, null);
        // play_Canva.SetActive(false);
        Debug.Log("Join a Lobby");
        //  connectCanva.SetActive(false);
        //  chooseTeamCanva.SetActive(true);

   //     Debug.Log("We're in a Room");
    }

    // public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    // {
    //     base.OnRoomPropertiesUpdate(propertiesThatChanged);
    // }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //chooseTeamEvent.Raise(this, -999);
        isMaster.Value = PhotonNetwork.IsMasterClient;
       
        finishConnectToServer.Value = true;
        finishConnectToRoomEvent.Raise(this, isMaster.Value);
        Debug.Log("JoinedRoom");
    }
    // public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    // {
    //     base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    // }

    // Call With Event
    public void AfterJoinRoom(Component _sender,object _data)
    {
        if (isMaster.Value && (bool)_data)
        {
            masterPanelEvent.Raise(this, isMaster.Value);
        }
        else
        {
            chooseTeamEvent.Raise(this, isMaster.Value);
        }
    }

    public void KICKROOM(Component _sender, object _data)
    {
        //   PhotonNetwork.LeaveRoom();
        //  PhotonNetwork.Disconnect();
        Debug.Log("Kicked out");
       // DisconnectServer();
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
