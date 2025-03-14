using TMPro;
using UnityEngine;

public class PlayerTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName_txt;
    [SerializeField] private StringValue stringValue;

    void Awake()
    {
        if (playerName_txt == null) playerName_txt = GetComponent<TMP_Text>();
    }
    void OnEnable()
    {
        stringValue.OnValueChange += UpdateText;
    }
    void OnDisable()
    {
        stringValue.OnValueChange -= UpdateText;
    }
    private void UpdateText(string _data)
    {
        string[] data = _data.Split(",");

        if (playerName_txt != null)
        {
            playerName_txt.text = data[0];
        }

    }
}
