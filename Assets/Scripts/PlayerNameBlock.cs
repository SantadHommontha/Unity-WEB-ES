using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerNameBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button kick_Btn;
    [SerializeField] private TMP_Text playerName_txt;
    [SerializeField] private int index = 0;



    public string teamType = "NOPE";
    private string playerID = "NOPE";

    [Header("Value")]
    [SerializeField] private StringValue stringValue;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        kick_Btn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    private void SetEvent()
    {
        GameManager.instance.ResetGameEvent += () => kick_Btn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    void Start()
    {
        SetEvent();
        kick_Btn.onClick.AddListener(KickBTN);
    }
    private void KickBTN()
    {
        Debug.Log("FFFFFFFFFFFFFFF: "+gameObject.name);
        if (playerID == "NOPE") return;
        TeamManager.instance.KickPlayer(playerID);
    }

    private void Clear()
    {
        playerName_txt.text = "";
        playerID = "";
    }
    public override void OnEnable()
    {
        base.OnEnable();

        if (stringValue.Value == null || stringValue.Value == "")
        {
            Clear();
        }
        else
        {
            PlayerDataForUI playerDataForUI = playerDataForUI = JsonUtility.FromJson<PlayerDataForUI>(stringValue.Value);


            playerName_txt.text = playerDataForUI.playerName;
            playerID = playerDataForUI.playerID;
        }




    }
}
