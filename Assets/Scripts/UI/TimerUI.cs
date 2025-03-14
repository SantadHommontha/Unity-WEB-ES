using UnityEngine;

public class TimerUI : TextUI
{
    [SerializeField] private string startText;
    [SerializeField] private string endText;

    [SerializeField] private FloatValue timerValue;


    void OnEnable()
    {
        timerValue.OnValueChange += UpdateText;
    }

    void OnDisable()
    {
        timerValue.OnValueChange -= UpdateText;
    }

    private void UpdateText(float _time)
    {
        text.text = $"{startText}{timerValue.Value.ToString()}{endText}";
    }

}
