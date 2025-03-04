using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private TMP_Text score_text;
    [SerializeField] private GameObject startGameBTN;
    [SerializeField] private TMP_Text clickCount_text;

    public bool isMaster;
    //--Var
    private int score = 0;
    private float timeToUpdate = 1;
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







    void Start()
    {
        clickCount = 0;
        score = 0;
        UpdateScoreText();

    }

    void Update()
    {
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
        int currentClick = clickCount;
        clickCount = 0;
        photonView.RPC("ReceiveClickScore", RpcTarget.MasterClient, currentClick);
    }

    [PunRPC]
    private void ReceiveClickScore(int _clickCount, PhotonMessageInfo _info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            score += _clickCount;
            photonView.RPC("ReceviceResponse", _info.Sender, Utility.ResponseDataToJson(ResponseState.Complete, "Update You Score To Master"));
            ExitGames.Client.Photon.Hashtable roomScore = new ExitGames.Client.Photon.Hashtable()
            {
                {"Score", score}
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



    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        if (propertiesThatChanged.ContainsKey("Score"))
        {
            int score = (int)propertiesThatChanged["Score"];
            this.score = score;
            UpdateScoreText(score.ToString());
        }
    }








    //--Call in Button UI
    public void StartGame()
    {
        gameStart = true;
        StartCO();

    }

    //--Call in Button UI
    public void Click()
    {
        clickCount++;
        allClick++;

    }
}
