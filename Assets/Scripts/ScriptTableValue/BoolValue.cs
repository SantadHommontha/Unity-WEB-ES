using UnityEngine;


[CreateAssetMenu(menuName = "Values/BoolValue")]
public class BoolValue : ScriptableValue<bool>
{
  //  public bool defualtValue;
    private void OnDisable()
    {
        value = false;
        OnValueChange = null;
    }
}
