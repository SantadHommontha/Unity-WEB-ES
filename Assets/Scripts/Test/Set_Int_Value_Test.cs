using UnityEngine;

public class Set_Int_Value_Test : MonoBehaviour
{
    [SerializeField] private IntValue intValue;
    [SerializeField] private int newInt;

    [ContextMenu("Set Int")]
    private void SetInt()
    {
        intValue.Value = newInt; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            intValue.Value += 10;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            intValue.Value -= 10;
        }
    }
}
