using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerGeneric<T>: MonoBehaviour
{
    public GameEventGeneric gameEvent;
    public UnityEvent<T> response;

    private void OnEnable()
    {
        if (gameEvent != null)
            gameEvent.RegisterListener<T>(OnEventRaised);
    }

    private void OnDisable()
    {
        if (gameEvent != null)
            gameEvent.UnregisterListener<T>(OnEventRaised);
    }

    public void OnEventRaised(T value)
    {
        response.Invoke(value);
    }
}
