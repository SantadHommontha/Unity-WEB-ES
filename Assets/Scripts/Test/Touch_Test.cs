using UnityEngine;

public class Touch_Test : MonoBehaviour
{
    [SerializeField] private IntValue score;
    public void AddClick()
    {
        score.Value += 1;
    }
    public void MinusClick()
    {
        score.Value -= 1;
    }
}
