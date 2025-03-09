using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;




public class TeamManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region  Variable
    public static TeamManager instance;

    [Header("Team Setting")]
    [SerializeField] private TeamSetting teamSetting;
    //--UI
    [Space]
    [SerializeField] private TMP_InputField enterNameInput;
    [SerializeField] private Button btn1, btn2;
    [SerializeField] private TMP_Text reportError;

    //--Canva
    [Space]
    //  [SerializeField] private GameObject chooseTeamCanva;
    //  [SerializeField] private GameObject playCanva;
    [Header("Event")]
    [SerializeField] private GameEvent chooseTeamEven;
    [SerializeField] private GameEvent playerEvent;
    [Space]
    //---Team
    [SerializeField] private Team addTeam;
    [SerializeField] private Team minusTeam;
    private string myTeamType;
    public string MyTeamType => myTeamType;

    private Team myTeam;
    public Team MyTeam => myTeam;
    public Team AddTeam => addTeam;
    public Team MinusTeam => minusTeam;
    //Var
    public int AllPLayerCount => addTeam.PlayerCount + minusTeam.PlayerCount;
    #endregion

    #region  GameEvent
    private void SetupEvent()
    {
        GameManager.instance.GameStopEvent += GameEnd;

        GameManager.instance.ResetGameEvent += Reset;


    }

    private void Reset()
    {
        AddTeam.Reset();
        minusTeam.Reset();
    }

    private void GameEnd()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable teamScore = new Hashtable()
            {
                { "AddScore", AddTeam.Score },
                { "MinusScore", MinusTeam.Score },
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(teamScore);
        }
    }
    #endregion


    #region Unity Function
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;


    }
    void Start()
    {
        // playCanva.SetActive(false);
        SetupEvent();
        addTeam = new Team(TeamName.Red, teamSetting);
        minusTeam = new Team(TeamName.Blue, teamSetting);

        btn1.onClick.AddListener(() => RequestJoinTeam(TeamName.Red));
        btn2.onClick.AddListener(() => RequestJoinTeam(TeamName.Blue));
    }
    #endregion


    #region Join Team
    private void RequestJoinTeam(TeamName _teamName)
    {
        var playerData = new PlayerData();
        playerData.playerName = enterNameInput.text == "" ? $"PLayer{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}" : enterNameInput.text;
        playerData.playerID = PhotonNetwork.LocalPlayer.UserId;

        playerData.teamName = _teamName;

        var jsonData = JsonUtility.ToJson(playerData);

        photonView.RPC("TryJoinTeam", RpcTarget.MasterClient, jsonData);
    }

    [PunRPC]
    private void TryJoinTeam(string _jsonData, PhotonMessageInfo _info)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var data = JsonUtility.FromJson<PlayerData>(_jsonData);
        data.info = _info;
        switch (data.teamName)
        {
            case TeamName.Red:
                if (!addTeam.HavePlayer(data.playerID))
                {
                    if (!addTeam.TeamFull())
                    {

                        addTeam.AddPlayer(data);
                        myTeam = addTeam;
                        Debug.Log($"PLayer Join AddTeam: {data.playerID}");
                        UpdatePlayerListToRoomPorperties();
                        photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Complete, "Join Team Complete", "ADD"));
                    }
                    else
                    {
                        photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Fail, "Team Have Full"));
                    }
                }
                else
                {
                    photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Fail, "You Have Join In Red Team"));
                }

                break;
            case TeamName.Blue:
                if (!minusTeam.HavePlayer(data.playerID))
                {
                    if (!minusTeam.TeamFull())
                    {
                        minusTeam.AddPlayer(data);
                        myTeam = minusTeam;
                        Debug.Log($"PLayer Join MinusTeam: {data.playerID}");
                        UpdatePlayerListToRoomPorperties();
                        photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Complete, "Join Team Complete", "MINUS"));
                    }
                    else
                    {
                        photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Fail, "Team Have Full"));
                    }
                }
                else
                {
                    photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Fail, "You Have Join In Blue Team"));
                }
                break;
        }
    }
    private void UpdatePlayerListToRoomPorperties()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var a = addTeam.GetAllPlayerToPlayerTeamList();
        var b = minusTeam.GetAllPlayerToPlayerTeamList();
        // Debug.Log($"SetP {a}");
        //  Debug.Log($"SetP2 {b}");
        ExitGames.Client.Photon.Hashtable playerList = new ExitGames.Client.Photon.Hashtable()
        {
            {"ADD_TEAM_PLAYER_LIST",a},
            {"MINUS_TEAM_PLAYER_LIST",b}
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(playerList);

    }
    [PunRPC]
    private void ReceiveJoinTeam(string _reportJson)
    {
        var data = JsonUtility.FromJson<SendResponseJoinData>(_reportJson);
        if (data.responseState == ResponseState.Complete.ToString())
        {
         
            playerEvent.Raise(this, this);
            myTeamType = data.myTeamType;
            AfterJoinTeam();
            GameManager.instance.RequstUpDataGameData();
        }
        else if (data.responseState == ResponseState.Fail.ToString())
        {
            reportError.text = data.responseMessage;
        }

        photonView.RPC("UpDatePlayerDate", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void UpDatePlayerDate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // Hashtable playerList = new Hashtable()
            // {
            //     {addTeam.TeamName,addTeam.GetAllPlayerListString()},
            //     {minusTeam.TeamName,minusTeam.GetAllPlayerListString()}
            // };

            // PhotonNetwork.CurrentRoom.SetCustomProperties(playerList);
            UpdatePlayerListToRoomPorperties();
        }
    }


    private void AfterJoinTeam()
    {


    }
    #endregion


    #region Utility
    // Utility Funcetion
    private string PackJsonData(Enum _responseState, string _responseMessage, string _teamType)
    {
        ResponsetData reportDate;
        reportDate.responseState = _responseState.ToString();
        reportDate.responseMessage = _responseMessage;

        SendResponseJoinData sendResponseJoinData = new SendResponseJoinData()
        {
            responseState = _responseState.ToString(),
            responseMessage = _responseMessage,
            myTeamType = _teamType
        };
        return JsonUtility.ToJson(sendResponseJoinData);
    }
    private string PackJsonData(Enum _responseState, string _responseMessage)
    {
        return PackJsonData(_responseState, _responseMessage, "Nope");
    }


    #endregion


    #region  Photon Function
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }



    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (PhotonNetwork.IsMasterClient) return;
        if (propertiesThatChanged.ContainsKey("AddScore"))
        {
            addTeam.SetScore((int)propertiesThatChanged["AddScore"]);
        }
        if (propertiesThatChanged.ContainsKey("MinusScore"))
        {
            minusTeam.SetScore((int)propertiesThatChanged["MinusScore"]);
        }
    }
    #endregion

    #region Kick Player
    private PlayerData GetPlayerFromID(string _playerID, out string _team)
    {
        var a = addTeam.HavePlayerAndGet(_playerID);
        var b = minusTeam.HavePlayerAndGet(_playerID);


        if (a != null)
        {
            _team = "ADD";
            return a;
        }
        else if (b != null)
        {
            _team = "MINUS";
            return b;
        }
        else
        {
            _team = "NONE";
            return null;
        }
    }


    private void FindPlayerForKick(string _playerID)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        PlayerData playerData = GetPlayerFromID(_playerID, out var _team);

        if (playerData == null) return;

        if (_team == "ADD")
            addTeam.RemovePlayer(playerData);
        if (_team == "MINUS")
            minusTeam.RemovePlayer(playerData);

        UpdatePlayerListToRoomPorperties();
        photonView.RPC("Kick", playerData.info.Sender);



    }
    [PunRPC]
    private void Kick()
    {
        RoomManager.instace.LeveRoom();
    }


    public void KickPlayer(string _playerID)
    {
        FindPlayerForKick(_playerID);
    }






    #endregion

}

