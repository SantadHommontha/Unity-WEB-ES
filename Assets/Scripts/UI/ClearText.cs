using TMPro;
using UnityEngine;

public class ClearText : MonoBehaviour
{
    [SerializeField] private TMP_Text tmp_text;
    [SerializeField] private string text;
    public void Clear()
    {
        tmp_text.text = text;
    }
}
