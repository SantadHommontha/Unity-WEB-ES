using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class CustomEvent : UnityEvent<Component, object> { }
public class GameEventListener : MonoBehaviour
{
    public CustomEvent response;
    public GameEvent gameEvent;

    public void OnEventRised(Component _sender,object _data)
    {
        response?.Invoke(_sender, _data);
    }

    private void OnEnable()
    {
        gameEvent.RegisterListener(this); 
    }
    

    private void OnDisable()
    {
        gameEvent?.UnregisterListener(this);
    }










}
