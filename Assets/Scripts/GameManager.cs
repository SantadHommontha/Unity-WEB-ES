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
    [Header("UI")]


    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private GameObject resetGameBTN;
    [Space]


    [Header("Event")]
    [Space]
    [SerializeField] private GameEvent gameEndEvent;
    [SerializeField] private GameEvent gameStartEvent;


    //--Var
    [Space]
    [SerializeField] private float timeToUpdateScore = 0.2f;

    [SerializeField] private ClickCount currentLickCount;
    [SerializeField] private float gameTime = 10f;
    [SerializeField] private int scoreForAddTeamWin = 50;




    //--Corutine
    private Coroutine coroutineUpdateScore;
    private Coroutine coroutineTimeUpdate;

    //--Event
    //   public event Action GameEndEvent;
    public event Action<float> GameTimeUpdateEvent;

    public event Action GameStopEvent;

    // public event Action ResetGameEvent;


    [Header("Value")]
    [Space]
    public BoolValue gameStart;
    public IntValue gameScore;
    public FloatValue gameTimer;
    [SerializeField] StringValue teamWin;
    // [SerializeField] StringValue gameScore;

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
        SetupEvents();

    }


    #endregion


    #region Setup Event

    public override void OnEnable()
    {

        Debug.Log("Enable");
    }



    private void UpdateGameStartToRoomProperties(bool _data)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {ValueName.GAME_START,gameStart.Value}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
    private void UpdateScoreToRoomProperties(bool _data)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {ValueName.GAME_SCORE,gameScore.Value}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
    private void SetupEvents()
    {
        gameStart.OnValueChange += StartTimeCount;
        gameStart.OnValueChange += StartUpdateScore;
        //   GameEndEvent += MasterGameEnd;
        gameStart.OnValueChange += UpdateGameStartToRoomProperties;
        gameScore.OnValueChange += UpdateScoreGame;
        teamWin.OnValueChange += UpdateTeamWin;
        //    ResetGameEvent += Reset;
    }


    private void Reset()
    {

        gameScore.Value = 0;
        currentLickCount.Reset();
        gameStart.Value = false;
        gameTimer.Value = 0;
        coroutineUpdateScore = null;
        coroutineTimeUpdate = null;
        StopAllCoroutines();


        // ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
        //     {
        //         {ValueName.GAME_START,gameStart.Value},
        //         {ValueName.GAME_SCORE, this.gameScore.Value}
        //     };

        // PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

    }

    [PunRPC]
    private void CallAllReSet()
    {

        //   ResetGameEvent?.Invoke();

    }

    #endregion

    #region Update Game Time

    private void StartTimeCount(bool _data)
    {
        if (_data)
        {
            if (coroutineTimeUpdate != null) return;
            gameTimer.Value = gameTime;
            coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(gameTime));
        }
        else
        {
            if (coroutineTimeUpdate != null)
                StopCoroutine(coroutineTimeUpdate);
            coroutineTimeUpdate = null;
        }

    }
    private IEnumerator GameTimerUpdate(float _gameTime)
    {
        while ((gameTimer.Value > 0) && gameStart.Value)
        {
            yield return new WaitForSeconds(1);

            UpdateGameTime(0);
        }
        coroutineTimeUpdate = null;
    }

    private void UpdateGameTime(float _time)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameTimer.Value--;
            gameTimer.Value = Mathf.Clamp(gameTimer.Value, 0, gameTime);

            if (gameTimer.Value <= 0)
            {
                gameTimer.Value = 0;
                gameStart.Value = false;

                //  GameStopEvent?.Invoke();
                GameEnd();
            }
            else
            {
                GameTimeUpdateEvent?.Invoke(gameTimer.Value);
            }
        }
        else
        {
            gameTimer.Value = _time;
            GameTimeUpdateEvent?.Invoke(gameTimer.Value);
        }

    }

    private void GameEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        EndGameScore endGameScore = new EndGameScore()
        {
            gameScore = gameScore.Value.ToString()
        };

        if (gameScore.Value > scoreForAddTeamWin)
        {
            teamWin.Value = "ADD TEAM WIN";
        }
        else
        {
            teamWin.Value = "MINUS TEAM WIN";
        }

        photonView.RPC("ReceiveGameEnd", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void ReceiveGameEnd()
    {
        gameEndEvent.Raise(this, -999);

    }

    #endregion


    #region UpdateScore
    private void StartUpdateScore(bool _data)
    {
        if (_data)
        {
            if (coroutineUpdateScore != null) return;

            coroutineUpdateScore = StartCoroutine(IEUpdaScore());

        }

        else
        {
            if (coroutineUpdateScore != null)
            {
                StopCoroutine(coroutineUpdateScore);
                coroutineUpdateScore = null;

            }
        }
    }

    private IEnumerator IEUpdaScore()
    {
          
        while (gameStart)
        {
            yield return new WaitForSeconds(timeToUpdateScore);
            RequstClickScore();
        }
        coroutineUpdateScore = null;
    }



    private void RequstClickScore()
    {
        
        photonView.RPC("SendClickScore", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void SendClickScore()
    {
        if (currentLickCount.CurrentClickCount == 0) return;
        int currentClick = currentLickCount.CurrentClickCount;
        currentLickCount.SetCurrentClick(0);
        string teamType = "Nope";
        teamType = TeamManager.instance.MyTeamType;
        ScoreSend data = new ScoreSend()
        {
            scoreType = teamType,
            score = currentClick
        };

        var jsonData = JsonUtility.ToJson(data);
        photonView.RPC("ReceiveClickScore", RpcTarget.MasterClient, jsonData);
    }

    [PunRPC]
    private void ReceiveClickScore(string _jsonData, PhotonMessageInfo _info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var data = JsonUtility.FromJson<ScoreSend>(_jsonData);
            Debug.Log($"Receive:{_jsonData}");
            if (data.scoreType == ValueName.ADD_TEAM)
            {
                this.gameScore.Value += data.score;

            }
            else if (data.scoreType == ValueName.MINUS_TEAM)
            {
                this.gameScore.Value -= data.score;
                this.gameScore.Value = Mathf.Clamp(this.gameScore.Value, 0, int.MaxValue);

            }
            //       photonView.RPC("ReceviceResponse", _info.Sender, Utility.ResponseDataToJson(ResponesState.COMPLETE, "Update You Score To Master"));
        }
    }

    [PunRPC]
    private void ReceviceResponse(string _response)
    {
        Utility.ResponseDataFromJson(_response, out var _state, out var _mess);

        if (_state == ResponesState.COMPLETE)
        {
            //  clickCount = 0;
            //     UpdateScoreText();
        }
        else
        {
            Debug.LogError(_mess);
        }

    }

    public void UpdateScoreGame(int _score)
    {
        ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {ValueName.GAME_SCORE, _score}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomScore);
    }

    public void UpdateTeamWin(string _name)
    {
        ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {ValueName.TEAM_WIN,_name}
            };
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomScore);
        Debug.Log($"JKJ {PhotonNetwork.CurrentRoom.CustomProperties[ValueName.TEAM_WIN]}");
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


    #region UI Button Funtion
    //--Call in Button UI
    public void StartGameBTN()
    {
        //   ResetGameEvent?.Invoke();
        if (PhotonNetwork.IsMasterClient)
        {
            gameStart.Value = true;
        }
    }

    //--Call in Button UI
    public void ClickBTN()
    {
        if (!gameStart.Value) return;
        currentLickCount.Click();
    }
    public void ResetBTN()
    {

        Reset();

        //photonView.RPC("CallAllReSet", RpcTarget.AllBuffered);
    }
    #endregion


    #region Phton Function And PropertiesUpdate
    //--CallBack
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
        {
            startGameBTN.SetActive(true);
            resetGameBTN.SetActive(true);
        }
        else
        {
            startGameBTN.SetActive(false);
            resetGameBTN.SetActive(false);
        }
    }
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if (propertiesThatChanged.ContainsKey(ValueName.GAME_SCORE))
        {
            gameScore.Value = (int)propertiesThatChanged[ValueName.GAME_SCORE];
        }

        if (propertiesThatChanged.ContainsKey(ValueName.GAME_START))
        {
            gameStart.Value = (bool)propertiesThatChanged[ValueName.GAME_START];
        }

        if (propertiesThatChanged.ContainsKey(ValueName.TEAM_WIN))
        {
            teamWin.Value = (string)propertiesThatChanged[ValueName.TEAM_WIN];
        }


    }

    #endregion



}
