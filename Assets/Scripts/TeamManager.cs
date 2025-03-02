using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class TeamManager : MonoBehaviourPun
{

    //--UI
    [Space]
    [SerializeField] private TMP_InputField enterNameInput;
    [SerializeField] private Button btn1,btn2;

    //--Canva
    [Space]
    [SerializeField] private GameObject chooseTeamCanva;
    [SerializeField] private GameObject playCanva;


    //---Team
    [SerializeField]  private Team redTeam = new Team(TeamName.Red);
    [SerializeField]  private Team blueTeam = new Team(TeamName.Blue);




    void Start()
    {
        playCanva.SetActive(false);
   

        btn1.onClick.AddListener(() => RequestAddPlayer(TeamName.Red));
        btn2.onClick.AddListener(() => RequestAddPlayer(TeamName.Blue));
    }

  
  

   private void RequestAddPlayer(TeamName _teamName)
    {
        var playerData =  new PlayerData();
        playerData.playerName = enterNameInput.text;
        playerData.playerID = PhotonNetwork.LocalPlayer.UserId;
        playerData.sender = PhotonNetwork.LocalPlayer;
        playerData.teamName = _teamName;
        photonView.RPC("TryAddPlayer", RpcTarget.MasterClient, playerData);


    }
    
    [PunRPC]
    private void TryAddPlayer(PlayerData _data)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        Debug.Log(_data);
        switch(_data.teamName)
        {
            case TeamName.Red:
              if (!redTeam.HavePlayer(_data.playerID))
                {
                    redTeam.AddPlayer(_data);
                
                    photonView.RPC("ReceiveAddPlayer", _data.sender, true);
                }
              else
                {
                    photonView.RPC("ReceiveAddPlayer", _data.sender, false);
                }

            break;
            case TeamName.Blue:
                if (!blueTeam.HavePlayer(_data.playerID))
                {
                    blueTeam.AddPlayer(_data);
                  
                    photonView.RPC("ReceiveAddPlayer", _data.sender, true);
                }
                else
                {
                    photonView.RPC("ReceiveAddPlayer", _data.sender, false);
                }


                break;
        }



    }
    [PunRPC]
    private void ReceiveAddPlayer(bool _isComplete)
    {
        if (_isComplete)
        {
          //  playCanva.SetActive(true);
         //   chooseTeamCanva.SetActive(false);
            Debug.Log("Add PLayer Complete");
        }
        else
        {
            Debug.Log("Fail To Add Player");
        }
    }
}
