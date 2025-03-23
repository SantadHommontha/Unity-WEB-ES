using TMPro;
using UnityEngine;

#if UNITY_EDITOR
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
        UpdateText();
    }
    [ContextMenu("MINOR")]
    public void MINOR()
    {
        minor++;
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
#endif
