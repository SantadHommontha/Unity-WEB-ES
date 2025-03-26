using UnityEngine;
using UnityEngine.UI;

public class BtnUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameEvent gameEvent;

    [SerializeField] private BoolValue finishConnect;
    [SerializeField] private BoolValue isMaster;


    void OnEnable()
    {
        if(button == null) button = GetComponent<Button>();
        finishConnect.OnValueChange += finishUpdate;
        finishUpdate(finishConnect.Value);
    }
    private void OnDisable()
    {
        finishConnect.OnValueChange -= finishUpdate;
    }
    private void finishUpdate(bool _conect)
    {
       // if (finishConnect.Value && button != null)
         //   button.gameObject.SetActive(isMaster.Value);
    }
    public void Click()
    {
        gameEvent.Raise(this, -999);
    }
}
