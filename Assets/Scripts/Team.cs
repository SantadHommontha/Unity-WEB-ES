
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
    public int PlayerCount => platerDatass.Count;
    [SerializeField] private int score;
    private Dictionary<string, PlayerData> platerDatass = new Dictionary<string, PlayerData>();
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
        return platerDatass.Count >= maxPlayer;
    }


    public bool HavePlayer(string _playerID)
    {
        // foreach (var t in playerDatas)
        // {
        //     if (t.playerID == playerID) return true;
        // }

        return platerDatass.ContainsKey(_playerID);
    }

    public PlayerData HavePlayerAndGet(string _playerID)
    {
        // foreach (var t in playerDatas)
        // {
        //     if (t.playerID == playerID) return t;
        // }

        if (platerDatass.ContainsKey(_playerID))
        {
            return platerDatass[_playerID];
        }
        return null;
    }

    public PlayerData AddPlayer(PlayerData _data)
    {
        //    playerDatas.Add(_data);
        platerDatass.Add(_data.playerID, _data);
        return _data;
    }

    public void RemovePlayer(PlayerData _data, bool player)
    {
        // if (playerDatas.Contains(_data))
        //     playerDatas.Remove(_data);

        if (platerDatass.ContainsKey(_data.playerID))
        {
            platerDatass.Remove(_data.playerID);
        }
    }

    public String GetAllPlayerListString()
    {
        string allName = "";

        // foreach (var t in playerDatas)
        // {
        //     allName += $"{t.playerName}" + "\n";
        // }

        foreach (var t in platerDatass)
        {
            allName += $"{t.Value.playerName}" + "\n";
        }
        return allName;
    }

    public void Reset()
    {
        score = 0;
    }
}
