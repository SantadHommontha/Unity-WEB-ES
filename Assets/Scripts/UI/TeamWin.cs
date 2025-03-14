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
    void OnEnable()
    {
        UpdateText();
        Debug.Log("OnEnable");
    }
    public void UpdateText()
    {
        text.text = stringValue.Value;
    }
}
