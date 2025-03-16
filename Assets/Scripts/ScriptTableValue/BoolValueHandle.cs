using UnityEngine;

public class BoolValueHandle : ValueHandle
{
    [SerializeField] private BoolValue boolValue;
   
    [SerializeField] private bool localValue = false;
    public bool Value
    {
        get
        {
            return  useLocalValue ? localValue : boolValue.Value;
        }
        set
        {
            if (useLocalValue)
                localValue = value;
            else
                boolValue.Value = value;
        }
    }
}
