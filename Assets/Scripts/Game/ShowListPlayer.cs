
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class ShowListPlayer : MonoBehaviourPunCallbacks
{
    public static ShowListPlayer instance;

    [SerializeField] private StringValue[] addTeam;
    [SerializeField] private StringValue[] minusTeam;
    [SerializeField] private BoolValue finishConnectToServer;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        //  Debug.Log("111");
        if (propertiesThatChanged.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        {
            var aJson = (string)propertiesThatChanged[ValueName.ADD_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson a = JsonUtility.FromJson<TwoStringArrayDataJson>(aJson);
            //  Debug.Log("Length a " + aJson);
            //    Debug.Log("222");
            foreach (var v in addTeam)
            {
                v.Value = "";
            }
            for (int i = 0; i < a.value1.Length; i++)
            {

                addTeam[i].Value = $"{a.value1[i]},{a.value2[i]}";

            }
        }
        if (propertiesThatChanged.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        {
            var mJson = (string)propertiesThatChanged[ValueName.MINUS_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson m = JsonUtility.FromJson<TwoStringArrayDataJson>(mJson);
            // Debug.Log("Length m " + mJson);
            //   Debug.Log("333");
            foreach (var v in minusTeam)
            {
                v.Value = "";
            }
            for (int j = 0; j < m.value1.Length; j++)
            {
                minusTeam[j].Value = $"{m.value1[j]},{m.value2[j]}";

            }
        }
    }

    public void PlayerListUpdate()
    {
        if (!finishConnectToServer.Value) return;
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        {
            var aJson = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.ADD_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson a = JsonUtility.FromJson<TwoStringArrayDataJson>(aJson);
            foreach (var v in addTeam)
            {
                v.Value = "";
            }
            for (int i = 0; i < a.value1.Length; i++)
            {
                addTeam[i].Value = $"{a.value1[i]},{a.value2[i]}";

            }
        }
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        {
            var mJson = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.MINUS_TEAM_PLAYER_LIST];
            TwoStringArrayDataJson m = JsonUtility.FromJson<TwoStringArrayDataJson>(mJson);
            foreach (var v in minusTeam)
            {
                v.Value = "";
            }
            for (int i = 0; i < m.value1.Length; i++)
            {
                minusTeam[i].Value = $"{m.value1[i]},{m.value2[i]}";
            }
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PlayerListUpdate();

        //  UpdatePlayerList();

    }
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            PlayerListUpdate();
        }
    }
}
