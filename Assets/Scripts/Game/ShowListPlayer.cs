
using System.Collections;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class ShowListPlayer : MonoBehaviourPunCallbacks
{
    public static ShowListPlayer instance;

    [SerializeField] private StringValue[] addTeam;
    [SerializeField] private StringValue[] minusTeam;
    [SerializeField] private StringValue[] addTeamID;
    [SerializeField] private StringValue[] minusTeamID;
    [SerializeField] private BoolValue finishConnectToServer;

    private Coroutine IE_UpdatePlayerlist;
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

    }


    void Start()
    {
        // TeamManager.instance.MyTeam.OnPlayerTeamChange += PlayerListUpdate2;
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        //  Debug.Log("111");
        // if (propertiesThatChanged.ContainsKey(ValueName.ADD_TEAM_PLAYER_LIST))
        // {
        //     var aJson = (string)propertiesThatChanged[ValueName.ADD_TEAM_PLAYER_LIST];
        //     TwoStringArrayDataJson a = JsonUtility.FromJson<TwoStringArrayDataJson>(aJson);
        //     //  Debug.Log("Length a " + aJson);
        //     //    Debug.Log("222");
        //     foreach (var v in addTeam)
        //     {
        //         v.Value = "";
        //     }
        //     for (int i = 0; i < a.value1.Length; i++)
        //     {

        //         addTeam[i].Value = $"{a.value1[i]},{a.value2[i]}";

        //     }
        // }
        // if (propertiesThatChanged.ContainsKey(ValueName.MINUS_TEAM_PLAYER_LIST))
        // {
        //     var mJson = (string)propertiesThatChanged[ValueName.MINUS_TEAM_PLAYER_LIST];
        //     TwoStringArrayDataJson m = JsonUtility.FromJson<TwoStringArrayDataJson>(mJson);
        //     // Debug.Log("Length m " + mJson);
        //     //   Debug.Log("333");
        //     foreach (var v in minusTeam)
        //     {
        //         v.Value = "";
        //     }
        //     for (int j = 0; j < m.value1.Length; j++)
        //     {
        //         minusTeam[j].Value = $"{m.value1[j]},{m.value2[j]}";

        //     }
        // }
      
    }

    public void StartUpdatePLayerList()
    {
        Debug.Log("StartUpdatePLayerList");
        if (PhotonNetwork.IsMasterClient)
        {
            if (IE_UpdatePlayerlist != null)
                StopCoroutine(IE_UpdatePlayerlist);
            IE_UpdatePlayerlist = StartCoroutine(IE_UpdatePlayerList());
        }

    }
    public void StopUpdatePLayerList()
    {
        StopCoroutine(IE_UpdatePlayerlist);
        IE_UpdatePlayerlist = null;
    }
    private IEnumerator IE_UpdatePlayerList()
    {
        while (true)

        {
            yield return new WaitForSeconds(1);
            PlayerListUpdate2();
        }

    }
    void Update()
    {
           PlayerListUpdate2();
    }
    private void PlayerListUpdate2()
    {
        if (!finishConnectToServer.Value) return;
        if (PhotonNetwork.CurrentRoom == null) return;
        if (TeamManager.instance == null) return;
        Debug.Log("UpdatePlayerList");
        TeamManager.instance.MyTeam.GetAllPlayerByTeam(out var _teamAdd, out var _teamMinus);
        
        for (int i = 0; i < 3; i++)
        {
            if (i < _teamAdd.Length)
            {
                addTeam[i].Value = _teamAdd[i].playerName;
                addTeamID[i].Value = _teamAdd[i].playerID;
            }
            else
            {
                addTeam[i].Value = "";
                addTeamID[i].Value = "";
            }

            if (i < _teamMinus.Length)
            {
                minusTeam[i].Value = _teamMinus[i].playerName;
                minusTeamID[i].Value = _teamMinus[i].playerID;
            }
            else
            {
                minusTeam[i].Value = "";
                minusTeamID[i].Value = "";
            }
        }

    }
    public void PlayerListUpdate()
    {
        if (!finishConnectToServer.Value) return;
        if (PhotonNetwork.CurrentRoom == null) return;
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
        //   PlayerListUpdate();

        //  UpdatePlayerList();

    }
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            // PlayerListUpdate();
        }
    }
}
