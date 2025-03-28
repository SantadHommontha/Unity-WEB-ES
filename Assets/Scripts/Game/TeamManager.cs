using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class TeamManager : MonoBehaviourPunCallbacks
{
    #region  Variable
    public static TeamManager instance;
    [SerializeField] private int maxTeamCount = 3;
    [Header("Team Setting")]

    //--UI
    [Space]
    [SerializeField] private TMP_InputField enterNameInput;
    [SerializeField] private TMP_InputField enterRoomCodeInput;
    [SerializeField] private Button btn1, btn2;
    [SerializeField] private TMP_Text reportError;

    //--Canva
    [Space]

    [Header("Event")]
    [SerializeField] private GameEvent chooseTeamEven;
    [SerializeField] private GameEvent playEvent;
    [SerializeField] private GameEvent afterJoinTeamComplete;
    [SerializeField] private GameEvent UpdatePlayerList;
    [SerializeField] private GameEvent kickEvent;
    [SerializeField] private GameEvent KickedOutEvent;

    [Space]

    private Team team = new Team();
    private PlayerData myPlayerData;
    public PlayerData MyPlayerData => myPlayerData;


    private Team myTeam;
    public Team MyTeam => myTeam;


    [Header("Value")]
    [SerializeField] private StringValue reSpones;
    [SerializeField] private StringValue roomCode;
    [SerializeField] private BoolValue isEnterToGame;
    [SerializeField] private StringValue myName;
    public string MyName => myName.Value;
    [SerializeField] private StringValue myTeamType;
    public string MyTeamType => myTeamType.Value;
    // [Space]
    [SerializeField] private int maxPlayerInTeam = 3;
    private int addTeamCount;
    private int minusTeamCount;
    #endregion

    #region  GameEvent
    private void SetupEvent()
    {
        team.OnPlayerTeamChange += UpdatePlayerListToRoomPorperties;
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
        SetupEvent();


        btn1.onClick.AddListener(() => RequestJoinTeam(ValueName.ADD_TEAM));
        btn2.onClick.AddListener(() => RequestJoinTeam(ValueName.MINUS_TEAM));


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (var v in team.GetAllPlayer())
            {
                Debug.Log("V " + v.playerName);
            }
        }
    }
    #endregion


    #region Join Team

    // send playerdata to master to join team
    private void RequestJoinTeam(string _teamName)
    {
        PlayerData playerData = new PlayerData()
        {
            info = new PhotonMessageInfo(),
            playerID = PhotonNetwork.LocalPlayer.UserId,
            teamName = _teamName,
            playerName = enterNameInput.text == "" ? $"PLayer{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}" : enterNameInput.text,
            clickCount = 0,
            code = roomCode.Value
        };
        string jsonData = JsonUtility.ToJson(playerData);
        myName.Value = enterNameInput.text == "" ? $"PLayer{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}" : enterNameInput.text;
        photonView.RPC("TryJoinTeam", RpcTarget.MasterClient, jsonData);
    }

    // receive player data at RequestJoinTeam Funcetion Send
    [PunRPC]
    private void TryJoinTeam(string _jsonData, PhotonMessageInfo _info)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var data = JsonUtility.FromJson<PlayerData>(_jsonData);
        data.info = _info;

        if (data.playerName == ValueName.ADMIN_NAME)
        {
            RoomManager.instace.ChangeMaster(data.info.Sender);

            return;
        }


        if (data.code == roomCode.Value && data.code != "")
        {
            if (data.teamName == ValueName.ADD_TEAM)
            {

                if (addTeamCount < maxTeamCount && team.TryToAddPlayer(data))
                {
                    // add Complete
                    Debug.Log($"PLayer Join Add Team:{data.playerName} {data.playerID}");
                    addTeamCount++;
                    photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.COMPLETE, "Join Team Complete", ValueName.ADD_TEAM));

                }
                else
                {
                    // fail to addFD
                    photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Team Have Full"));
                }
            }
            else if (data.teamName == ValueName.MINUS_TEAM)
            {

                if (minusTeamCount < maxTeamCount && team.TryToAddPlayer(data))
                {
                    // add Complete
                    Debug.Log($"PLayer Join Minus Team:{data.playerName} {data.playerID}");
                    minusTeamCount++;
                    photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.COMPLETE, "Join Team Complete", ValueName.MINUS_TEAM));
                }
                else
                {
                    // fail to add
                    photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Team Have Full"));
                }
            }
            else
            {
                // fail not have this team name
                photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Not Have this Team Name"));
            }
        }

        else
        {
            photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Invalid code"));
        }

    }


    [PunRPC]
    private void ReceiveJoinTeam(string _reportJson)
    {
        var data = JsonUtility.FromJson<SendResponseJoinData>(_reportJson);
        if (data.responseState == ResponesState.COMPLETE)
        {
            myTeamType.Value = data.myTeamType;
            isEnterToGame.Value = true;
            afterJoinTeamComplete.Raise(this, true);
        }
        else if (data.responseState == ResponesState.FAIL)
        {
            reportError.text = data.responseMessage;
            isEnterToGame.Value = false;
            afterJoinTeamComplete.Raise(this, false);
        }
        reSpones.Value = data.responseMessage;
        //   photonView.RPC("UpDatePlayerDate", RpcTarget.MasterClient);
    }



    // Update When Player List In Team Change
    private void UpdatePlayerListToRoomPorperties()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            int length = team.GetAllPlayer().Length;
            List<PlayerData> at = new List<PlayerData>();
            List<PlayerData> mt = new List<PlayerData>();

            foreach (var v in team.GetAllPlayer())
            {
                if (v.teamName == ValueName.ADD_TEAM)
                {
                    at.Add(v);
                }

                else if (v.teamName == ValueName.MINUS_TEAM)
                {
                    mt.Add(v);
                }
            }

            TwoStringArrayDataJson a = new TwoStringArrayDataJson()
            {
                value1 = new string[at.Count],
                value2 = new string[at.Count]
            };
            TwoStringArrayDataJson m = new TwoStringArrayDataJson()
            {
                value1 = new string[mt.Count],
                value2 = new string[mt.Count]
            };
            int num = 0;
            foreach (var v in at)
            {
                a.value1[num] = at[num].playerName;
                a.value2[num] = at[num].playerID;
                num++;
            }
            num = 0;
            foreach (var v in mt)
            {
                m.value1[num] = mt[num].playerName;
                m.value2[num] = mt[num].playerID;
                num++;
            }

            var aJson = JsonUtility.ToJson(a);
            var mJson = JsonUtility.ToJson(m);

            ExitGames.Client.Photon.Hashtable playerListSS = new ExitGames.Client.Photon.Hashtable()
        {
            {ValueName.ADD_TEAM_PLAYER_LIST,aJson},
            {ValueName.MINUS_TEAM_PLAYER_LIST,mJson}
        };

            PhotonNetwork.CurrentRoom.SetCustomProperties(playerListSS);
        }
        else
        {
            UpdatePlayerList.Raise(this, -999);
        }
    }
    #endregion


    #region Utility
    // Utility Funcetion
    private string PackJsonData(string _responseState, string _responseMessage, string _teamType)
    {
        SendResponseJoinData sendResponseJoinData = new SendResponseJoinData()
        {
            responseState = _responseState.ToString(),
            responseMessage = _responseMessage,
            myTeamType = _teamType
        };
        return JsonUtility.ToJson(sendResponseJoinData);
    }
    private string PackJsonData(string _responseState, string _responseMessage)
    {
        return PackJsonData(_responseState, _responseMessage, "Nope");
    }


    #endregion


    #region PullDataFromRoomProperties
    // Fetch data from a room when joining a room without being the master
    public void PullData()
    {

        List<PlayerData> pd = new List<PlayerData>();
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        {
            var aJson = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.ADD_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson a = JsonUtility.FromJson<TwoStringArrayDataJson>(aJson);
            for (int i = 0; i < a.value1.Length; i++)
            {
                pd.Add(new PlayerData()
                {
                    playerName = a.value1[i],
                    playerID = a.value2[i]
                });
            }
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        {
            var mJson = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.MINUS_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson m = JsonUtility.FromJson<TwoStringArrayDataJson>(mJson);
            for (int i = 0; i < m.value1.Length; i++)
            {
                pd.Add(new PlayerData()
                {
                    playerName = m.value1[i],
                    playerID = m.value2[i]
                });
            }
        }

        team.SetPlayerData(pd.ToArray());
    }

    #endregion



    #region  Photon Function

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (!PhotonNetwork.IsMasterClient)
        {
            PullData();


        }
    }

    #endregion


    #region Kick Player


    private void FindPlayerForKick(string _playerID)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!team.HavePlayer(_playerID, out var _player)) return;
        team.RemovePlayer(_playerID);

        photonView.RPC("Kick", _player.info.Sender);

    }
    // Call From FindPlayerForKick 
    [PunRPC]
    private void Kick()
    {
        KickedOutEvent.Raise(this, -999);
    }

    public void KickPlayer(Component _sender, object _playerID)
    {

        FindPlayerForKick((string)_playerID);
    }
    // Call With kick Event
    public void KickAllPlayer()
    {
        var ids = team.GetAllPlayerID();

        foreach (var id in ids)
        {

            FindPlayerForKick(id);
        }

    }
    // Call With LeaveGame Event
    public void LeaveGame(Component _sender, object _playerID)
    {
        // Debug.Log("LeaveGame Called by: " + _sender.name + " | PlayerID: " + _playerID);
        if (_playerID is int && (int)_playerID == -999) return;
        photonView.RPC("ReviesLeaveGame", RpcTarget.MasterClient, (string)_playerID);

    }

    [PunRPC]
    private void ReviesLeaveGame(string _playerID)
    {
        FindPlayerForKick(_playerID);
    }


    public void ClearAll()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            team.ClearAll();
            photonView.RPC("KickServer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void KickServer()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            RoomManager.instace.DisconnectServer();
        }
    }

    #endregion

    #region Event Call

    public void ResetIsEnterTogame()
    {
        isEnterToGame.Value = false;
    }
    #endregion
    #region  Reset
    public void Reset()
    {
        addTeamCount = 0;
        minusTeamCount = 0;
    }
    #endregion
}

