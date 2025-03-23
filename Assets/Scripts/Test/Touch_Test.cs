using UnityEngine;
using UnityEngine.EventSystems;
public class Touch_Test : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int scorevalue = 5;
    [SerializeField] private IntValue score;
    [SerializeField] private bool red;

    public void AddClick()
    {
        Debug.Log("ADD");
        score.Value += scorevalue;
    }
    public void MinusClick()
    {
        Debug.Log("MINUS");
        score.Value -= scorevalue;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (red)
        {
            AddClick();
        }
        else
        {
            MinusClick();
        }
    }
}

