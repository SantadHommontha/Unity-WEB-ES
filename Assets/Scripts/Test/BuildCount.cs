using TMPro;
using UnityEngine;


public class BuildCount : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private int major;
    [SerializeField] private int minor;
    [SerializeField] private int patch;

    [ContextMenu("MAJOR")]
    public void MAJOR()
    {
        major++;
        minor = 0;
        patch =0;
        UpdateText();
    }
    [ContextMenu("MINOR")]
    public void MINOR()
    {
        minor++;
        patch =0;
        UpdateText();
    }
    [ContextMenu("PATCH")]
    public void PATCH()
    {
        patch++;
        UpdateText();
    }
    [ContextMenu("UpdateText")]
    public void UpdateText()
    {
        text.text = $"{major}.{minor}.{patch}";
    }
}

