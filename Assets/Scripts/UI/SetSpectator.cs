using UnityEngine;
using UnityEngine.UI;

public class SetSpectator : MonoBehaviour
{
    [SerializeField] private BoolValue setSpectator;
    public void OnValueChange(bool value)
    {
        setSpectator.Value = value;
    }
}
