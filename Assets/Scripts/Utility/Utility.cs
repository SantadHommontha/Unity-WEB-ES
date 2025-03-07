using System;
using JetBrains.Annotations;
using UnityEngine;

public enum ResponseState
{
    Fail,
    Complete
}

public struct ResponsetData
{
    public string responseState;
    public string responseMessage;
}

public struct ScoreSend
{
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
    public static string ResponseDataToJson(ResponseState _responseState, String _responseMessage)
    {
        return ResponseDataToJson(_responseState.ToString(), _responseMessage);
    }

    public static string ResponseDataToJson(string _responseState, String _responseMessage)
    {
        var responseData = new ResponsetData()
        {
            responseState = _responseState.ToString(),
            responseMessage = _responseMessage
        };

        var jsonData = JsonUtility.ToJson(responseData);

        return jsonData;
    }
}
