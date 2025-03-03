using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;



public class TeamManager : MonoBehaviourPun
{

    //--UI
    [Space]
    [SerializeField] private TMP_InputField enterNameInput;
    [SerializeField] private Button btn1, btn2;

    //--Canva
    [Space]
    [SerializeField] private GameObject chooseTeamCanva;
    [SerializeField] private GameObject playCanva;


    //---Team
    [SerializeField] private Team redTeam = new Team(TeamName.Red);
    [SerializeField] private Team blueTeam = new Team(TeamName.Blue);




    void Start()
    {
        playCanva.SetActive(false);


        btn1.onClick.AddListener(() => RequestJoinTeam(TeamName.Red));
        btn2.onClick.AddListener(() => RequestJoinTeam(TeamName.Blue));
    }




    private void RequestJoinTeam(TeamName _teamName)
    {
        var playerData = new PlayerData();
        playerData.playerName = enterNameInput.text == "" ? $"PLayer{PhotonNetwork.LocalPlayer.ActorNumber.ToString()}" : enterNameInput.text;
        playerData.playerID = PhotonNetwork.LocalPlayer.UserId;
        playerData.sender = PhotonNetwork.LocalPlayer;
        playerData.teamName = _teamName;

        var jsonData = JsonUtility.ToJson(playerData);

        photonView.RPC("TryJoinTeam", RpcTarget.MasterClient, jsonData);


    }

    [PunRPC]
    private void TryJoinTeam(string _jsonData)
    {
        if (!PhotonNetwork.IsMasterClient) return;


        var data = JsonUtility.FromJson<PlayerData>(_jsonData);
        ReportDate reportDate;
        switch (data.teamName)
        {
            case TeamName.Red:
                if (!redTeam.HavePlayer(data.playerID))
                {
                    if (!redTeam.TeamFull())
                    {
                        redTeam.AddPlayer(data);

                        reportDate.ReportFail = false;
                        reportDate.ReportText = "Join Player Complete";

                        photonView.RPC("ReceiveAddPlayer", data.sender, reportDate);
                    }
                    else
                    {
                        reportDate.ReportFail = true;
                        reportDate.ReportText = "Team Have Full";

                        photonView.RPC("ReceiveAddPlayer", data.sender, reportDate);
                    }
                }
                else
                {

                    reportDate.ReportFail = true;
                    reportDate.ReportText = "You Have Join In Red Team";

                    photonView.RPC("ReceiveAddPlayer", data.sender, reportDate);
                }

                break;
            case TeamName.Blue:
                if (!blueTeam.HavePlayer(data.playerID))
                {
                    if (!blueTeam.TeamFull())
                    {
                        blueTeam.AddPlayer(data);

                        reportDate.ReportFail = false;
                        reportDate.ReportText = "Join Player Complete";

                        photonView.RPC("ReceiveAddPlayer", data.sender, reportDate);
                    }
                    else
                    {
                        reportDate.ReportFail = true;
                        reportDate.ReportText = "Team Have Full";

                        photonView.RPC("ReceiveAddPlayer", data.sender, reportDate);
                    }
                }
                else
                {
                    reportDate.ReportFail = true;
                    reportDate.ReportText = "You Have Join In Blue Team";

                    photonView.RPC("ReceiveAddPlayer", data.sender, reportDate);
                }


                break;
        }



    }
    [PunRPC]
    private void ReceiveAddPlayer(string _reportJson)
    {
        var data = JsonUtility.FromJson<ReportDate>(_reportJson);
        if (!data.ReportFail)
        {
            //  playCanva.SetActive(true);
            //   chooseTeamCanva.SetActive(false);
            Debug.Log(data.ReportText);
        }
        else
        {
            Debug.Log(data.ReportText);
        }
    }
}


public struct ReportDate
{
    public bool ReportFail;
    public string ReportText;
}