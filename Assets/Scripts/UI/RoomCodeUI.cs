using TMPro;
using UnityEngine;

public class RoomCodeUI : MonoBehaviour
{
    [Header("Referent")]
    [SerializeField] private TMP_InputField input;
    [Header("Value")]
    [SerializeField] private StringValue code;

    private void OnEnable()
    {
        code.OnValueChange += ShowText;
    }
    private void OnDisable()
    {
        code.OnValueChange -= ShowText;
    }
    public void CharacterUpdate(string _input)
    {
        if(_input.Length<= 4)
        {
            code.Value = _input.ToUpper();
        }
        else
        {
            code.Value = _input.Substring(0, 4).ToUpper();
        }
    }
    private void ShowText(string _text)
    {
        input.text = code.Value;
    }
    public void GenerateRandom(int _length)
    {
        code.Value = RandomString.GenerateRandomString(_length);
    }

}
