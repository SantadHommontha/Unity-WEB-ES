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
    [SerializeField] private BoolValue isMaster;
    [SerializeField] private BoolValue finishConnect;


    private void SetEvent()
    {
        kick_Btn.onClick.AddListener(KickBTN);
    }

    void Start()
    {
        SetEvent();
        stringValue.OnValueChange += UpdateText;
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


    private void UpdateText(string _text)
    {
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
    public override void OnEnable()
    {
        base.OnEnable();

        if (finishConnect.Value)
        {
            kick_Btn.gameObject.SetActive(isMaster);
            UpdateText("");
        }






    }
}
