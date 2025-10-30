using TMPro;
using UnityEngine;

public class ShowIntValue : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private IntValue intValue;



    public void ChangeText(int _value)
    {
        text.text = _value.ToString();
    }

     public void ChangeText()
    {
        ChangeText(intValue.Value);
    }
}
