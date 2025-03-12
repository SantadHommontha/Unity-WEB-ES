using System;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    enum ValueType
    {
        String,
        Int,
        Float,
        Bool
    }
    [SerializeField] private TMP_Text text;

    [Header("Value")]
    [SerializeField] private StringValue stringValue;
    [SerializeField] private IntValue intValue;
    [SerializeField] private FloatValue floatValue;
    [SerializeField] private BoolValue boolValue;
    [Header("Value Type")]
    [SerializeField] private ValueType valueType = ValueType.Int;
    [SerializeField] private string startText;
    [SerializeField] private string endText;

    void OnEnable()
    {
        switch (valueType)
        {
            case ValueType.String:
                stringValue.OnValueChange += UpdateText;
                break;
            case ValueType.Int:
                intValue.OnValueChange += IntText;
                break;
            case ValueType.Float:
                floatValue.OnValueChange += FloatText;
                break;
            case ValueType.Bool:
                boolValue.OnValueChange += BoolText;
                break;
        }
    }

    void OnDestroy()
    {
          switch (valueType)
        {
            case ValueType.String:
                stringValue.OnValueChange -= UpdateText;
                break;
            case ValueType.Int:
                intValue.OnValueChange -= IntText;
                break;
            case ValueType.Float:
                floatValue.OnValueChange -= FloatText;
                break;
            case ValueType.Bool:
                boolValue.OnValueChange -= BoolText;
                break;
        }
    }



    private void IntText(int _data)
    {
        UpdateText(_data.ToString());
    }
    private void FloatText(float _data)
    {
        UpdateText(_data.ToString());
    }
    private void BoolText(bool _data)
    {
        UpdateText(_data.ToString());
    }


    private void UpdateText(string _data)
    {
        text.text = $"{startText}{_data}{endText}";
    }
}
