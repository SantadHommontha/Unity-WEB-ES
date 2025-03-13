using System;
using UnityEngine;

/*public enum ResponseState
{
    Fail,
    Complete
}*/
public struct PlayerDataForUI
{
    public int index;
    public string playerName;
    public string playerID;
  
}



public struct ResponsetData
{
    public string responseState;
    public string responseMessage;
}

public struct ScoreSend
{
    public string playerNameSend;
    public string scoreType;
    public int score;
}

public struct SendResponseJoinData
{
    public string responseState;
    public string responseMessage;
    public string myTeamType;
}


public struct GameUpdate
{
    public int currentScore;
    public bool gameStart;
}


public struct PlayerTeamList
{
    public string[] playerName;
    public string[] playerID;
}

public static class Utility
{
   
    public static string ResponseDataToJson(string _responseState, string _responseMessage)
    {
        var responseData = new ResponsetData()
        {
            responseState = _responseState,
            responseMessage = _responseMessage
        };

        var jsonData = JsonUtility.ToJson(responseData);

        return jsonData;
    }
    public static void ResponseDataFromJson(string _jsonData,out string _responseState,out string _responseMessage)
    {
        var data = JsonUtility.FromJson<ResponsetData>(_jsonData);
        _responseState = data.responseState;
        _responseMessage = data.responseMessage;
    }
}
