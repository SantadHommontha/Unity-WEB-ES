using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private IntValue gameScore;

    void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }
    void OnEnable()
    {
        gameScore.OnValueChange += GameScoreUpdate;
        GameScoreUpdate(gameScore.Value);
    }
    void OnDisable()
    {
        gameScore.OnValueChange -= GameScoreUpdate;
    }

    private void GameScoreUpdate(int _score)
    {
        if (slider == null) return;

        slider.value = _score * -1;

    }


    // Call With Event
    public void SetUp()
    {
        slider.value = 0;
    }
}
