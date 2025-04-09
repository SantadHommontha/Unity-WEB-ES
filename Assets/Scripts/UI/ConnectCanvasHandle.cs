using UnityEngine;

public class ConnectHandle : MonoBehaviour
{
    [SerializeField] private GameObject connect;
    [SerializeField] private GameObject tapToEnterGame;
    [SerializeField] private UIBlink uIBlink;


    public void OnConnected()
    {
        connect.SetActive(true);
        tapToEnterGame.SetActive(false);
    }

    public void OnFishiConnect()
    {
        connect.SetActive(false);
        tapToEnterGame.SetActive(true);
        uIBlink.StartFade();
    }
}
