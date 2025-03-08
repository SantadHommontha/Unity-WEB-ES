
using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using System;
using Photon.Pun;
using UnityEngine.InputSystem.Utilities;
using Unity.VisualScripting;

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
    // [SerializeField] private List<PlayerData> playerDatas = new List<PlayerData>();
    public int PlayerCount => playerDate.Count;
    [SerializeField] private int score;
    private Dictionary<string, PlayerData> playerDate = new Dictionary<string, PlayerData>();
    public int Score => score;
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

    public bool TeamFull()
    {
        return playerDate.Count >= maxPlayer;
    }


    public bool HavePlayer(string _playerID)
    {
        // foreach (var t in playerDatas)
        // {
        //     if (t.playerID == playerID) return true;
        // }

        return playerDate.ContainsKey(_playerID);
    }

    public PlayerData HavePlayerAndGet(string _playerID)
    {
        // foreach (var t in playerDatas)
        // {
        //     if (t.playerID == playerID) return t;
        // }

        if (playerDate.ContainsKey(_playerID))
        {
            return playerDate[_playerID];
        }
        return null;
    }

    public PlayerData AddPlayer(PlayerData _data)
    {
        //    playerDatas.Add(_data);
        playerDate.Add(_data.playerID, _data);
        return _data;
    }

    public void RemovePlayer(PlayerData _data)
    {
        // if (playerDatas.Contains(_data))
        //     playerDatas.Remove(_data);

        if (playerDate.ContainsKey(_data.playerID))
        {
            playerDate.Remove(_data.playerID);
        }
    }

    public String GetAllPlayerListString()
    {
        string allName = "";

        // foreach (var t in playerDatas)
        // {
        //     allName += $"{t.playerName}" + "\n";
        // }

        foreach (var t in playerDate)
        {
            allName += $"{t.Value.playerName}" + "\n";
        }
        return allName;
    }

    public void Reset()
    {
        score = 0;
    }
    public string GetAllPlayerToPlayerTeamList()
    {
        PlayerTeamList p;
        p.playerName = new string[maxPlayer];
        p.playerID = new string[maxPlayer];
        int i = 0;

        foreach (var t in playerDate)
        {
            p.playerName[i] = t.Value.playerName;
            p.playerID[i] = t.Value.playerID;
            i++;
        }

        return JsonUtility.ToJson(p);
    }
}
