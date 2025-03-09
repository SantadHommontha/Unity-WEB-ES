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
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text clickCountText;
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private GameObject resetGameBTN;
    [Space]
    [Header("Canva")]
  //  [SerializeField] private GameObject playCanva;
    [Header("GameEnd")]
  //  [SerializeField] private GameObject gameendCanva;
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
   
  //  [SerializeField] private bool gameStart = false;
   /* private bool GameStart
    {
        get
        {
            return gameStart;
        }
        set
        {
       //     gameStart = value;
        //    GameStratChangeEvent?.Invoke(value);
        }
    }*/
    [SerializeField] private float gameTime = 10f;
    [SerializeField] private int scoreForAddTeamWin = 50;
 //   private int score = 0;
   // private float gameTimer = 0;
   
    private int clickCount = 0;
    private int allClick = 0;

    //--Corutine
    private Coroutine coroutineUpdateScore;
    private Coroutine coroutineTimeUpdate; 

    //--Event
    public event Action GameEndEvent;
    public event Action<float> GameTimeUpdateEvent;
  //  public event Action GameStartEvent;
    public event Action GameStopEvent;
    public event Action GameSetupEvent;
   // private Action<bool> GameStratChangeEvent;
    public event Action ResetGameEvent;


    [Header("Value")]
    [Space]
    public BoolValue gameStart;
    public IntValue gameScore;
    public FloatValue gameTimer;
    public 
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
        clickCount = 0;
        gameScore.Value = 0;
        UpdateScoreText();
        SetupEvents();
   //     gameendCanva.SetActive(false);
    }

    void Update()
    {
        UpdateScoreText();
        ClickCountUpdate();
        UpdateTimeText();
    }
    #endregion


    #region Setup Event

    private void OnEnable()
    {
        gameStart.OnValueChange += UpdateGameStartToRoomProperties;
    }
    


    private void UpdateGameStartToRoomProperties(bool _data)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {"GAME_START",gameStart.Value}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
  private void UpdateScoreToRoomProperties(bool _data)
    {
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable()
        {
            {"GAME_SCORE",gameStart.Value}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }
    private void SetupEvents()
    {
        //Setup in Awake
        //GameStart
      //  GameStartEvent += StartTimeCount;
      //  GameStartEvent += StartUpdateScore;
        gameStart.OnValueChange += StartTimeCount;
        gameStart.OnValueChange += StartUpdateScore;
        GameEndEvent += MasterGameEnd;

        ResetGameEvent += Reset;

        //GameStart
      //  GameStart += ()=> { if(!GameStart) }
       // GameSetupEvent 
    }


     private void Reset()
    {

        gameScore.Value = 0;
        clickCount = 0;
        allClick = 0;
        gameStart.Value = false;
        gameTimer.Value = 0;
        coroutineUpdateScore = null;
        coroutineTimeUpdate = null;
        StopAllCoroutines();

       
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
            {
                {"GameStart",gameStart.Value},
                   {"Score", this.gameScore.Value}
            };

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

    }

    [PunRPC]
    private void CallAllReSet()
    {
       
        ResetGameEvent?.Invoke();

    }

    #endregion


    #region UpdateText
    private void UpdateScoreText()
    {
        scoreText.text = gameScore.Value.ToString();
    }
    private void UpdateScoreText(string _score)
    {
        scoreText.text = _score;
    }
    private void ClickCountUpdate()
    {
        clickCountText.text = $"{clickCount}:{allClick}";
    }
    private void UpdateTimeText()
    {
        timerTxt.text = gameTimer.ToString();
    }
    #endregion


    #region Update Game Time

    private void StartTimeCount(bool _data)
    {
        if (!_data) return;

        if (coroutineTimeUpdate != null) return;
        gameTimer.Value = gameTime;
        coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(gameTime));
    }
    private IEnumerator GameTimerUpdate(float _gameTime)
    {
        while((gameTimer.Value > 0) && gameStart.Value)
        { 
            yield return new WaitForSeconds(1);
           
            UpdateGameTime(0);
        }
        coroutineTimeUpdate = null;
    }

    private void UpdateGameTime(float _time)
    {
        // Game Time will Update to other Player by OnPhotonSerializeView
        if (PhotonNetwork.IsMasterClient)
        {
            gameTimer.Value--;
            gameTimer.Value = Mathf.Clamp(gameTimer.Value, 0, gameTime);
                      
            if (gameTimer.Value <= 0)
            {
                gameTimer.Value = 0;
                gameStart.Value = false;

                GameStopEvent?.Invoke();

                //photonView.RPC("ReceiveGameEnd", RpcTarget.AllBuffered);
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
        var addScore = TeamManager.instance.AddTeam.Score;
        var minusScore = TeamManager.instance.MinusTeam.Score;

        EndGameScore endGameScore = new EndGameScore()
        {
            gameScore = gameScore.Value
        };

        scoreWin.text = $"Score: {gameScore.Value.ToString()}";
        if (addScore > scoreForAddTeamWin)
        {
            teamWinTxt.text = "ADD Team";
           
            scoreTeam.text = $"Team Score: {addScore.ToString()}";
            endGameScore.teamWin = teamWinTxt.text;
            endGameScore.teamWinScore = addScore;
        }
        else
        {
            teamWinTxt.text = "Minus Team";
            scoreTeam.text = $"Team Score: {minusScore.ToString()}" ;
            endGameScore.teamWin = teamWinTxt.text;
            endGameScore.teamWinScore = minusScore;
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
        if (!PhotonNetwork.IsMasterClient)
        {
            teamWinTxt.text = endGameScore.teamWin;
            scoreWin.text = $"Score: {endGameScore.gameScore.ToString()}";
            scoreTeam.text = $"Team Score: {endGameScore.teamWinScore.ToString()}";
        }


    }

    #endregion


    #region UpdateScore
    private void StartUpdateScore(bool _data)
    {
        if (!_data) return;
        if (coroutineUpdateScore != null) return;
        coroutineUpdateScore = StartCoroutine(IEUpdaScore());
       
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
        if (clickCount == 0) return;
        int currentClick = clickCount;
        clickCount = 0;
        string teamType = "Nope";
        if(photonView.IsOwnerActive)
        {

        }
        if (TeamManager.instance.MyTeamType == "ADD") { teamType = "ADD"; }
        else if (TeamManager.instance.MyTeamType == "MINUS") { teamType = "MINUS"; }

        ScoreSend data = new ScoreSend()
        {
            scoreType = teamType,
            score = currentClick
        };

        var jsonData = JsonUtility.ToJson(data);
       // Debug.Log($"Send:{jsonData}");
        photonView.RPC("ReceiveClickScore", RpcTarget.MasterClient, jsonData);
    }

    [PunRPC]
    private void ReceiveClickScore(string _jsonData, PhotonMessageInfo _info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var data = JsonUtility.FromJson<ScoreSend>(_jsonData);
            Debug.Log($"Receive:{_jsonData}");
            if (data.scoreType == "ADD")
            {
                this.gameScore.Value += data.score;
                TeamManager.instance.AddTeam.SetScore(data.score);
            }
            else if (data.scoreType == "MINUS")
            {
                this.gameScore.Value -= data.score;
                this.gameScore.Value = Mathf.Clamp(this.gameScore.Value, 0, int.MaxValue);
                TeamManager.instance.MinusTeam.SetScore(data.score);
            }
            
            photonView.RPC("ReceviceResponse", _info.Sender, Utility.ResponseDataToJson(ResponseState.Complete, "Update You Score To Master"));
            ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {"Score", this.gameScore.Value}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomScore);
        }
    }

    [PunRPC]
    private void ReceviceResponse(string _response)
    {
        ResponsetData data = JsonUtility.FromJson<ResponsetData>(_response);

        if (data.responseState == ResponseState.Complete.ToString())
        {
            //  clickCount = 0;
            UpdateScoreText();
        }
        else
        {
            Debug.LogError(data.responseMessage);
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
        clickCount++;
        allClick++;
    }
    public void ResetBTN()
    {
        //   ResetGameEvent?.Invoke();
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
            UpdateScoreText(score.ToString());
        }

        if (propertiesThatChanged.ContainsKey("GAME_START"))
        {
            gameStart.Value = (bool)propertiesThatChanged["GAME_START"];
            


         //   GameStartEvent?.Invoke();
        }

      
    }

  /*  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
        {
            stream.SendNext(gameTimer);
            stream.SendNext(gameStart.Value);
        }
       else
        {
            UpdateGameTime((float)stream.ReceiveNext());
            gameStart.Value = (bool)stream.ReceiveNext();
        }
    }*/
    #endregion

   

}
