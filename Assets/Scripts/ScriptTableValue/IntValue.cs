using UnityEngine;

[CreateAssetMenu(menuName = "Values/IntValue")]
public class IntValue : ScriptableValue<int>
{
    private void OnDisable()
    {
        Clear();
    }

    public void Clear()
    {
        value = 0;
        OnValueChange = null;
    }
}
