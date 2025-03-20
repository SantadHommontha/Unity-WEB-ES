using UnityEngine;

public class Touch_Test : MonoBehaviour
{
    [SerializeField] private IntValue score;
    public void AddClick()
    {
        score.Value += 4;
    }
    public void MinusClick()
    {
        int scoreValue = score.Value;
        scoreValue -= 4;
        
        score.Value = Mathf.Clamp(scoreValue, 0, int.MaxValue);
    }
}
