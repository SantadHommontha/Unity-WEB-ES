using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private bool invert = false;

    private bool show = false;
    private bool Show
    {
        get
        {
            return invert ? !show : show;
        }
    }

    [SerializeField] private bool startShow;

    void Start()
    {
        if (startShow) ShowWindow();
        else HideWindow();
    }

    public void Toggle()
    {
        if (Show) HideWindow();
        else ShowWindow();
    }

    public void ShowWindow()
    {
        target.SetActive(true);
        show = true;
    }

    public void HideWindow()
    {
        target.SetActive(false);
        show = false;
    }
}
