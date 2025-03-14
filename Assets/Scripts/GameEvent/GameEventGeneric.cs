using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game Event (Generic)")]
public class GameEventGeneric : ScriptableObject
{
    private Dictionary<Type, Delegate> listeners = new Dictionary<Type, Delegate>();

    public void Raise<T>(T value)
    {
        Type type = typeof(T);
        if (listeners.ContainsKey(type))
        {
            (listeners[type] as Action<T>)?.Invoke(value);
        }
    }

    public void RegisterListener<T>(Action<T> listener)
    {
        Type type = typeof(T);
        if (listeners.ContainsKey(type))
        {
            listeners[type] = Delegate.Combine(listeners[type], listener);
        }
        else
        {
            listeners[type] = listener;
        }
    }

    public void UnregisterListener<T>(Action<T> listener)
    {
        Type type = typeof(T);
        if (listeners.ContainsKey(type))
        {
            listeners[type] = Delegate.Remove(listeners[type], listener);
            if (listeners[type] == null) listeners.Remove(type);
        }
    }
}
