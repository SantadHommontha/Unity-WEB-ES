using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ResetBTN : MonoBehaviour
{
    [SerializeField] private Button startBtn;
    [SerializeField] private GameEvent reSetGameEvent;

    [SerializeField] private BoolValue finishConnect;
    [SerializeField] private BoolValue isMaster;
    void OnEnable()
    {
        if (finishConnect.Value)
            startBtn.gameObject.SetActive(isMaster.Value);
    }
    public void Click()
    {
        reSetGameEvent.Raise(this, -999);
    }
}
