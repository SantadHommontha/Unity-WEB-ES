using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ShowInLobbyAndInRoom : MonoBehaviour
{
    [SerializeField] private Image lobby;
    [SerializeField] private Image inroom;
    [SerializeField] private Image master;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InLobby)
            lobby.color = Color.green;
        else
            lobby.color = Color.red;

        if (PhotonNetwork.InRoom)
            inroom.color = Color.green;
        else
            inroom.color = Color.red;

            if (PhotonNetwork.IsMasterClient)
            master.color = Color.green;
        else
            master.color = Color.red;
    }
}
