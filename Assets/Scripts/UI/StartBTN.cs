using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class StartBTN : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameEvent startEvent;

    [SerializeField] private BoolValue gamestart;
    [SerializeField] private BoolValue finishConnect;
    [SerializeField] private BoolValue isMaster;
    void OnEnable()
    {
        if (finishConnect.Value)
            startBtn.gameObject.SetActive(isMaster.Value);
    }
    public void Click()
    {
        gamestart.Value = true;
        startEvent.Raise(this, -999);
    }

}
