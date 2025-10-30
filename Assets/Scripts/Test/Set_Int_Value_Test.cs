using UnityEngine;

public class Set_Int_Value_Test : MonoBehaviour
{
    [SerializeField] private IntValue intValue;
    [SerializeField] private int newInt;

    [ContextMenu("Set Int")]
    public void SetInt()
    {
        intValue.Value = newInt;
    }

    public void ChangeInt()
    {
        intValue.Value += newInt;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            intValue.Value += newInt;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            intValue.Value -= newInt;
        }
    }
}
