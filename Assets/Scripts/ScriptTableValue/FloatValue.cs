using UnityEngine;

[CreateAssetMenu(menuName = "Values/FloatValue")]
public class FloatValue : ScriptableValue<float>
{
  //  public float defualtValue;
   /* private void OnEnable()
    {
        value = defualtValue;
    }*/
    private void OnDisable()
    {
        value = 0.0f;
        OnValueChange = null;
    }
}
