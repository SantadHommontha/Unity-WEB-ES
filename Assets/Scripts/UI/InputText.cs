using TMPro;
using UnityEngine;

public class InputText : MonoBehaviour
{
    [SerializeField] private TMP_InputField tMP_InputField;

    public void ClearInput()
    {
        tMP_InputField.text = "";
    }
}
