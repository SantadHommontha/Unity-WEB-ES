using UnityEngine;


[CreateAssetMenu(menuName = "Values/BoolValue")]
public class BoolValue : ScriptableValue<bool>
{
  //  public bool defualtValue;
  private void OnDisable()
  {
    Clear();
  }
  public void Clear()
  {
    value = false;
    OnValueChange = null;
  }
}
