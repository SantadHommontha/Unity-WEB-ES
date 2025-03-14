using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public struct EndGameScore
{
    public string teamWin;
    public int teamWinScore;
    public int gameScore;
}
public class GameManager : MonoBehaviourPunCallbacks
{
    #region  Variable
    public static GameManager instance;
    [Header("UI")]


    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private GameObject resetGameBTN;
    [Space]
  
 
    [SerializeField] private TMP_Text teamWinTxt;
    [SerializeField] private TMP_Text scoreWin;
    [SerializeField] private TMP_Text scoreTeam;

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
    public event Action GameEndEvent;
    public event Action<float> GameTimeUpdateEvent;

    public event Action GameStopEvent;
    public event Action GameSetupEvent;

    public event Action ResetGameEvent;


    [Header("Value")]
    [Space]
    public BoolValue gameStart;
    public IntValue gameScore;
    public FloatValue gameTimer;

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
        GameEndEvent += MasterGameEnd;
        gameStart.OnValueChange += UpdateGameStartToRoomProperties;
        gameScore.OnValueChange += UpdateScoreGame;
        ResetGameEvent += Reset;
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


        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
            {
                {ValueName.GAME_START,gameStart.Value},
                {ValueName.GAME_SCORE, this.gameScore.Value}
            };

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

    }

    [PunRPC]
    private void CallAllReSet()
    {

        ResetGameEvent?.Invoke();

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

                GameStopEvent?.Invoke();
                MasterGameEnd();
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

    private void MasterGameEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        var addScore = TeamManager.instance.GetAddTeamScore();
        var minusScore = TeamManager.instance.GetMinusTeamScore();

        EndGameScore endGameScore = new EndGameScore()
        {
            gameScore = gameScore.Value
        };

        if (gameScore.Value > scoreForAddTeamWin)
        {
            endGameScore.teamWin = "ADD TEAM WIN";
        }
        else
        {
            endGameScore.teamWin = "MINUS TEAM WIN";
        }

        var jsonData = JsonUtility.ToJson(endGameScore);

        photonView.RPC("GameEnd", RpcTarget.AllBuffered, jsonData);
    }
    [PunRPC]
    private void ReceiveGameEnd()
    {
        GameEndEvent?.Invoke();
    }
    [PunRPC]
    private void GameEnd(string _scoreDataJson)
    {
        EndGameScore endGameScore = JsonUtility.FromJson<EndGameScore>(_scoreDataJson);

        gameEndEvent.Raise(this, this);

        teamWinTxt.text = endGameScore.teamWin;
        scoreWin.text = $"Score: {endGameScore.gameScore.ToString()}";
        //    scoreTeam.text = $"Team Score: {endGameScore.teamWinScore.ToString()}";



    }

    #endregion


    #region UpdateScore
    private void StartUpdateScore(bool _data)
    {
        if (!_data)
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
        // Debug.Log("Update Score");
        photonView.RPC("SendClickScore", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void SendClickScore()
    {
        if (currentLickCount.CurrentClickCount == 0) return;
        int currentClick = currentLickCount.CurrentClickCount;
        currentLickCount.SetCurrentClick(0);
        string teamType = "Nope";

        //  if (TeamManager.instance.MyTeamType == "ADD") { teamType = "ADD"; }
        //  else if (TeamManager.instance.MyTeamType == "MINUS") { teamType = "MINUS"; }
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
                //   TeamManager.instance.AddTeam.SetScore(data.score);
            }
            else if (data.scoreType == ValueName.MINUS_TEAM)
            {
                this.gameScore.Value -= data.score;
                this.gameScore.Value = Mathf.Clamp(this.gameScore.Value, 0, int.MaxValue);
                //      TeamManager.instance.MinusTeam.SetScore(data.score);
            }

            photonView.RPC("ReceviceResponse", _info.Sender, Utility.ResponseDataToJson(ResponesState.COMPLETE, "Update You Score To Master"));

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
        ResetGameEvent?.Invoke();
        if (PhotonNetwork.IsMasterClient)
        {
            gameStart.Value = true;
            //   GameStartEvent?.Invoke();

            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
            {
                {"GameStart",gameStart.Value}
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

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
        photonView.RPC("CallAllReSet", RpcTarget.AllBuffered);
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

        if (propertiesThatChanged.ContainsKey("GAME_SCORE"))
        {
            int score = (int)propertiesThatChanged["GAME_SCORE"];
            this.gameScore.Value = score;
        }

        if (propertiesThatChanged.ContainsKey("GAME_START"))
        {
            gameStart.Value = (bool)propertiesThatChanged["GAME_START"];
        }


    }

    #endregion



}
