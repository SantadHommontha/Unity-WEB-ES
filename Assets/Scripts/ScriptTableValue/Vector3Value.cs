using UnityEngine;


[CreateAssetMenu(menuName = "Values/Vector3Value")]
public class Vector3Value : ScriptableValue<Vector3>
{
    private void OnDisable()
    {
        Clear();
    }

    public void Clear()
    {
        value = new Vector3();
        OnValueChange = null;
    }
}
