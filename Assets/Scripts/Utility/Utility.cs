using System;
using JetBrains.Annotations;
using UnityEngine;



public struct ScoreSend
{
    
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
