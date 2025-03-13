using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public struct ChangePlayerData
{
    public string playerTargetID;
    public string playerName;
    public string teamName;
    public int clickCount;
}
public class PlayerData2
{
    public PhotonMessageInfo info;
    public string playerID;
    public string teamName;
    public string playerName;
    public int clickCount;
}
public class TeamData 
{
    private TeamSetting teamSetting;

    
    // string is playerID
    private Dictionary<string, PlayerData2> playerdata = new Dictionary<string, PlayerData2>();
    public Action<PlayerData2[]> OnPlayerTeamChange;

    #region ChangeData

    public void Change(ChangePlayerData _playerData)
    {
        if(HavePlayer(_playerData.playerTargetID,out var _player))
        {
            if(_playerData.playerName != null)
            {
                _player.playerName = _playerData.playerName;
            }
            if(_playerData.teamName != null)
            {
                _player.teamName = _playerData.teamName;
            }

        }
    }

    #endregion
    #region  Add Player And Remove Player
    public bool TryToAddPlayer(PlayerData2 _data)
    {
        if(!HavePlayer(_data.playerID))
        {
            AddPlayer(_data);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddPlayer(PlayerData2 _data)
    {
        if (!playerdata.ContainsKey(_data.playerID))
        {
            playerdata.Add(_data.playerID, _data);
            OnPlayerTeamChange?.Invoke(GetAllPlayer());
        }
    }

    public void RemovePlayer(PlayerData2 _data)
    {
        if (playerdata.ContainsKey(_data.playerID))
        {
            playerdata.Remove(_data.playerID);
            OnPlayerTeamChange?.Invoke(GetAllPlayer());
        }
    }
    #endregion


    #region  HavePlayer
    public bool HavePlayer(string _playerID)
    {
        return playerdata.ContainsKey(_playerID);
    }
    public bool HavePlayer(string _playerID,out PlayerData2 _playerData)
    {
        if(HavePlayer(_playerID))
        {
            _playerData = GetPlayerData(_playerID);
            return true;
        }
        else
        {
            _playerData = null;
            return false;
        }
      
    }
    #endregion


    #region Get Player And Player Data
    public PlayerData2 GetPlayerData(string _playerID)
    {
        return playerdata[_playerID];
    }
    public String[] GetAllPlayerName()
    {
        string[] allName = new String[playerdata.Count];
        int num = 0;
        foreach (var t in playerdata)
        {
            allName[num] = t.Value.playerName;
            num++;
        }
        return allName;
    }

    public PlayerData2[] GetAllPlayer()
    {
        PlayerData2[] playerDatas = new PlayerData2[playerdata.Count];
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
