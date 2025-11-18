using System;
using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;


public struct EndGameScore
{
    public string teamWin;
    public string teamWinScore;
    public string gameScore;
}
public class GameManager : MonoBehaviourPunCallbacks
{

    #region  Variable
    public static GameManager instance;

    [Space]

    [Header("Event")]
    [Space]
    [SerializeField] private GameEvent gameEndEvent;
    [SerializeField] private GameEvent scoreUpdateEvent;
    [SerializeField] private GameEvent playCanvasEvent;
    [SerializeField] private GameEvent gameEndMasterEvent;
    [SerializeField] private GameEvent PlayConnectSound;
    [SerializeField] private GameEvent StopConnectSound;
    [SerializeField] private GameEvent PlayPlaySound;
    [SerializeField] private GameEvent StopPlaySound;
    [SerializeField] private GameEvent PlayEndSound;
    [SerializeField] private GameEvent StopEndSound;
    //--Var
    [Space]
    [SerializeField] private ClickCount currentCickCount;
    [SerializeField] private int scoreForAddTeamWin = 50;



    //--Corutine
    private Coroutine coroutineUpdateScore;
    private Coroutine coroutineTimeUpdate;



    [Header("Value")]
    [Space]
    [SerializeField] private BoolValue gameStart;
    [SerializeField] private IntValue gameScore;
    [SerializeField] private FloatValue gameTimer;
    [SerializeField] StringValue teamWin;
    [SerializeField] private BoolValue isEnterToGame;

