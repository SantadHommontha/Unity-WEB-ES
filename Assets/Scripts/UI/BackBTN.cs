using UnityEngine;

public class BackBTN : MonoBehaviour
{
    // this script at GameEnd Canvas


    [SerializeField] private GameEvent masterPanelCanvas;


   public void OnClick()
    {
        masterPanelCanvas.Raise(this, -999);
    }
}
