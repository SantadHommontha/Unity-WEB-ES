using UnityEngine;
using UnityEngine.UI;

public class TeamWin : TextUI
{
    [SerializeField] private StringValue stringValue;
    [SerializeField] private string start;
    [SerializeField] private string end;
    [SerializeField] private Color redColor;
    [SerializeField] private Color blueColor;
    [SerializeField] private Image image;

    void Start()
    {
        stringValue.OnValueChange += UpdateText;
    }
    public void UpdateText(string _text)
    {
        if (_text == ValueName.ADD_TEAM)
            image.color = redColor;
        else
            image.color = blueColor;
        text.text = $"{start}{_text}{end}";
    }
    void OnEnable()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        UpdateText(stringValue.Value);
    }
}
