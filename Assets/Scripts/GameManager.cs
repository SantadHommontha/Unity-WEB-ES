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
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance;
    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text clickCountText;
    [SerializeField] private TMP_Text timerTxt;
    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private GameObject resetGameBTN;
    [Space]
    [Header("Canva")]
    [SerializeField] private GameObject playCanva;
    [Header("GameEnd")]
    [SerializeField] private GameObject gameendCanva;
    [SerializeField] private TMP_Text teamWinTxt;
    [SerializeField] private TMP_Text scoreWin;
    [SerializeField] private TMP_Text scoreTeam;

   

    //--Var
    [Space]
    [SerializeField] private float timeToUpdateScore = 0.2f;
    [SerializeField] private bool gameStart = false;
    private bool GameStart
    {
        get
        {
            return gameStart;
        }
        set
        {
            gameStart = value;
            GameStratChangeEvent?.Invoke(value);
        }
    }
    [SerializeField] private float gameTime = 10f;
    [SerializeField] private int scoreForAddTeamWin = 50;
    private int score = 0;
    private float gameTimer = 0;
   
    private int clickCount = 0;
    private int allClick = 0;

    //--Corutine
    private Coroutine coroutineUpdateScore;
    private Coroutine coroutineTimeUpdate; 

    //--Event
    public event Action GameEndEvent;
    public event Action<float> GameTimeUpdateEvent;
    public event Action GameStartEvent;
    public event Action GameStopEvent;
    public event Action GameSetupEvent;
    private Action<bool> GameStratChangeEvent;
    public event Action ResetGameEvent;
    #region Unity Function
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        SetupEvents();
    }

    void Start()
    {
        clickCount = 0;
        score = 0;
        UpdateScoreText();
        gameendCanva.SetActive(false);
    }

    void Update()
    {
        UpdateScoreText();
        ClickCountUpdate();
        UpdateTimeText();
    }
    #endregion
    

    #region Setup Event
    private void SetupEvents()
    {
        //Setup in Awake
        //GameStart
        GameStartEvent += StartTimeCount;
        GameStartEvent += StartUpdateScore;

        GameEndEvent += MasterGameEnd;

        ResetGameEvent += Reset;

        //GameStart
      //  GameStart += ()=> { if(!GameStart) }
       // GameSetupEvent 
    }

    #endregion


    #region UpdateText
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
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

    private void StartTimeCount()
    {
        if (coroutineTimeUpdate != null) return;
        gameTimer = gameTime;
        coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(gameTime));
    }
    private IEnumerator GameTimerUpdate(float _gameTime)
    {
        while((gameTimer > 0) && GameStart)
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
            gameTimer--;
            gameTimer = Mathf.Clamp(gameTimer, 0, gameTime);
                      
            if (gameTimer <= 0)
            {
                gameTimer = 0;
                GameStart = false;

                GameStopEvent?.Invoke();

                //photonView.RPC("ReceiveGameEnd", RpcTarget.AllBuffered);
                MasterGameEnd();
            }
            else
            {
                GameTimeUpdateEvent?.Invoke(gameTimer);
            }
        }
       else
        {
            gameTimer = _time;
            GameTimeUpdateEvent?.Invoke(gameTimer);
        }
        
    }

    private void MasterGameEnd()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        var addScore = TeamManager.instance.AddTeam.Score;
        var minusScore = TeamManager.instance.MinusTeam.Score;

        EndGameScore endGameScore = new EndGameScore()
        {
            gameScore = score
        };

        scoreWin.text = $"Score: {score.ToString()}";
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
        gameendCanva.SetActive(true);
        playCanva.SetActive(false);

        if (!PhotonNetwork.IsMasterClient)
        {
            teamWinTxt.text = endGameScore.teamWin;
            scoreWin.text = $"Score: {endGameScore.gameScore.ToString()}";
            scoreTeam.text = $"Team Score: {endGameScore.teamWinScore.ToString()}";
        }


    }

    #endregion


    #region UpdateScore
    private void StartUpdateScore()
    {
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
                this.score += data.score;
                TeamManager.instance.AddTeam.SetScore(data.score);
            }
            else if (data.scoreType == "MINUS")
            {
                this.score -= data.score;
                this.score = Mathf.Clamp(this.score, 0, int.MaxValue);
                TeamManager.instance.MinusTeam.SetScore(data.score);
            }
            
            photonView.RPC("ReceviceResponse", _info.Sender, Utility.ResponseDataToJson(ResponseState.Complete, "Update You Score To Master"));
            ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {"Score", this.score}
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
                currentScore = this.score
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
        gameStart = gameUpdate.gameStart;
        score = gameUpdate.currentScore;
    }


    #endregion


    #region UI Button Funtion
    //--Call in Button UI
    public void StartGameBTN()
    {
        ResetGameEvent?.Invoke();
        if (PhotonNetwork.IsMasterClient)
        {
            GameStart = true;
            GameStartEvent?.Invoke();
           
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
            {
                {"GameStart",GameStart}
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
           
        }
    }

    //--Call in Button UI
    public void ClickBTN()
    {
        if (!GameStart) return;
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

        if (propertiesThatChanged.ContainsKey("Score"))
        {
            int score = (int)propertiesThatChanged["Score"];
            this.score = score;
            UpdateScoreText(score.ToString());
        }

        if (propertiesThatChanged.ContainsKey("GameStart"))
        {
            GameStart = (bool)propertiesThatChanged["GameStart"];
            GameStartEvent?.Invoke();
        }

      
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
        {
            stream.SendNext(gameTimer);
            stream.SendNext(GameStart);
        }
       else
        {
            UpdateGameTime((float)stream.ReceiveNext());
            GameStart = (bool)stream.ReceiveNext();
        }
    }
    #endregion

    private void Reset()
    {
       
        score = 0;
        clickCount = 0;
        allClick = 0;
        gameStart = false;
        gameTimer = 0;

        StopAllCoroutines();

       
        ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
            {
                {"GameStart",GameStart},
                   {"Score", this.score}
            };

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);

    }

    [PunRPC]
    private void CallAllReSet()
    {
       
        ResetGameEvent?.Invoke();

    }

}
