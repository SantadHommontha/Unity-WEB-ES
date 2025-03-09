using UnityEngine;

[CreateAssetMenu(menuName = "Values/IntValue")]
public class IntValue : ScriptableValue<int>
{
 //  public int defualtValue;
 /*   private void OnEnable()
    {
        value = defualtValue;
    }*/
    private void OnDisable()
    {
        value = 0;
        OnValueChange = null;
    }
}
