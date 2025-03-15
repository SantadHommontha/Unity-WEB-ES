using System.Collections;
using UnityEngine;

public class FetchTimer : GameTimer
{
    [Header("Value")]
    [SerializeField] private BoolValue gameStart;

    // public void StartFetchTime()
    // {
    //     if (coroutineTimeUpdate != null) return;
    //     coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(time));
    // }
    // public void StopFetchTime()
    // {
    //     if (coroutineTimeUpdate != null)
    //         StopCoroutine(coroutineTimeUpdate);
    //     coroutineTimeUpdate = null;
    // }

    protected override IEnumerator GameTimerUpdate(float _fetchTime)
    {
        while (gameStart.Value)
        {
            yield return new WaitForSeconds(_fetchTime);
            timerUpdateEvent.Raise(this, _fetchTime);
        }
        coroutineTimeUpdate = null;
    }
}
