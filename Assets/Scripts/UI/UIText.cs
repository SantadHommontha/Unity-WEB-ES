using System;
using System.Collections;
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
    // [SerializeField] private ValueType valueType = ValueType.Int;
    [SerializeField] private string startText;
    [SerializeField] private string endText;

    [SerializeField] private bool autoHide = false;
    [SerializeField] private float autoHideTime = 3f;
    private Coroutine ct_autoHideText;
    void Start()
    {
        if (text == null) text = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        if (stringValue != null) stringValue.OnValueChange += UpdateText;
        else if (intValue != null) intValue.OnValueChange += IntText;
        else if (floatValue != null) floatValue.OnValueChange += FloatText;
        else if (boolValue != null) boolValue.OnValueChange += BoolText;


        // switch (valueType)
        // {
        //     case ValueType.String:
        //         stringValue.OnValueChange += UpdateText;
        //         break;
        //     case ValueType.Int:
        //         intValue.OnValueChange += IntText;
        //         break;
        //     case ValueType.Float:
        //         floatValue.OnValueChange += FloatText;
        //         break;
        //     case ValueType.Bool:
        //         boolValue.OnValueChange += BoolText;
        //         break;
        // }
    }

    void OnDestroy()
    {
        if (stringValue != null) stringValue.OnValueChange -= UpdateText;
        else if (intValue != null) intValue.OnValueChange -= IntText;
        else if (floatValue != null) floatValue.OnValueChange -= FloatText;
        else if (boolValue != null) boolValue.OnValueChange -= BoolText;
        // switch (valueType)
        // {
        //     case ValueType.String:
        //         stringValue.OnValueChange -= UpdateText;
        //         break;
        //     case ValueType.Int:
        //         intValue.OnValueChange -= IntText;
        //         break;
        //     case ValueType.Float:
        //         floatValue.OnValueChange -= FloatText;
        //         break;
        //     case ValueType.Bool:
        //         boolValue.OnValueChange -= BoolText;
        //         break;
        // }
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
        if (text == null) return;
        text.text = $"{startText}{_data}{endText}";
        if (autoHide)
        {
            if (ct_autoHideText != null)
                StopCoroutine(ct_autoHideText);
            ct_autoHideText = StartCoroutine(AutoHideText(autoHideTime));


         
        }
    }

    private IEnumerator AutoHideText(float _time)
    {
        yield return new WaitForSeconds(_time);
        text.text = "";
    }
}
