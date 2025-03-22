
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpriteStacker : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private SanAnimation[] sanAnimationsSet;
    [SerializeField] private int nextScoreTarget;
    [SerializeField] private int perviousScore;
    [SerializeField] private int nextScore = 10;

    [SerializeField] private List<SanAnimation> allSanAnimation = new List<SanAnimation>();
    [Header("Value")]
    [SerializeField] private Vector3Value lastSanTranform;
    [SerializeField] private IntValue gameScore;
    [SerializeField] private BoolValue gameStart;

    private GameObject lastSprite;
    void Start()
    {
        nextScoreTarget = 10;
        perviousScore = 0;
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable");
        gameScore.OnValueChange += GameScoreUpdate;
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        gameScore.OnValueChange -= GameScoreUpdate;
    }

    private GameObject RandomSprite()
    {
        return sanAnimationsSet[Random.Range(0, sanAnimationsSet.Length)].gameObject;
    }

    [ContextMenu("Create New Sprite")]
    private void CreateNewSprite()
    {
        if (allSanAnimation.Count == 0)
        {
            CreateNewSprite(startPosition.position);
        }
        else
        {
            float height = allSanAnimation[allSanAnimation.Count - 1].GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 newPosition = allSanAnimation[allSanAnimation.Count - 1].transform.position + new Vector3(0, height, 0);

            CreateNewSprite(newPosition);
        }
    }
    private void CreateNewSprite(Vector3 _position)
    {
        var t = Instantiate(RandomSprite(), _position, Quaternion.identity);
        var sa = t.GetComponent<SanAnimation>();

        allSanAnimation.Add(sa);
        sa.PlayAnimationUP();

        lastSanTranform.Value = _position;
    }
    [ContextMenu("Destroy Sprite")]
    private void DestroySprite()
    {
        if (allSanAnimation.Count <= 0) return;

        var ls = allSanAnimation[allSanAnimation.Count - 1];

        lastSprite = ls.gameObject;
        //  ls.PlayAnimationDown(()=> { Destroy(ls.gameObject); });
        ls.PlayAnimationDown(() => { Destroy(ls.gameObject); });
        lastSanTranform.Value = ls.transform.position;
        allSanAnimation.Remove(ls);
    }


    private void GameScoreUpdate(int _score)
    {
        if (!gameStart.Value) return;
        if (_score >= nextScoreTarget)
        {

            perviousScore = nextScoreTarget - nextScore;
            nextScoreTarget += nextScore;
            CreateNewSprite();
        }
        else if (_score <= perviousScore && _score > 0)
        {

            nextScoreTarget = perviousScore;
            perviousScore -= nextScore;
            DestroySprite();
        }
        else if (_score <= 0)
        {
            Reset();
        }
    }
    // call with play Event
    public void SetUP()
    {
        CreateNewSprite();
    }
    // call with reset game Event
    public void Reset()
    {
        var count = allSanAnimation.Count;
        for (int i = 0; i < count; i++)
        {
            DestroySprite();
        }

        nextScoreTarget = 10;
        perviousScore = 0;
    }

}
