using UnityEngine;

public class GameScore : TextUI
{
    [SerializeField] private IntValue intValue;

    void Start()
    {
        intValue.OnValueChange += UpdateText;
    }
    public void UpdateText(int _score)
    {
        text.text = _score.ToString();
    }
    public void UpdateText()
    {
        text.text = intValue.Value.ToString();
    }
}
