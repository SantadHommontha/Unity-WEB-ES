using UnityEngine;

public class BoolValueHandle : ValueHandle
{
    public bool localValue = false;
    [SerializeField] private BoolValue boolValue;
   
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
