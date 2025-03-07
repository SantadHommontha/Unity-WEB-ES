using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine.UI;
public class PlayerNameBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button kick_Btn;
    [SerializeField] private TMP_Text playerName_txt;
    [SerializeField] private int index = 0;
    public string teamType = "NOPE";
    private string playerID = "NOPE";
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        kick_Btn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }


    private void SetEvent()
    {
        GameManager.instance.ResetGameEvent += () => kick_Btn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }


    public void SetPlayer(string _playerName, string _playerID)
    {
        playerID = _playerID;
        playerName_txt.text = _playerName;
    }

    void Awake()
    {



        SetEvent();
    }
    void Start()
    {
        ShowListPlayer.instance.AddPlayerBlock(index, this);
        kick_Btn.onClick.AddListener(KickBTN);
    }
    private void KickBTN()
    {
        if (teamType == "NOPE" || playerID == "NOPE") return;
        TeamManager.instance.KickPlayer(playerID);
    }
}
