
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ShowListPlayer : MonoBehaviourPunCallbacks
{
    public static ShowListPlayer instance;
    [SerializeField] private PlayerNameBlock[] addTeam;
    [SerializeField] private PlayerNameBlock[] minusTeam;

    void Awake()
    {
        addTeam = new PlayerNameBlock[5];
        minusTeam = new PlayerNameBlock[5];

        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }
    public void AddPlayerBlock(int _idex, PlayerNameBlock _block)
    {
        if (_block.teamType == "ADD")
        {
            addTeam[_idex] = _block;
        }
        else if (_block.teamType == "MINUS")
        {
            minusTeam[_idex] = _block;
        }
        else
        {

        }
    }
    public void UpdatePlayerList()
    {
        if (addTeam[0] == null) return;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("ADD_TEAM_PLAYER_LIST"))
        {

            PlayerTeamList playerTeamList = JsonUtility.FromJson<PlayerTeamList>((string)PhotonNetwork.CurrentRoom.CustomProperties["ADD_TEAM_PLAYER_LIST"]);

            // foreach (var t in playerTeamList.playerID)
            // {
            //     Debug.Log($"ADD{} {t} ");
            //   
            // }
            for (int i = 0; i < playerTeamList.playerID.Length; i++)
            {

                addTeam[i].SetPlayer(playerTeamList.playerName[i], playerTeamList.playerID[i]);

            }
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("MINUS_TEAM_PLAYER_LIST"))
        {
            PlayerTeamList playerTeamList = JsonUtility.FromJson<PlayerTeamList>((string)PhotonNetwork.CurrentRoom.CustomProperties["MINUS_TEAM_PLAYER_LIST"]);
          
            for (int i = 0; i < playerTeamList.playerID.Length; i++)
            {
                minusTeam[i].SetPlayer(playerTeamList.playerName[i], playerTeamList.playerID[i]);
            }
        }
    }



    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        UpdatePlayerList();
        // if (propertiesThatChanged.ContainsKey(TeamName.Blue.ToString()))
        // {
        //     var allName = (string)propertiesThatChanged[TeamName.Blue.ToString()];
        //     teamName2.text = "Blue";
        //     Team2List.text = allName;
        //     Debug.Log($"Blue : {allName}");
        // }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();


        UpdatePlayerList();

    }
}
