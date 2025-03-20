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
}
