using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private bool loop = false;
    [SerializeField][Range(0, 1)] float volume = 0.4f;
    [SerializeField] bool fadeOut = false;
    private Coroutine ct_WaitForSoundFinish;
    private Coroutine ct_fadeAudioFinish;
    private int currentAudioClipIndex = 0;
    private Action OnAudioFinished;
    private Action OnAudioFadeFinished;
    void Start()
    {
        OnAudioFinished = OnAudioFinish;
        OnAudioFadeFinished = OnAudioFadeFinish;
        currentAudioClipIndex = 0;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        Reset();
    }


    public void Reset()
    {
        currentAudioClipIndex = 0;
        Stop();
        audioSource.clip = null;
        if (ct_WaitForSoundFinish != null)
            StopCoroutine(ct_WaitForSoundFinish);
        ct_WaitForSoundFinish = null;
        if (ct_fadeAudioFinish != null)
            StopCoroutine(ct_fadeAudioFinish);
        ct_fadeAudioFinish = null;
        audioSource.volume = volume;
    }
    public void SetUpAudioSource()
    {
        audioSource.volume = volume;
        audioSource.clip = null;
        if (ct_WaitForSoundFinish != null)
            StopCoroutine(ct_WaitForSoundFinish);
        ct_WaitForSoundFinish = null;

        if (ct_fadeAudioFinish != null)
            StopCoroutine(ct_fadeAudioFinish);
        ct_fadeAudioFinish = null;
    }
    public void SetAudioClip(int _audioIndex)
    {
        if (_audioIndex >= audioClips.Length)
        {
            Debug.Log($"Not have audio clip in index {_audioIndex} at " + gameObject.name);
            return;
        }
        SetUpAudioSource();
        audioSource.clip = audioClips[_audioIndex];
        currentAudioClipIndex = _audioIndex;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        if (audioSource.clip == null)
        {
            Debug.Log("Audio clip is null at " + gameObject.name);
            return;
        }


        audioSource.Play();
        ct_WaitForSoundFinish = StartCoroutine(WaitForSoundFinish());
    }
    [ContextMenu("SetAndPlay")]
    public void SetAndPlay()
    {
        if (currentAudioClipIndex >= audioClips.Length)
            currentAudioClipIndex = 0;
        SetAudioClip(currentAudioClipIndex);
        Play();
    }
    [ContextMenu("Stop")]
    public void Stop()
    {
        if (fadeOut)
        {
            ct_fadeAudioFinish = StartCoroutine(FadeAudio());
        }
        else

        {
            audioSource.Stop();
            audioSource.clip = null;
            OnAudioFadeFinish();
        }


    }
    private void OnAudioFadeFinish()
    {
        if (ct_WaitForSoundFinish != null)
            StopCoroutine(ct_WaitForSoundFinish);
        ct_WaitForSoundFinish = null;

        audioSource.Stop();
        SetUpAudioSource();
    }
    private IEnumerator FadeAudio()
    {

        while (audioSource.volume > 0.1f)
        {
            audioSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        ct_fadeAudioFinish = null;
        OnAudioFadeFinished?.Invoke();
    }
    private void OnAudioFinish()
    {
        audioSource.clip = null;

        if (++currentAudioClipIndex < audioClips.Length)
        {
            SetAndPlay();
        }
        else
        {
            if (loop)
            {
                currentAudioClipIndex = 0;
                SetAndPlay();
            }
        }


    }
    private IEnumerator WaitForSoundFinish()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        OnAudioFinished?.Invoke();
        ct_WaitForSoundFinish = null;
    }


    public void SetLoop(bool _loop)
    {
        loop = _loop;
    }
    public void SetVolume(float _volume)
    {
        volume = Mathf.Clamp(_volume, 0, 1);
    }
}
