using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Values/StringValue")]
public class StringValue : ScriptableValue<String>
{
    private void OnDisable()
    {
        Clear();
    }
    public void Clear()
    {
        value = null;
        OnValueChange = null;
    }
}
