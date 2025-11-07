using TMPro;
using UnityEngine;

public class ShowIntValue : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private IntValue intValue;

    [SerializeField] private bool useAbsToShow;

    public void ChangeText(int _value)
    {
        int value = _value;
        if (useAbsToShow)
        {
            value = Mathf.Abs(value);
        }

        text.text = value.ToString();

    }

    public void ChangeText()
    {
        ChangeText(intValue.Value);
    }
}
