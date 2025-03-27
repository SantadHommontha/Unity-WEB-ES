using System.Collections;
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
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject blue;

    [SerializeField] private GameObject LeaveBtn;
    void Start()
    {
        //  stringValue.OnValueChange += UpdateText;
    }
    public void UpdateText(string _text)
    {
        if (_text == ValueName.ADD_TEAM)
        {
            red.SetActive(true);
            blue.SetActive(false);
            image.color = redColor;
        }
        else
        {
            blue.SetActive(true);
            red.SetActive(false);
            image.color = blueColor;
        }
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
    public void Startt()
    {
        StartCoroutine(TT());
    }
    private IEnumerator TT()
    {
        LeaveBtn.SetActive(false);
        yield return new WaitForSeconds(2);
        LeaveBtn.SetActive(true);
    }
}
