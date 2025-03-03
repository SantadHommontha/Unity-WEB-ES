using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private bool show = false;

    [SerializeField] private bool startShow;

    void Start()
    {
        if (startShow) Show();
        else Hide();
    }

    public void Toggle()
    {
        if (show) Hide();
        else Show();
    }

    public void Show()
    {
        target.SetActive(true);
        show = true;
    }

    public void Hide()
    {
        target.SetActive(false);
        show = false;
    }
}
