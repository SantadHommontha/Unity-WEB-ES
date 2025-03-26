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
[System.Serializable]
public class PlayerData
{
    public PhotonMessageInfo info;
    public string playerID;
    public string teamName;
    public string playerName;
    public int clickCount;
    public string code;
}

[System.Serializable]
public class Team
{
    // private TeamSetting teamSetting;


    // string is playerID
    private Dictionary<string, PlayerData> playerdata = new Dictionary<string, PlayerData>();
    public Action OnPlayerTeamChange;





    public void SetPlayerData(PlayerData[] _playerData)
    {
        Dictionary<string, PlayerData> playerdatas = new Dictionary<string, PlayerData>();
        foreach (var v in _playerData)
        {
            playerdatas.Add(v.playerID, v);
        }
        playerdata = playerdatas;
        OnPlayerTeamChange?.Invoke();
    }


    #region Team Count
    public int AddTeamCount()
    {
        int count = 0;
        foreach (var v in playerdata)
        {
            if (v.Value.teamName == ValueName.ADD_TEAM) count++;
        }
        return count;
    }
    public int MinusTeamCount()
    {
        int count = 0;
        foreach (var v in playerdata)
        {
            if (v.Value.teamName == ValueName.MINUS_TEAM) count++;
        }
        return count;
    }
    public void TeamCount(out int _addTeam, out int _minusTeam)
    {
        _addTeam = AddTeamCount();
        _minusTeam = MinusTeamCount();
    }
    #endregion



    #region ChangeData


    public void Change(ChangePlayerData _playerData)
    {
        if (HavePlayer(_playerData.playerTargetID, out var _player))
        {
            if (_playerData.playerName != null)
            {
                _player.playerName = _playerData.playerName;
            }
            if (_playerData.teamName != null)
            {
                _player.teamName = _playerData.teamName;
            }

        }
    }

    #endregion
    #region  Add Player And Remove Player
    public bool TryToAddPlayer(PlayerData _data)
    {

        if (!HavePlayer(_data.playerID))
        {
            AddPlayer(_data);
            return true;
        }
        else
        {
            return false;
        }
    }
    public void AddPlayer(PlayerData _data)
    {

        if (!playerdata.ContainsKey(_data.playerID))
        {

            playerdata.Add(_data.playerID, _data);
            OnPlayerTeamChange?.Invoke();
        }

    }

    public void RemovePlayer(string _playerID)
    {
        if (playerdata.ContainsKey(_playerID))
        {
            Debug.Log("Remove Player Complete");
            playerdata.Remove(_playerID);
            OnPlayerTeamChange?.Invoke();
        }
    }

    public void ClearAll()
    {
        playerdata.Clear();
        OnPlayerTeamChange?.Invoke();
    }
    #endregion


    #region  HavePlayer
    public bool HavePlayer(string _playerID)
    {
        return playerdata.ContainsKey(_playerID);
    }
    public bool HavePlayer(string _playerID, out PlayerData _playerData)
    {
        if (HavePlayer(_playerID))
        {
            _playerData = GetPlayerData(_playerID);
            return true;
        }
        else
        {
            _playerData = new PlayerData();
            return false;
        }

    }
    #endregion


    #region Get Player And Player Data
    public PlayerData GetPlayerData(string _playerID)
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

    public PlayerData[] GetAllPlayer()
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
    public string[] GetAllPlayerID()
    {
        string[] ids = new string[playerdata.Count];
        int num = 0;
        foreach (var t in playerdata)
        {
            ids[num] = t.Value.playerID;
            num++;
        }
        return ids;
    }
    #endregion
}
