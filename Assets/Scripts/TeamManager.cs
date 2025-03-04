using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using System;



public class TeamManager : MonoBehaviourPun
{
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
    [SerializeField] private GameObject chooseTeamCanva;
    [SerializeField] private GameObject playCanva;

    //---Team
    [SerializeField] private Team redTeam;
    [SerializeField] private Team blueTeam;


    //Var
    public int AllPLayerCount => redTeam.PlayerCount + blueTeam.PlayerCount;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    void Start()
    {
        playCanva.SetActive(false);

        redTeam = new Team(TeamName.Red, teamSetting);
        blueTeam = new Team(TeamName.Blue, teamSetting);

        btn1.onClick.AddListener(() => RequestJoinTeam(TeamName.Red));
        btn2.onClick.AddListener(() => RequestJoinTeam(TeamName.Blue));
    }

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
                if (!redTeam.HavePlayer(data.playerID))
                {
                    if (!redTeam.TeamFull())
                    {

                        redTeam.AddPlayer(data);

                        photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Complete, "Join Team Complete"));
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
                if (!blueTeam.HavePlayer(data.playerID))
                {
                    if (!blueTeam.TeamFull())
                    {
                        blueTeam.AddPlayer(data);

                        photonView.RPC("ReceiveJoinTeam", _info.Sender, PackJsonData(ResponseState.Complete, "Join Team Complete"));
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

    [PunRPC]
    private void ReceiveJoinTeam(string _reportJson)
    {
        var data = JsonUtility.FromJson<ResponsetData>(_reportJson);
        if (data.responseState == ResponseState.Complete.ToString())
        {
            playCanva.SetActive(true);
            chooseTeamCanva.SetActive(false);
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
            Hashtable playerList = new Hashtable()
            {
                {redTeam.TeamName,redTeam.GetAllPlayerListString()},
                {blueTeam.TeamName,blueTeam.GetAllPlayerListString()}
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(playerList);
        }
    }


    // Utility Funcetion
    private string PackJsonData(Enum _responseState, string responseMessage)
    {
        ResponsetData reportDate;
        reportDate.responseState = _responseState.ToString();
        reportDate.responseMessage = responseMessage;

        return JsonUtility.ToJson(reportDate);
    }
}

public enum ResponseState
{
    Fail,
    Complete
}

public struct ResponsetData
{
    public string responseState;
    public string responseMessage;
}