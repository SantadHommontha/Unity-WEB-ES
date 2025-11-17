using UnityEngine;

public class CK : MonoBehaviour
{
    [SerializeField] PlaySpriteAnimation ta;
    [SerializeField] PlaySpriteAnimation ab;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        var mt = TeamManager.instance.MyTeamType;

        if (mt == ValueName.ADD_TEAM)
        {
            ta.Play(0.1f);
        }
        if (mt == ValueName.MINUS_TEAM)
        {
            ab.Play(0.1f);
        }
    }
}
