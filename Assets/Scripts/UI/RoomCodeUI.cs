using TMPro;
using UnityEngine;

public class RoomCodeUI : MonoBehaviour
{



    [Header("Referent")]
    [SerializeField] private TMP_InputField input;


    public void GenerateRandom(int _length)
    {
        input.text = RandomString.GenerateRandomString(_length);
    }

    public void CharacterUpdate(string _input)
    {
        if (_input.Length <= 4)
        {
            input.text = _input.ToUpper();
        }
        else
        {
            input.text = _input.Substring(0, 4).ToUpper();
        }
    }






}
