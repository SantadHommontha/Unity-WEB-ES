using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager instace;
    //  [SerializeField] private GameObject connectCanva;
    //  [SerializeField] private GameObject chooseTeamCanva;
    //  [SerializeField] private GameObject play_Canva;
    // [SerializeField] private GameObject leveRoomCanvaTest;
    [Header("Event")]
    [SerializeField] private GameEvent connectEvent;
    [SerializeField] private GameEvent chooseMode;
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
    private bool leftToNewRoom = false;
    [Header("Value")]

    [SerializeField] private BoolValue isMaster;
    // [SerializeField] private BoolValueHandle isMaster;
    //[SerializeField] private BoolValueHandle isMaster;
    [SerializeField] private BoolValue finishConnectToServer;
    [SerializeField] private FloatValue connectTOserver;
    [SerializeField] private BoolValue iamAdmin;
    // [SerializeField] private 
    // [SerializeField] private StringValue myRoomCode;


    //  [SerializeField] private bool isMaster;
    public void NewScene()
    {
        PhotonNetwork.LeaveRoom();


    }
    void Awake()
    {
        if (instace != null && instace != this)
            Destroy(this.gameObject);
        else
            instace = this;
    }
    private void Start()
    {

        reconnectCount = 0;
        Debug.Log("Connect...");
        connectEvent.Raise(this, this);
        connectTOserver.Value = 0.2f;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        finishConnectToServer.Value = false;
        Debug.Log("OnConnectedToMaster");
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

        Debug.Log("Join a Lobby");
        finishConnectToServer.Value = true;
        finishConnectToRoomEvent.Raise(this, PhotonNetwork.IsMasterClient);
        //      CreateRoom("Game Room" + Random.Range(0, 1000).ToString());
        // if (!leftToNewRoom)
        //     CreateRoom("Game Room" + Random.Range(0, 1000).ToString());
        // else
        // {
        //     leftToNewRoom = false;
        //     if (iamAdmin.Value)
        //         CreateRoom();
        //     else
        //         JoinRoom();
        // }
        if (leftToNewRoom)
        {
            leftToNewRoom = false;
            CreateRoomForMutiPLayer();
        }
        connectTOserver.Value = 1f;
    }
    public void CreateRoomForSinglePlayer()
    {
        CreateRoom("Game Room" + Random.Range(0, 1000).ToString());
    }
    public void CreateRoomForMutiPLayer()
    {
        CreateRoom();
    }
    public void CreateRoom(string _roomName = "Game Room Main")
    {

        PhotonNetwork.JoinOrCreateRoom(_roomName, null, null);
    }

    public void LeftAndJoinNewRoom()
    {
        // leftToNewRoom = true;
        Debug.Log("LeftAndJoinNewRoom");
        chooseTeamEvent.Raise(this, -999);
        //  PhotonNetwork.LeaveRoom();
    }
    private void JoinRoom(string _roomName = "Game Room Main")
    {
        Debug.Log("Join Room");
        PhotonNetwork.JoinRoom(_roomName);

    }

    public void Onlef()
    {



    }
    #region OnMasterClientSwitched
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //  base.OnMasterClientSwitched(newMasterClient);
        // resetGameEvent.Raise(this, -999);
        // isMaster.Value = false;
        // //   Debug.Log("HHHHHHHHHHHHHH");
        // if (newMasterClient == PhotonNetwork.LocalPlayer)
        // {
        //     Debug.Log(" ย้าย Master Client มาที่คุณสำเร็จแล้ว! คุณคือ Master Client ใหม่");

        //     resetRoomEvent.Raise(this, -999);
        //     masterPanelEvent.Raise(this, isMaster.Value);
        //     PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
        //     PhotonNetwork.KeepAliveInBackground = 60f;
        //     co_SendKeepAlive = StartCoroutine(IE_SendKeepAlive());
        //     iamAdmin.Value = true;
        //     isMaster.Value = true;
        // }
        // else
        // {
        //     if (!openMaster) return;
        //     openMaster = false;
        //     Debug.Log($"Master Client ถูกย้ายไปที่ผู้เล่น: {newMasterClient.NickName}");
        //     iamAdmin.Value = false;
        //     PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 300000;
        //     PhotonNetwork.KeepAliveInBackground = 300f;
        //     isMaster.Value = false;
        //     LeftAndJoinNewRoom();
        // }
        isMaster.Value = false;
        if (PhotonNetwork.IsMasterClient)
        {
            SetingRoom();
            photonView.RPC("RPC_GOto", RpcTarget.Others);
            isMaster.Value = PhotonNetwork.IsMasterClient;
            GameManager.instance.SetupEvents();
        }
        else
        {
            GameManager.instance.ResetSetupEvents();
        }

    }
    #endregion
    [PunRPC]
    public void RPC_GOto()

    {

        //  RoomManager.instace.SetingRoom();
        NewScene();
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("OnLeftRoom");
        // PhotonNetwork.JoinLobby();
        SceneManager.LoadScene("MainScene");

        //   if (leftToNewRoom)

        // {
        //     JoinNewRoom();
        // }
    }
    public void SetingRoom()
    {
        resetRoomEvent.Raise(this, -999);
        masterPanelEvent.Raise(this, PhotonNetwork.IsMasterClient);
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
        PhotonNetwork.KeepAliveInBackground = 60f;
        co_SendKeepAlive = StartCoroutine(IE_SendKeepAlive());
        iamAdmin.Value = true;
        isMaster.Value = true;
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


        Debug.Log("JoinedRoom");
        if (!PhotonNetwork.IsMasterClient)
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



        // if (leftToNewRoom)
        // {
        //     leftToNewRoom = false;
        //     TapToEnterGame();
        // }
        // if (TeamManager.instance.requestSendJoyTestAgaing)
        // {
        //     TeamManager.instance.SendJoyTeamAgain();
        // }

    }




    #endregion
    // Call With Event
    public void AfterJoinRoom(Component _sender, object _data)
    {

        // StartCoroutine(CountDownBeforeEnterGame());
    }
    // Call With Button

    public void TapToChooseTeam()
    {
        chooseMode.Raise(this, -999);
    }
    public void TapToEnterGame()
    {
        chooseTeamEvent.Raise(this, PhotonNetwork.IsMasterClient);
        UpdatePlayerList.Raise(this, -999);
    }
    private IEnumerator CountDownBeforeEnterGame()
    {
        connectTOserver.Value = 0.7f;
        yield return new WaitForSeconds(0.4f);
        connectTOserver.Value = 1f;
        yield return new WaitForSeconds(0.6f);

        chooseTeamEvent.Raise(this, PhotonNetwork.IsMasterClient);
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
    bool openMaster;
    public void ChangeMaster(Player _newMaster)
    {
        // if (PhotonNetwork.IsMasterClient)
        // {
        //   PhotonNetwork.SetMasterClient(_newMaster);
        //
        // Debug.Log("ChangeMaster");
        // iamAdmin.Value = false;

        // if (PhotonNetwork.IsMasterClient)
        // {
        //     Debug.Log("เป็น Master Client อยู่แล้ว");
        //     resetRoomEvent.Raise(this, -999);
        //     masterPanelEvent.Raise(this, isMaster.Value);
        //     PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
        //     PhotonNetwork.KeepAliveInBackground = 60f;
        //     co_SendKeepAlive = StartCoroutine(IE_SendKeepAlive());

        // }
        // else

        // {
        //     PhotonNetwork.SetMasterClient(_newMaster);
        //     openMaster = true;
        PhotonNetwork.SetMasterClient(_newMaster);
        // }
        // resetRoomEvent.Raise(this, -999);

        //  }
        // RequestMasterClientTransferToSelf(_newMaster);
        if (co_SendKeepAlive != null)
            StopCoroutine(co_SendKeepAlive);
        co_SendKeepAlive = null;
        //   StartCoroutine(CountDownAfterStartNewMaster());
    }


    public void RequestMasterClientTransferToSelf(Player _newMaster)
    {
        // 1. ตรวจสอบว่าเราอยู่ในห้องหรือไม่
        if (!PhotonNetwork.InRoom)
        {
            Debug.LogWarning("ไม่สามารถย้าย Master Client ได้ เพราะไม่ได้อยู่ในห้อง!");
            return;
        }

        // 2. ตรวจสอบว่าเราเป็น Master Client อยู่แล้วหรือไม่
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("คุณเป็น Master Client อยู่แล้ว");
            return;
        }

        // 3. เรียกฟังก์ชันเพื่อย้าย Master Client มาที่ LocalPlayer (ตัวเราเอง)
        Debug.Log("กำลังส่งคำขอเป็น Master Client...");

        // **นี่คือฟังก์ชันหลัก:**
        bool success = PhotonNetwork.SetMasterClient(_newMaster);

        if (success)
        {
            Debug.Log("ส่งคำขอสำเร็จ! รอกการยืนยัน...");
            chooseTeamEvent.Raise(this, -999);
        }
        else
        {
            // โดยปกติจะล้มเหลวถ้าการเชื่อมต่อไม่เสถียร หรือมีปัญหาอื่นๆ
            Debug.LogError("การส่งคำขอเป็น Master Client ล้มเหลว");
        }
    }
    IEnumerator CountDownAfterStartNewMaster()
    {
        yield return new WaitForSeconds(1);

        chooseTeamEvent.Raise(this, -999);
        photonView.RPC("NewMaster", RpcTarget.All);
    }

    [PunRPC]
    private void NewMaster()
    {

        isMaster.Value = PhotonNetwork.IsMasterClient;

        if (co_SendKeepAlive != null)
            StopCoroutine(co_SendKeepAlive);


        if (PhotonNetwork.IsMasterClient)
        {
            resetRoomEvent.Raise(this, -999);
            masterPanelEvent.Raise(this, PhotonNetwork.IsMasterClient);
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 60000;
            PhotonNetwork.KeepAliveInBackground = 60f;
            co_SendKeepAlive = StartCoroutine(IE_SendKeepAlive());
        }
        else
        {
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 300000;
            PhotonNetwork.KeepAliveInBackground = 300f;
            LeftAndJoinNewRoom();
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
        while (PhotonNetwork.IsMasterClient && PhotonNetwork.InRoom)
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
            //  PhotonNetwork.Reconnect();
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
        if (!PhotonNetwork.IsConnected && finishConnectToServer.Value || !leftToNewRoom)
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
        Debug.Log("RESETROOMM");
        photonView.RPC("RRRRR", RpcTarget.Others);
    }
    public void GoTOChooseMode()
    {
        Debug.Log("RoomManager Goto");
        PhotonNetwork.LeaveRoom();
        chooseMode.Raise(this, -999);
    }
    [PunRPC]
    private void RRRRR()
    {
        Debug.Log("RRRRR");
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();
            chooseMode.Raise(this, -999);
            //   leftToNewRoom = true;
            //  StartCoroutine(GGG());

        }
    }
    private IEnumerator GGG()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.JoinOrCreateRoom("Room Test", null, null);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log($"Join Room Fail Error: {message}");

    }

}
