using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
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
    [SerializeField] private GameEvent resetGameEvent;
    // [SerializeField] private GameEvent leaveRoomEven;
    [SerializeField] private GameEvent resetRoomEvent;
    //  [SerializeField] private GameEvent gameEndEvent;
    [SerializeField] private GameEvent finishConnectToRoomEvent;
    //  [SerializeField] private GameEvent afterJoinTeamComplete;
    [SerializeField] private GameEvent masterPanelEvent;
    [SerializeField] private GameEvent UpdatePlayerList;
    [SerializeField] private GameEvent disconnectServer;

    [SerializeField] private Image inServerStatus;
    private Coroutine co_SendKeepAlive;
    private Coroutine co_Reconnect;
    private int reconnectCount;
    private int maxReconnectCount = 5;
    [Header("Value")]

    //  [SerializeField] private BoolValue isMaster;
    [SerializeField] private BoolValueHandle isMaster;
    //[SerializeField] private BoolValueHandle isMaster;
    [SerializeField] private BoolValue finishConnectToServer;
    [SerializeField] private FloatValue connectTOserver;


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
        reconnectCount = 0;
        Debug.Log("Connect...");
        connectEvent.Raise(this, this);
        connectTOserver.Value = 0.2f;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        connectTOserver.Value = 0.6f;
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "asia";
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnected()
    {
        base.OnConnected();

    }
    #region  OnJoinedLobby
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom("Room Test", null, null);
        connectTOserver.Value = 0.8f;
        Debug.Log("Join a Lobby");
    }
    #endregion
    // public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    // {
    //     base.OnRoomPropertiesUpdate(propertiesThatChanged);
    // }
    #region  OnJoinedRoom
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();


        if (!isMaster.localValue)
        {
            isMaster.Value = PhotonNetwork.IsMasterClient;
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
            PhotonNetwork.KeepAliveInBackground = 60f;
        }
        else
        {
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 300000;
            PhotonNetwork.KeepAliveInBackground = 300f;
        }
        connectTOserver.Value = 1f;
        finishConnectToServer.Value = true;
        finishConnectToRoomEvent.Raise(this, isMaster.Value);

        Debug.Log("JoinedRoom");
    }




    #endregion
    // Call With Event
    public void AfterJoinRoom(Component _sender, object _data)
    {

        // StartCoroutine(CountDownBeforeEnterGame());
    }
    // Call With Button
    public void TapToEnterGame()
    {
        chooseTeamEvent.Raise(this, isMaster.Value);
        UpdatePlayerList.Raise(this, -999);
    }
    private IEnumerator CountDownBeforeEnterGame()
    {
        connectTOserver.Value = 0.7f;
        yield return new WaitForSeconds(0.4f);
        connectTOserver.Value = 1f;
        yield return new WaitForSeconds(0.6f);

        chooseTeamEvent.Raise(this, isMaster.Value);
        UpdatePlayerList.Raise(this, -999);
    }
    #region Kick And Leave Room
    public void LeaveRoom(Component _sender, object _data)
    {

        Debug.Log("Kicked out");
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

    #endregion
    #region OnPlayerLeftRoom
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log($"Player Left Room: {otherPlayer.UserId}");
    }
    #endregion

    #region ChangeMaster
    public void ChangeMaster(Player _newMaster)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.SetMasterClient(_newMaster);
            resetRoomEvent.Raise(this, -999);
            StartCoroutine(CountDownAfterStartNewMaster());
        }
    }
    IEnumerator CountDownAfterStartNewMaster()
    {
        yield return new WaitForSeconds(2);

        chooseTeamEvent.Raise(this, -999);
        photonView.RPC("NewMaster", RpcTarget.All);
    }

    [PunRPC]
    private void NewMaster()
    {

        isMaster.Value = PhotonNetwork.IsMasterClient;

        if (co_SendKeepAlive != null)
            StopCoroutine(co_SendKeepAlive);


        if (isMaster.Value)
        {
            resetRoomEvent.Raise(this, -999);
            masterPanelEvent.Raise(this, isMaster.Value);
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
            PhotonNetwork.KeepAliveInBackground = 60f;
            co_SendKeepAlive = StartCoroutine(IE_SendKeepAlive());
        }
        else
        {
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 300000;
            PhotonNetwork.KeepAliveInBackground = 300f;
        }
        resetGameEvent.Raise(this, -999);
    }

    public void UpdateMasterClient()
    {
        isMaster.Value = PhotonNetwork.IsMasterClient;
    }
    #endregion
    #region  SendKeepAlive
    IEnumerator IE_SendKeepAlive()
    {
        while (isMaster.Value && PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(10);
            photonView.RPC("SendKeepAlive", RpcTarget.All);
        }
    }
    [PunRPC]
    private void SendKeepAlive()
    {
        Debug.Log("Sending KeepAlive RPC...");
    }
    #endregion


    #region When Disconnect

    private IEnumerator IE_Reconncet()
    {
        while (!PhotonNetwork.IsConnected && reconnectCount < maxReconnectCount)
        {
            PhotonNetwork.Reconnect();
            yield return new WaitForSeconds(1);
            reconnectCount++;
            Debug.Log("Reconnect");
        }
        //        disconnectServer.Raise(this, -999);
    }


    #endregion
    #region Update 
    void Update()
    {
        if (!PhotonNetwork.IsConnected && finishConnectToServer.Value)
        {
            if (co_Reconnect == null)
            {
                co_Reconnect = StartCoroutine(IE_Reconncet());
            }
        }
       
        if (PhotonNetwork.IsConnected)
        {
            inServerStatus.color = Color.green;
        }
        else
        {
            inServerStatus.color = Color.red;
        }
    }
    #endregion

    private void OnApplicationFocus(bool focus)
    {

        if (focus && !PhotonNetwork.IsConnected)
        {
            Debug.Log("Reconnecting to Photon...");
            PhotonNetwork.Reconnect();
        }

    }

    public void RESETROOMM()
    {
        photonView.RPC("RRRRR", RpcTarget.All);
    }
    [PunRPC]
    private void RRRRR()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();
            StartCoroutine(GGG());
        }
    }
    private IEnumerator GGG()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.JoinOrCreateRoom("Room Test", null, null);
    }



}
