using UnityEngine;

public class RunEventBTN : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    public void RunEvent()
    {
        gameEvent.Raise(this, -999);
    }
}
