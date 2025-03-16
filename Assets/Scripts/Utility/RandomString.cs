using UnityEngine;
using System;
using System.Linq;
public static class RandomString 
{
  public  static string GenerateRandomString(int length)
    {
        return new string(Guid.NewGuid().ToString("N").ToUpper().Take(length).ToArray());

    }
}
