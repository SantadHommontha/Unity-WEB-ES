using Photon.Pun;
using UnityEngine;

public class LeaveBTN : MonoBehaviour
{
    [SerializeField] private GameEvent leave;
    public void Click()
    {
        leave.Raise(this, PhotonNetwork.LocalPlayer.UserId);
    }
}
