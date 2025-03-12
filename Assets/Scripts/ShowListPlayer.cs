
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ShowListPlayer : MonoBehaviourPunCallbacks
{
    public static ShowListPlayer instance;
    // [SerializeField] private PlayerNameBlock[] addTeam;
    // [SerializeField] private PlayerNameBlock[] minusTeam;

    [SerializeField] private StringValue[] addTeam;
    [SerializeField] private StringValue[] minusTeam;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }
    void Start()
    {
    //    TeamManager.instance.AddTeam.OnPlayerTeamChange += UpdatePlayerAddTeam;
    //    TeamManager.instance.MinusTeam.OnPlayerTeamChange += UpdatePlayerMinusTeam;
    }

    public void UpdatePlayerAddTeam(PlayerData[] _playerDatas)
    {
        for (int i = 0; i < _playerDatas.Length; i++)
        {
            PlayerDataForUI playerDataForUI = new PlayerDataForUI()
            {
                playerName = _playerDatas[i].playerName,
                playerID = _playerDatas[i].playerID
            };

            addTeam[i].Value = JsonUtility.ToJson(playerDataForUI);
        }
    }
    public void UpdatePlayerMinusTeam(PlayerData[] _playerDatas)
    {
        for (int i = 0; i < _playerDatas.Length; i++)
        {
            PlayerDataForUI playerDataForUI = new PlayerDataForUI()
            {
                playerName = _playerDatas[i].playerName,
                playerID = _playerDatas[i].playerID
            };

            minusTeam[i].Value = JsonUtility.ToJson(playerDataForUI);
        }
    }
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        // UpdatePlayerList();
        // // if (propertiesThatChanged.ContainsKey(TeamName.Blue.ToString()))
        // // {
        // //     var allName = (string)propertiesThatChanged[TeamName.Blue.ToString()];
        // //     teamName2.text = "Blue";
        // //     Team2List.text = allName;
        // //     Debug.Log($"Blue : {allName}");
        // // }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();


        //  UpdatePlayerList();

    }
}
