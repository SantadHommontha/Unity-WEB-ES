using TMPro;
using UnityEngine;

public class PlayerTextUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName_txt;
    [SerializeField] private StringValue stringValue;
    [SerializeField] private BoolValue finishConnect;
    void Awake()
    {
        if (playerName_txt == null) playerName_txt = GetComponent<TMP_Text>();
    }
    void OnEnable()
    {
        UpdateText("");
        stringValue.OnValueChange += UpdateText;
    }

    private void UpdateText(string _data)
    {
        string[] data = stringValue.Value.Split(",");
        playerName_txt.text = "";
        if (playerName_txt != null && finishConnect.Value)
        {
            playerName_txt.text = data[0];
            //    Debug.Log($"{gameObject.name} : {data[0]}");
        }

    }
}
