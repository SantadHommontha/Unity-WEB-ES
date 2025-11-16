using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    private static LoadScene _instance;
    public static LoadScene Instance=> _instance;

    void Awake()
    {
        _instance = this;
    }
    public void LoadSceneName(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
