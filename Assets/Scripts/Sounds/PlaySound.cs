using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("Play")]
    public void Play()
    {
        audioSource.Play();
    }
}
