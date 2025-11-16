using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System;

public class PlayerNameBlock : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button kick_Btn;
    [SerializeField] private TMP_Text playerName_txt;
    public string playerID = "NOPE";

    [SerializeField] private GameEvent KickEvent;
    [Header("Value")]
    [SerializeField] private StringValue playerNameValue;
    [SerializeField] private StringValue playerIdValue;
    [SerializeField] private BoolValue isMaster;
    [SerializeField] private BoolValue finishConnect;


    private void SetEvent()
    {
        kick_Btn.onClick.AddListener(KickBTN);
    }
    void Awake()
    {
        // playerNameValue.OnValueChange
    }
    void Start()
    {
        //     SetEvent();
        //  stringValue.OnValueChange += UpdateText;
        kick_Btn.onClick.AddListener(KickBTN);
    }
    private void KickBTN()
    {
        if (playerID == "") return;
        KickEvent.Raise(this, playerIdValue.Value);
        //  TeamManager.instance.KickPlayer(playerID);
    }

    private void Clear()
    {
        playerName_txt.text = "";
        playerID = "";
    }
    void FixedUpdate()
    {
        if (playerName_txt.text != playerNameValue.Value)
        {
            playerName_txt.text = playerNameValue.Value;
        }

    }

    private void UpdateText(string _text)
    {
        // if (stringValue.Value == null || stringValue.Value == "")
        // {
        //     Clear();
        // }
        // else
        // {
        //     string[] data = stringValue.Value.Split(",");

        //     playerName_txt.text = data[0];
        //     playerID = data[1];
        // }
    }
    public override void OnEnable()
    {
        base.OnEnable();

        // if (finishConnect.Value)
        // {
        //     kick_Btn.gameObject.SetActive(isMaster.Value);
        //     UpdateText("");
        // }






    }
}
