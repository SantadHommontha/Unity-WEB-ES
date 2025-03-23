using TMPro;
using UnityEngine;

public class SpectatorBTN : MonoBehaviour
{
    

   [SerializeField] private GameEvent spectatorEvent;
    [SerializeField] private BoolValue spectator;

    public void OnClick()
    {
        spectatorEvent.Raise(this, -999);
        spectator.Value = true;
    }

   

}
