using System.Collections;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField][TextArea] protected string description;
    [SerializeField] protected float time;
    protected float TIME
    {
        get
        {
            if (gameTime != null)
                return gameTime.Value;
            else
                return time;
        }
    }
    protected float timer;
    protected Coroutine coroutineTimeUpdate;
    [Header("Value")]
    [SerializeField] private FloatValue gameTime;
    [Header("Event")]
    [SerializeField] protected GameEvent timerUpdateEvent;




    public void StartTimer()
    {
        if (coroutineTimeUpdate != null) return;
        timer = TIME;
        coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(TIME));
    }
    public void StopTimer()
    {
        if (coroutineTimeUpdate != null)
            StopCoroutine(coroutineTimeUpdate);
        coroutineTimeUpdate = null;
    }


    protected virtual void StartTimeCount(bool _data)
    {
        if (_data)
        {
            if (coroutineTimeUpdate != null) return;
            timer = TIME;
            coroutineTimeUpdate = StartCoroutine(GameTimerUpdate(TIME));
        }
        else
        {
            if (coroutineTimeUpdate != null)
                StopCoroutine(coroutineTimeUpdate);
            coroutineTimeUpdate = null;
        }

    }
    protected virtual IEnumerator GameTimerUpdate(float _gameTime)
    {
        while ((timer > 0))
        {
            timerUpdateEvent.Raise(this, timer);
            yield return new WaitForSeconds(1);
            timerUpdateEvent.Raise(this, timer);

            timer--;
        }
        timerUpdateEvent.Raise(this, timer);
        coroutineTimeUpdate = null;
    }
}
