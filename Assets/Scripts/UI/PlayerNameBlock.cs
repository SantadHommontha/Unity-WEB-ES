using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PlayerNameBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button kick_Btn;
    [SerializeField] private TMP_Text playerName_txt;
    private string playerID = "NOPE";

    [SerializeField] private GameEvent KickEvent;
    [Header("Value")]
    [SerializeField] private StringValue stringValue;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        kick_Btn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    private void SetEvent()
    {
        //  GameManager.instance.ResetGameEvent += () => kick_Btn.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    void Start()
    {
        SetEvent();
        kick_Btn.onClick.AddListener(KickBTN);
    }
    private void KickBTN()
    {
        if (playerID == "NOPE") return;
        KickEvent.Raise(this, playerID);
      //  TeamManager.instance.KickPlayer(playerID);
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
            string[] data = stringValue.Value.Split(",");

            playerName_txt.text = data[0];
            playerID = data[1];
        }
    }
}
