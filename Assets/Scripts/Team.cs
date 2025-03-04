
using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;
using System;
using Photon.Pun;

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
    [SerializeField] private List<PlayerData> playerDatas = new List<PlayerData>();
    public int PlayerCount => playerDatas.Count;

    public Team(TeamName _teamName, TeamSetting _teamSetting)
    {
        teamName = _teamName;
        teamSetting = _teamSetting;


        maxPlayer = teamSetting.MaxPlayer;
    }

   

    public bool TeamFull()
    {
        return playerDatas.Count >= maxPlayer;
    }


    public bool HavePlayer(string playerID)
    {
        foreach (var t in playerDatas)
        {
            if (t.playerID == playerID) return true;
        }
        return false;
    }

    public void AddPlayer(PlayerData _data)
    {
        playerDatas.Add(_data);
    }

    public void RemovePlayer(PlayerData _data, bool player)
    {
        if (playerDatas.Contains(_data))
            playerDatas.Remove(_data);
    }

    public String GetAllPlayerListString()
    {
        string allName = "";

        foreach (var t in playerDatas)
        {
            allName += $"{t.playerName}" + "\n";
        }
        return allName;
    }

}
