using UnityEngine;

public class ClickCount : MonoBehaviour
{
    //private int currentCount = 0;
    // private int allClick = 0;

    [SerializeField] private IntValue currentCount;
    [SerializeField] private IntValue allClick;
    [SerializeField] private BoolValue gamestart;
    public int CurrentClickCount => currentCount.Value;


    
    public void Click()
    {
        Debug.Log("ClickCount");
        if (!gamestart.Value) return;
        Debug.Log("Click");
        currentCount.Value++;
        allClick.Value++;
    }
    public void SetCurrentClick(int _click) => currentCount.Value = _click;
    public void Reset()
    {
        currentCount.Value = 0;
        allClick.Value = 0;
    }
}
