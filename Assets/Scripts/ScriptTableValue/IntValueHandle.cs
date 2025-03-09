using UnityEngine;

public class IntValueHandle : MonoBehaviour
{
    [SerializeField][TextArea] private string description;
    [SerializeField] private IntValue  intValue;
    [SerializeField] private bool useLocalValue = false;
    [SerializeField] private int localValue = 0;
    public int Value => useLocalValue ? localValue: intValue.Value;
}
