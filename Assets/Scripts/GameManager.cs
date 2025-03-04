using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    [Header("UI")]
    [SerializeField] private TMP_Text score_text;
    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private TMP_Text clickCount_text;

    public bool isMaster;
    //--Var
    private int score = 0;
    [SerializeField] private float timeToUpdate = 1;
    [SerializeField] private bool gameStart = false;


    private int clickCount = 0;
    private int allClick = 0;




    //--CallBack

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        if (PhotonNetwork.IsMasterClient)
        {


            startGameBTN.SetActive(true);
        }
        else
        {
            startGameBTN.SetActive(false);
        }
    }



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
        score = 0;
        UpdateScoreText();

    }

    void Update()
    {
        UpdateScoreText();
        clickCount_text.text = $"{clickCount}:{allClick}";
    }


    private void UpdateScoreText()
    {
        score_text.text = score.ToString();
    }
    private void UpdateScoreText(string _score)
    {
        score_text.text = _score;
    }

    private Coroutine timerCORU;
    private void StartCO()
    {
        Debug.Log("111");
        if (timerCORU != null) return;
        timerCORU = StartCoroutine(Timer());
        Debug.Log("222");
    }

    private IEnumerator Timer()
    {
        while (gameStart)
        {
            yield return new WaitForSeconds(timeToUpdate);
            RequstClickScore();
        }
        timerCORU = null;
    }



    private void RequstClickScore()
    {
        Debug.Log("Update Score");
        photonView.RPC("SendClickScore", RpcTarget.AllBuffered);
    }
    [PunRPC]
    private void SendClickScore()
    {
        if (clickCount == 0) return;
        int currentClick = clickCount;
        clickCount = 0;
        string teamType = "Nope";
        if (TeamManager.instance.MyTeamType == "ADD") { teamType = "ADD"; }
        else if (TeamManager.instance.MyTeamType == "MINUS") { teamType = "MINUS"; }

        ScoreSend data = new ScoreSend()
        {
            scoreType = teamType,
            score = currentClick
        };

        var jsonData = JsonUtility.ToJson(data);
        Debug.Log($"Send:{jsonData}");
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
            }
            else if (data.scoreType == "MINUS")
            {
                this.score -= data.score;
                this.score = Mathf.Clamp(this.score, 0, int.MaxValue);
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
            gameStart = (bool)propertiesThatChanged["GameStart"];
        }
    }








    //--Call in Button UI
    public void StartGame()
    {
        gameStart = true;
        StartCO();
        if (PhotonNetwork.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable()
            {
                {"GameStart",gameStart}
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        }
    }

    //--Call in Button UI
    public void Click()
    {
        if (!gameStart) return;
        clickCount++;
        allClick++;

    }
}
