using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowRoomName : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private bool useUpdate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateText()
    {
        if (PhotonNetwork.InRoom)
            uiText.text = PhotonNetwork.CurrentRoom.Name;
    }

    // Update is called once per frame
    void Update()
    {
        if (useUpdate)
        {
            UpdateText();
        }
    }
}
