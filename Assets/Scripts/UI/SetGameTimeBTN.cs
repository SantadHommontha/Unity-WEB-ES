using UnityEngine;
using TMPro;
public class SetGameTimeBTN : MonoBehaviour
{

    [Header("Referent")]
    [SerializeField] private TMP_InputField input;

    [SerializeField] private FloatValue gameTime;
    public void GenerateRandom(int _length)
    {
        input.text = RandomString.GenerateRandomString(_length);
    }
    void Start()
    {
        gameTime.Value = 30f;
    }
    public void CharacterUpdate(string _input)
    {
        if (float.TryParse(_input, out var result))
        {
            gameTime.Value = result;
        }
    }
}
