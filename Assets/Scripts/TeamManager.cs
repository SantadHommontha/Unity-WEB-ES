using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System.Collections.Generic;

public class TeamManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region  Variable
    public static TeamManager instance;
    [SerializeField] private int maxTeamCount = 3;
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
    [SerializeField] private GameEvent playEvent;
    [Space]
    //---Team
    //[SerializeField] private Team addTeam;
    // [SerializeField] private Team minusTeam;
    private Team team = new Team();
    private PlayerData myPlayerData;
    public PlayerData MyPlayerData => myPlayerData;
    private string myName;
    public string MyName => myName;
    private string myTeamType;
    public string MyTeamType => myTeamType;

    private Team myTeam;
    public Team MyTeam => myTeam;
    //  public Team AddTeam => addTeam;
    //   public Team MinusTeam => minusTeam;
    //Var
    //  public int AllPLayerCount => addTeam.PlayerCount + minusTeam.PlayerCount;


    [SerializeField] private StringValue[] addTeamUI;
    [SerializeField] private StringValue[] minusTeamUI;
    [SerializeField] private StringValue reSpones;


    #endregion

    #region  GameEvent
    private void SetupEvent()
    {
        GameManager.instance.GameStopEvent += GameEnd;

        GameManager.instance.ResetGameEvent += Reset;

        team.OnPlayerTeamChange += UpdatePlayerListToRoomPorperties;


    }

    private void Reset()
    {
        // AddTeam.Reset();
        //  minusTeam.Reset();
    }

    private void GameEnd()
    {
        /*  if (PhotonNetwork.IsMasterClient)
          {
              ExitGames.Client.Photon.Hashtable teamScore = new Hashtable()
              {
                  { "AddScore", AddTeam.Score },
                  { "MinusScore", MinusTeam.Score },
              };
              PhotonNetwork.CurrentRoom.SetCustomProperties(teamScore);
          }*/
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
        //   addTeam = new Team(TeamName.Red, teamSetting);
        //  minusTeam = new Team(TeamName.Blue, teamSetting);

        btn1.onClick.AddListener(() => RequestJoinTeam(ValueName.ADD_TEAM));
        btn2.onClick.AddListener(() => RequestJoinTeam(ValueName.MINUS_TEAM));

        //    addTeam.OnPlayerTeamChange += UpdatePlayerListToRoomPorperties;
        //   minusTeam.OnPlayerTeamChange += UpdatePlayerListToRoomPorperties;

        //  addTeam.OnPlayerTeamChange += UpdatePlayerAddTeam;
        //  minusTeam.OnPlayerTeamChange += UpdatePlayerMinusTeam;
    }

    void Update()
    {
        /*  if (Input.GetKeyDown(KeyCode.A))
          {
              foreach (var value in addTeam.GetAllPlayerData())
              {
                  Debug.Log("ADD TEAM: " + value.playerName);
              }
          }
          if (Input.GetKeyDown(KeyCode.M))
          {
              foreach (var value in minusTeam.GetAllPlayerData())
              {
                  Debug.Log("MINUS TEAM: " + value.playerName);
              }
          }*/
    }
    #endregion


    #region Join Team


    private void RequestJoinTeam(string _teamName)
    {
        PlayerData playerData = new PlayerData()
        {
            info = new PhotonMessageInfo(),
            playerID = PhotonNetwork.LocalPlayer.UserId,
            teamName = _teamName,
            playerName = enterNameInput.text == "" ? $"PLayer{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}" : enterNameInput.text,
            clickCount = 0

        };
        /*   playerData.playerName = enterNameInput.text == "" ? $"PLayer{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}" : enterNameInput.text;
           playerData.playerID = PhotonNetwork.LocalPlayer.UserId;
           playerData.info = new PhotonMessageInfo();
           playerData.teamName = _teamName;*/

        string jsonData = JsonUtility.ToJson(playerData);
        Debug.Log(jsonData);
        photonView.RPC("TryJoinTeam", RpcTarget.MasterClient, jsonData);
    }

    [PunRPC]
    private void TryJoinTeam(string _jsonData, PhotonMessageInfo _info)
    {
        Debug.Log("fffffff " + _jsonData);


        if (!PhotonNetwork.IsMasterClient) return;

        var data = JsonUtility.FromJson<PlayerData>(_jsonData);
        data.info = _info;




        var add_team = ValueName.ADD_TEAM;
        var minus_team = ValueName.MINUS_TEAM;

        if (data.teamName == add_team)
        {
            //  Debug.Log("11111");
            if ((team.AddTeamCount() <= maxTeamCount) && team.TryToAddPlayer(data))
            {
                // add Complete

                Debug.Log($"PLayer Join Add Team:{data.playerName} {data.playerID}");
                //      photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.COMPLETE, "Join Team Complete", ValueName.MINUS_TEAM));

            }
            else
            {
                // fail to add
                //   Debug.Log("3333");
                //       photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Team Have Full"));
            }
        }
        else if (data.teamName == minus_team)
        {

            if ((team.MinusTeamCount() <= maxTeamCount) && team.TryToAddPlayer(data))
            {
                // add Complete
                Debug.Log($"PLayer Join Minus Team:{data.playerName} {data.playerID}");
                //      photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.COMPLETE, "Join Team Complete", ValueName.MINUS_TEAM));
            }
            else
            {
                // fail to add
                //      photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Team Have Full"));
            }
        }
        else
        {
            //    Debug.Log("4444");
            // fail not have this team name
            //   photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponesState.FAIL, "Not Have this Team Name"));
        }

        //    Debug.Log("5555");







        /* switch (data.teamName)
         {
             case TeamName.Red:
                 if (!addTeam.HavePlayer(data.playerID))
                 {
                     if (!addTeam.TeamFull())
                     {

                         addTeam.AddPlayer(data);
                         myTeam = addTeam;
                         Debug.Log($"PLayer Join AddTeam: {data.playerID}");
                         //         UpdatePlayerListToRoomPorperties();
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
                         //      UpdatePlayerListToRoomPorperties();
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
         }*/
    }


    [PunRPC]
    private void ReceiveJoinTeam(string _reportJson)
    {
        Debug.Log("ReceiveJoinTeam 1111");
        var data = JsonUtility.FromJson<SendResponseJoinData>(_reportJson);
        if (data.responseState == ResponesState.COMPLETE)
        {

            playEvent.Raise(this, this);
            myTeamType = data.myTeamType;
            // AfterJoinTeam();
            Debug.Log("ReceiveJoinTeam TRUE");
            //  UpdatePlayerListToRoomPorperties();
            GameManager.instance.RequstUpDataGameData();
        }
        else if (data.responseState == ResponesState.FAIL)
        {
            Debug.Log("ReceiveJoinTeam FALSE");
            reportError.text = data.responseMessage;
        }
        reSpones.Value = data.responseMessage;

        Debug.Log("ReceiveJoinTeam 22222");
        //   photonView.RPC("UpDatePlayerDate", RpcTarget.MasterClient);
    }
    // private void UpdatePlayerListToRoomPorperties()
    // {
    //     if (!PhotonNetwork.IsMasterClient) return;

    //     var a = addTeam.GetAllPlayerToPlayerTeamList();
    //     var b = minusTeam.GetAllPlayerToPlayerTeamList();
    //     ExitGames.Client.Photon.Hashtable playerList = new ExitGames.Client.Photon.Hashtable()
    //     {
    //         {ValueName.ADD_TEAM_PLAYER_LIST,a},
    //         {ValueName.MINUS_TEAM_PLAYER_LIST,b}
    //     };

    //     PhotonNetwork.CurrentRoom.SetCustomProperties(playerList);

    // }


    // Update When Player List In Team Change
    private void UpdatePlayerListToRoomPorperties()
    {

        if (!PhotonNetwork.IsMasterClient) return;

        List<PlayerData> a = new List<PlayerData>();
        List<PlayerData> m = new List<PlayerData>();
        foreach (var v in team.GetAllPlayer())
        {
            Debug.Log("L: " + v.playerName);
        }
        /*  foreach (var v in team.GetAllPlayer())
          {
              Debug.Log("11111");
              if (v.teamName == ValueName.ADD_TEAM)
              {
                  a.Add(v);
                  Debug.Log("2222");
              }

             else if (v.teamName == ValueName.MINUS_TEAM)
              {
                  m.Add(v);
                  Debug.Log("33333");
              }
          }
          */
        ExitGames.Client.Photon.Hashtable playerListSS = new ExitGames.Client.Photon.Hashtable()
        {
            {ValueName.ADD_TEAM_PLAYER_LIST,125}
          //  {ValueName.MINUS_TEAM_PLAYER_LIST,m.ToArray()}
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(playerListSS);
        playEvent.Raise(this, this);
    }

    /*  public void UpdatePlayerAddTeam(PlayerData[] _playerDatas)
      {
          foreach (var t in addTeamUI) t.Clear();

          for (int i = 0; i < _playerDatas.Length; i++)
          {
              PlayerDataForUI playerDataForUI = new PlayerDataForUI()
              {
                  playerName = _playerDatas[i].playerName,
                  playerID = _playerDatas[i].playerID
              };

              addTeamUI[i].Value = JsonUtility.ToJson(playerDataForUI);
          }
      }*/

    /*  public void UpdatePlayerMinusTeam(PlayerData[] _playerDatas)
      {
           foreach (var t in minusTeamUI) t.Clear();
          for (int i = 0; i < _playerDatas.Length; i++)
          {
              PlayerDataForUI playerDataForUI = new PlayerDataForUI()
              {
                  playerName = _playerDatas[i].playerName,
                  playerID = _playerDatas[i].playerID
              };
              minusTeamUI[i].Clear();
              minusTeamUI[i].Value = JsonUtility.ToJson(playerDataForUI);
          }
      }
    */



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
            //        UpdatePlayerListToRoomPorperties();
        }
    }


    private void AfterJoinTeam()
    {


    }
    #endregion


    #region Utility
    // Utility Funcetion
    private string PackJsonData(string _responseState, string _responseMessage, string _teamType)
    {
        // ResponsetData reportDate;
        //  reportDate.responseState = _responseState.ToString();
        //  reportDate.responseMessage = _responseMessage;

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
    // Pull Data From Room 
    public void PullData()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        {

        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        {

        }
    }

    #endregion



    #region  Photon Function
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }



    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        Debug.Log("dEBUG.LOG");
        /*   if (PhotonNetwork.IsMasterClient) return;
           if (propertiesThatChanged.ContainsKey("AddScore"))
           {
               addTeam.SetScore((int)propertiesThatChanged["AddScore"]);
           }
           if (propertiesThatChanged.ContainsKey("MinusScore"))
           {
               minusTeam.SetScore((int)propertiesThatChanged["MinusScore"]);
           }*/
    }
    #endregion

    #region Kick Player
    /* private PlayerData GetPlayerFromID(string _playerID, out string _team)
     {
         /*  var a = addTeam.HavePlayerAndGet(_playerID);
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
         _team = "EIEI";
         return null;
     }*/


    private void FindPlayerForKick(string _playerID)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        //   PlayerData playerData = GetPlayerFromID(_playerID, out var _team);

        if (!team.HavePlayer(_playerID, out var _player)) return;



        //     UpdatePlayerListToRoomPorperties();
        photonView.RPC("Kick", _player.info.Sender);



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


    public int GetAddTeamScore()
    {
        return 0;
    }

    public int GetMinusTeamScore()
    {
        return 0;
    }

    #endregion

}

