using UnityEngine;

public class UpdateTimer_singlePLayer : MonoBehaviour
{
    [SerializeField] private FloatValue gameTimer;
    [SerializeField] private GameEvent gameEndSingle;

    public void SetTimer(Component sender, object _timer)
    {
        gameTimer.Value = (float)_timer;
    }


    public void CheckTimer(Component _sender,object _timer)
    {
        if ((float)_timer <= 0)
        {

            gameEndSingle.Raise(this, -999);

        }
    }
}
