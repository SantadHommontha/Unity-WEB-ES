using UnityEngine;

public class Spectator : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private BoolValue spectator;
    [Header("Event")]
    [SerializeField] private GameEvent spectatorCanvasEvent;
    [SerializeField] private GameEvent masterCanvasEvent;
    public void SwitchOrHideSpectator()
    {
           if(spectator.Value)
        {
            spectatorCanvasEvent.Raise(this, -999);
        }
    }


    public void GameEnd()
    {
        if (spectator.Value)
        {
            masterCanvasEvent.Raise(this, -999);
        }
    }
}
