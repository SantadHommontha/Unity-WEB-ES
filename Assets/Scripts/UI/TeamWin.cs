using UnityEngine;
using UnityEngine.UI;

public class TeamWin : TextUI
{
    [SerializeField] private StringValue stringValue;

    void Start()
    {
        stringValue.OnValueChange += UpdateText;
    }
    public void UpdateText(string _text)
    {
        text.text = _text;
    }
    public void UpdateText()
    {
        text.text = stringValue.Value;
    }
}
