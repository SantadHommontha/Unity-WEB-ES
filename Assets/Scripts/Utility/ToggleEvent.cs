using UnityEngine;

public class ToggleEvent : MonoBehaviour
{
    [SerializeField] private GameEvent event1;
    [SerializeField] private GameEvent event2;
    [SerializeField] private bool invert = false;

    private bool event_1 = false;
    private bool Event_1
    {
        get
        {
            return invert ? !event_1 : event_1;
        }
    }

    [SerializeField] private bool startEvent1;

    void Start()
    {
        if (startEvent1) RaiseEvent1();
        else RaiseEvent2();
    }

    public void Toggle()
    {
        if (event_1) RaiseEvent2();
        else RaiseEvent1();
    }

    public void RaiseEvent1()
    {
        event1.Raise(this, 0);
        event_1 = true;
    }

    public void RaiseEvent2()
    {
        event2.Raise(this, 0);
        event_1 = false;
    }
}
