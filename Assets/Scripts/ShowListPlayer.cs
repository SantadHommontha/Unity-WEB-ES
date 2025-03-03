using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ShowListPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text teamName1;
    [SerializeField] private TMP_Text Team1List;
    [SerializeField] private TMP_Text teamName2;
    [SerializeField] private TMP_Text Team2List;


    public void UpdatePlayerList()
    {

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(TeamName.Red.ToString()))
        {
            var allName = (string)propertiesThatChanged[TeamName.Red.ToString()];
            teamName1.text = "Red";
            Team1List.text = allName;
            Debug.Log($"Red : {allName}");

        }
        if (propertiesThatChanged.ContainsKey(TeamName.Blue.ToString()))
        {
            var allName = (string)propertiesThatChanged[TeamName.Blue.ToString()];
            teamName2.text = "Blue";
            Team2List.text = allName;
            Debug.Log($"Blue : {allName}");
        }
    }
}
