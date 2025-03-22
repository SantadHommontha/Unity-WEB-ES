using System;
using UnityEngine;

[System.Serializable]
public abstract class ScriptableValue<T> : ScriptableObject
{
    [SerializeField][TextArea] private string description;

    [SerializeField] protected T value;
    public Action<T> OnValueChange;
    public T Value
    {
        get { return value; }
        set
        {
            this.value = value;
            OnValueChange?.Invoke(value);
        }
    }

    public void SetValue(T _value)
    {
        Value = _value;
    }
    public void ClearAction()
    {
        OnValueChange = null;
    }

}
