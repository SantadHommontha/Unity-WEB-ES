
using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using System;
using Photon.Pun;
using UnityEngine.InputSystem.Utilities;
using Unity.VisualScripting;
using System.Linq;

public enum TeamName
{
    Red,
    Blue
}
[System.Serializable]
public class PlayerData
{
    public string playerName;
    public string playerID;
    public TeamName teamName;
    public PhotonMessageInfo info;
}


[System.Serializable]
public class Team
{

    private TeamSetting teamSetting;


    [Space]
    private int maxPlayer = 0;
    [SerializeField] private TeamName teamName;
    public string TeamName => teamName.ToString();
    public int PlayerCount => playerdata.Count;
    [SerializeField] private int score;
    private Dictionary<string, PlayerData> playerdata = new Dictionary<string, PlayerData>();
    public int Score => score;
    public Action<PlayerData[]> OnPlayerTeamChange;
    public Team(TeamName _teamName, TeamSetting _teamSetting)
    {
        teamName = _teamName;
        teamSetting = _teamSetting;


        maxPlayer = teamSetting.MaxPlayer;
    }

    public void SetScore(int _score)
    {
        score += _score;
    }
    public void Reset()
    {
        score = 0;
    }
    public bool TeamFull()
    {
        return playerdata.Count >= maxPlayer;
    }

    #region  HavePlayer
    public bool HavePlayer(string _playerID)
    {
        return playerdata.ContainsKey(_playerID);
    }

    public PlayerData HavePlayerAndGet(string _playerID)
    {
        if (playerdata.ContainsKey(_playerID))
        {
            return playerdata[_playerID];
        }
        return null;
    }
    #endregion

    #region  Add Player And Remove Player
    public PlayerData AddPlayer(PlayerData _data)
    {
        playerdata.Add(_data.playerID, _data);
        OnPlayerTeamChange?.Invoke(GetAllPlayerData());
        return _data;
    }

    public void RemovePlayer(PlayerData _data)
    {
        if (playerdata.ContainsKey(_data.playerID))
        {
            playerdata.Remove(_data.playerID);
            OnPlayerTeamChange?.Invoke(GetAllPlayerData());
        }
    }
    #endregion



    #region Get Player Player Data
    public String GetAllPlayerListString()
    {
        string allName = "";
        foreach (var t in playerdata)
        {
            allName += $"{t.Value.playerName}" + "\n";
        }
        return allName;
    }


    public string GetAllPlayerToPlayerTeamList()
    {
        PlayerTeamList p;
        p.playerName = new string[maxPlayer];
        p.playerID = new string[maxPlayer];
        int i = 0;

        foreach (var t in playerdata)
        {
            p.playerName[i] = t.Value.playerName;
            p.playerID[i] = t.Value.playerID;
            i++;
        }

        return JsonUtility.ToJson(p);
    }

    public PlayerData[] GetAllPlayerData()
    {
        PlayerData[] playerDatas = new PlayerData[playerdata.Count];
        int num = 0;
        foreach (var t in playerdata)
        {
            playerDatas[num] = t.Value;
            num++;
        }
        return playerDatas;

    }
    #endregion
}
