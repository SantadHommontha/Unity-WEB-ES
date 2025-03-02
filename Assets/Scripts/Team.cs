
using UnityEngine;
using System.Collections.Generic;
using Photon.Realtime;

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
    public Player sender;
}


[System.Serializable]
public class Team
{
    private const int maxPlayer = 4;
    public TeamName teamName;
    public List<PlayerData> playerData = new List<PlayerData>();

   
    public Team(TeamName _teamName)
    {
        teamName = _teamName;
    }

    public bool TeamFull()
    {
        return playerData.Count < maxPlayer;
    }


    public bool HavePlayer(string playerID)
    {
        foreach (var t in playerData)
        {
            if(t.playerID == playerID) return true;
        }
        return false;
    }

    public void AddPlayer(PlayerData _data)
    {
        playerData.Add(_data);
    }

    public void RemovePlayer(PlayerData _data, bool player)
    {
        if(playerData.Contains(_data))
            playerData.Remove(_data);
    }

}
