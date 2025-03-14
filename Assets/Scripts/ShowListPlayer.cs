
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class ShowListPlayer : MonoBehaviourPunCallbacks
{
    public static ShowListPlayer instance;

    [SerializeField] private StringValue[] addTeam;
    [SerializeField] private StringValue[] minusTeam;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        {
            var aJson = (string)propertiesThatChanged[ValueName.ADD_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson a = JsonUtility.FromJson<TwoStringArrayDataJson>(aJson);
            for (int i = 0; i < a.value1.Length; i++)
            {
                Debug.Log($"==={a.value1[i]},{a.value2[i]}===");
                addTeam[i].Value = $"{a.value1[i]},{a.value2[i]}";
            }
        }
        if (propertiesThatChanged.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        {
            var mJson = (string)propertiesThatChanged[ValueName.MINUS_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson m = JsonUtility.FromJson<TwoStringArrayDataJson>(mJson);
            for (int i = 0; i < m.value1.Length; i++)
            {
                minusTeam[i].Value = $"{m.value1[i]},{m.value2[i]}";
            }
        }
    }

    public void PlayerListUpdate()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        {
            var aJson = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.ADD_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson a = JsonUtility.FromJson<TwoStringArrayDataJson>(aJson);
            for (int i = 0; i < a.value1.Length; i++)
            {
                Debug.Log($"==={a.value1[i]},{a.value2[i]}===");
                addTeam[i].Value = $"{a.value1[i]},{a.value2[i]}";
            }
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        {
            var mJson = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.MINUS_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson m = JsonUtility.FromJson<TwoStringArrayDataJson>(mJson);
            for (int i = 0; i < m.value1.Length; i++)
            {
                minusTeam[i].Value = $"{m.value1[i]},{m.value2[i]}";
            }
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();


        //  UpdatePlayerList();

    }
}
