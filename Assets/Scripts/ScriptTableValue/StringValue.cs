using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Values/StringValue")]
public class StringValue : ScriptableValue<String>
{

  //  public string defualtValue;
  /*  private void OnEnable()
    {
        value = defualtValue;
    }*/
    private void OnDisable()
    {
        value = null;
        OnValueChange = null;
    }
}