    [SerializeField] private BoolValue isMaster;
    [SerializeField] private BoolValue singlePlayer;


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
        gameScore.Value = 0;


    }


    #endregion

    #region Setup Event
    private void UpdateGameStartToRoomProperties(bool _data)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {ValueName.GAME_START,gameStart.Value}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
    // Call with Event
    public void UpdateTimer(Component _sender, object _time)
    {

        gameTimer.Value = (float)_time;
        if (!PhotonNetwork.IsMasterClient) return;
        // if (!isMaster) return;
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {ValueName.GAME_TIME,gameTimer.Value}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    private void UpdateScoreGame(int _score)
    {
        //        scoreUpdateEvent.Raise(this, _score);
        if (!PhotonNetwork.IsMasterClient) return;
        ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {ValueName.GAME_SCORE, _score}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomScore);
    }

    private void UpdateTeamWin(string _name)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {ValueName.TEAM_WIN,_name}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomScore);
    }
    private bool isSetupEvents = false;
    // call in OnJoinRoom Function
    public void SetupEvents()
    {

        //   gameStart.OnValueChange += StartUpdateScore;
        if (PhotonNetwork.IsMasterClient)
        {

            isSetupEvents = true;
            gameStart.OnValueChange += UpdateGameStartToRoomProperties;

            gameScore.OnValueChange += UpdateScoreGame;

            teamWin.OnValueChange += UpdateTeamWin;
        }

    }

    public void ResetSetupEvents()
    {
        if (isSetupEvents)
        {
            gameStart.OnValueChange -= UpdateGameStartToRoomProperties;

            gameScore.OnValueChange -= UpdateScoreGame;

            teamWin.OnValueChange -= UpdateTeamWin;
        }
    }


    // Call With Event
    public void Reset()
    {
        Debug.Log("Game Reset");
        gameScore.Value = 0;
        currentCickCount.Reset();
        gameStart.Value = false;
        gameTimer.Value = 0;
        coroutineUpdateScore = null;
        coroutineTimeUpdate = null;
        StopAllCoroutines();
        SetupEvents();

    }

    [PunRPC]
    private void CallAllReSet()
    {

        //   ResetGameEvent?.Invoke();

    }

    #endregion

    #region Update Game Time

    // Call with Event
    public void ChackGameTime(Component _sender, object _time)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var timer = (float)_time;
            if (timer <= 0)
            {
                gameTimer.Value = 0;
                //   Debug.Log(";;;;;;;;;;;;;;;;;;;");
                gameStart.Value = false;

                GameEnd();
            }
        }
    }

    private void GameEnd()
    {
        Debug.Log("Game End");

        if (!PhotonNetwork.IsMasterClient) return;
        gameStart.Value = false;
        if (gameScore.Value > 0)
        {

            teamWin.Value = ValueName.ADD_TEAM;
        }
        else
        {
            teamWin.Value = ValueName.MINUS_TEAM;
        }
        gameEndMasterEvent.Raise(this, -999);
        photonView.RPC("ReceiveGameEnd", RpcTarget.All);
    }
    [PunRPC]
    private void ReceiveGameEnd()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            gameEndEvent.Raise(this, -999);
        }
    }

    #endregion

    #region UI Button Funtion

    //--Call in Button UI
    #endregion

    #region Phton Function And PropertiesUpdate
    //-- if not master this function will receive data form master
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (!isEnterToGame.Value) return;
        if (!PhotonNetwork.IsMasterClient || singlePlayer.Value)
        {
            if (propertiesThatChanged.ContainsKey(ValueName.GAME_SCORE))
            {
                gameScore.Value = (int)propertiesThatChanged[ValueName.GAME_SCORE];
            }

            if (propertiesThatChanged.ContainsKey(ValueName.GAME_TIME))
            {
                gameTimer.Value = (float)propertiesThatChanged[ValueName.GAME_TIME];
            }

            if (propertiesThatChanged.ContainsKey(ValueName.GAME_START))
            {
                gameStart.Value = (bool)propertiesThatChanged[ValueName.GAME_START];
                if (gameStart.Value)
                {
                    StopConnectSound.Raise(this, -999);
                    StopEndSound.Raise(this, -999);
                    PlayPlaySound.Raise(this, -999);
                }

            }

            if (propertiesThatChanged.ContainsKey(ValueName.TEAM_WIN))
            {
                teamWin.Value = (string)propertiesThatChanged[ValueName.TEAM_WIN];
                StopPlaySound.Raise(this, -999);
                PlayEndSound.Raise(this, -999);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SetupEvents();
    }
    #endregion

    #region UpdateScore
    // Call with Event
    public void RequstClickScore(Component _sender, object _data)
    {

        photonView.RPC("SendClickScore", RpcTarget.Others);
        Debug.Log("RequstClickScore");
    }

    [PunRPC]
    private void SendClickScore()
    {
        if (currentCickCount.CurrentClickCount == 0) return;
        int currentClick = currentCickCount.CurrentClickCount;
        currentCickCount.SetCurrentClick(0);
        string teamType = "Nope";
        teamType = TeamManager.instance.MyTeamType;
        ScoreSend data = new ScoreSend()
        {
            scoreType = teamType,
            score = currentClick
        };
        Debug.Log($"Send click {data.score} for {data.scoreType}");
        var ScoreSendJson = JsonUtility.ToJson(data);
        photonView.RPC("ReceiveClickScore", RpcTarget.MasterClient, ScoreSendJson);
        Debug.Log("Below Send");
    }

    [PunRPC]
    private void ReceiveClickScore(string _jsonData)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ScoreSend data = JsonUtility.FromJson<ScoreSend>(_jsonData);
            Debug.Log($"Receive Click:{_jsonData}");
            if (data.scoreType == ValueName.ADD_TEAM)
            {
                var gamescore = this.gameScore.Value;
                gamescore += data.score;
                //  gamescore = Mathf.Clamp(gamescore, -100, 100);
                this.gameScore.Value = gamescore;
            }
            else if (data.scoreType == ValueName.MINUS_TEAM)
            {
                int gamescore = this.gameScore.Value;
                gamescore -= data.score;
                //      gamescore = Mathf.Clamp(gamescore, -100, 100);
                this.gameScore.Value = gamescore;

            }
        }
    }




    #endregion

    #region Update Game Data when Join Team
    public void RequstUpDataGameData()
    {
        photonView.RPC("SendUpDataGameData", RpcTarget.MasterClient);
    }
    [PunRPC]
    private void SendUpDataGameData()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameUpdate gameUpdate = new GameUpdate()
            {
                gameStart = this.gameStart,
                currentScore = this.gameScore.Value
            };

            var jsonData = JsonUtility.ToJson(gameUpdate);
            photonView.RPC("ReceiveUpDataGameData", RpcTarget.AllBuffered, jsonData);

        }
    }

    [PunRPC]
    private void ReceiveUpDataGameData(string _jsonData)
    {
        if (PhotonNetwork.IsMasterClient) return;
        GameUpdate gameUpdate = JsonUtility.FromJson<GameUpdate>(_jsonData);
        gameStart.Value = gameUpdate.gameStart;
        gameScore.Value = gameUpdate.currentScore;
    }


    #endregion

    #region Event Call

    public void AfterJoinTeam(Component _sender, object _data)
    {
        if ((bool)_data)
        {
            playCanvasEvent.Raise(this, -999);
        }
    }


    public void StartScore()
    {
        gameScore.Value = 10;
    }

    public void GameStart()

    {
        gameStart.Value = true;

    }
    #endregion

}
