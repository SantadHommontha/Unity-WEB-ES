using System;
using UnityEngine;


public abstract class ScriptableValue<T> : ScriptableObject
{
    [SerializeField] private T value;
    public Action<T> OnValueChange;
    public T Value
    {
        get { return value; }
        set { 
            this.value = value; 
            OnValueChange(value);
        }
    }

    public void SetValue(T _value)
    {
        Value = _value;
    }

}
