using UnityEngine;

public class SetSpectator : MonoBehaviour
{
    [SerializeField] private BoolValue setSpectator;

    public void OnValueChange(bool value)
    {
        setSpectator.Value = value;
    }
}
