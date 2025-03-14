using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndGameText : MonoBehaviour
{
    [SerializeField] private TMP_Text teamWinName;
    [SerializeField] private TMP_Text score;
    public void ShowText()
    {
        Debug.Log((string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.END_GAME_SCORE]);
          if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(ValueName.END_GAME_SCORE))
        {
            
            var egs = (string)PhotonNetwork.CurrentRoom.CustomProperties[ValueName.END_GAME_SCORE];
            EndGameScore endGameScore = JsonUtility.FromJson<EndGameScore>(egs);
            Debug.Log(egs);
            teamWinName.text = endGameScore.teamWin;
            score.text = endGameScore.gameScore;


        }
    }
}
