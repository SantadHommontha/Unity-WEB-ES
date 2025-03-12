using UnityEngine;

[CreateAssetMenu(menuName = "Values/FloatValue")]
public class FloatValue : ScriptableValue<float>
{
    private void OnDisable()
    {
        Clear();
    }
    public void Clear()
    {
        value = 0.0f;
        OnValueChange = null;
    }
}
