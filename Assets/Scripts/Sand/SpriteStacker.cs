
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
    [SerializeField] private Transform parentSpawn;
    [SerializeField] private List<SanAnimation> allSanAnimation = new List<SanAnimation>();
    [Header("Value")]
    [SerializeField] private Vector3Value lastSanTranform;
    [SerializeField] private IntValue gameScore;
    public float nnn = 1;
    private GameObject lastSprite;

    private int lastScore = 0;

    public int zCount = 1;

    [SerializeField] private Bucket bucket;
    [SerializeField] private WaterGun waterGun;


    void Start()
    {
        nextScoreTarget = 10;
        perviousScore = 0;
        lastScore = 0;
    }

    private void OnEnable()
    {
        gameScore.OnValueChange += GameScoreUpdate;
    }

    private void OnDisable()
    {
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
            Vector3 newPosition = allSanAnimation[allSanAnimation.Count - 1].transform.position + new Vector3(0, (height / 2f) - nnn, zCount);
            CreateNewSprite(newPosition);
        }
    }
    private void CreateNewSprite(bool _s)
    {
        if (allSanAnimation.Count == 0)
        {
            CreateNewSprite(startPosition.position);
        }
        else
        {
            float height = allSanAnimation[allSanAnimation.Count - 1].GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 newPosition = allSanAnimation[allSanAnimation.Count - 1].transform.position + new Vector3(0, (height / 2f) - nnn, zCount);
            CreateNewSprite(newPosition, _s);
        }
    }
    private void CreateNewSprite(Vector3 _position)
    {
        var t = Instantiate(RandomSprite(), _position, Quaternion.identity, parentSpawn);
        var sa = t.GetComponent<SanAnimation>();
        zCount++;
        // if (allSanAnimation.Count > 0)
        // {
        //     var lasp = allSanAnimation[allSanAnimation.Count - 1];
        //     //  if (lasp.sanHeadAnimation != null)

        //     //  lasp.sanHeadAnimation.GetSpriteRenderer.enabled = false;
        // }

        allSanAnimation.Add(sa);
        sa.PlayAnimationUP();
        // bucket.PlayAnimation();
        lastSanTranform.Value = _position;
    }
    private void CreateNewSprite(Vector3 _position, bool _s)
    {
        var t = Instantiate(RandomSprite(), _position, Quaternion.identity, parentSpawn);
        var sa = t.GetComponent<SanAnimation>();

        allSanAnimation.Add(sa);
        sa.SkipToLstSprite();
        // bucket.PlayAnimation();
        lastSanTranform.Value = _position;
    }
    [ContextMenu("Destroy Sprite")]
    private void DestroySprite()
    {
        if (allSanAnimation.Count <= 0) return;

        var ls = allSanAnimation[allSanAnimation.Count - 1];
        zCount--;
        lastSprite = ls.gameObject;
        ls.PlayAnimationDown(() => { Destroy(ls.gameObject); });
        lastSanTranform.Value = ls.transform.position;
        //   waterGun.PlayAnimation();
        allSanAnimation.Remove(ls);
    }


    private void GameScoreUpdate(int _score)
    {
        Debug.Log("score " + _score);
        if (_score > lastScore)
        {
            bucket.PlayAnimation();
        }
        else if (_score < lastScore)
        {
            waterGun.PlayAnimation();
        }
        if (_score >= nextScoreTarget)
        {
            //  Debug.Log("111");
            //  bucket.PlayAnimation();
            perviousScore = nextScoreTarget;
            nextScoreTarget += nextScore;
            CreateNewSprite();
        }
        else if (_score < perviousScore && parentSpawn.transform.childCount > 0)
        {
            //  Debug.Log("222");
            //     waterGun.PlayAnimation();
            nextScoreTarget = perviousScore;
            perviousScore -= nextScore;
            DestroySprite();
        }
        else if (_score <= 0)
        {
            //  Debug.Log("333");
            //    waterGun.PlayAnimation();
            nextScoreTarget = 10;
            perviousScore = 0;
        }
    }
    public void SetUp()

    {

        CreateNewSprite();
    }
    public void Reset()
    {
        nextScoreTarget = 10;
        perviousScore = 0;
        lastScore = 0;
        var num = parentSpawn.childCount;

        for (int i = 0; i < num; i++)
        {
            Destroy(parentSpawn.GetChild(i).gameObject);
        }
        allSanAnimation.Clear();

    }



    public void CheckScore()
    {
        // int sum = (gameScore.Value / nextScore) - allSanAnimation.Count;
        // if (sum > 0)
        // {
        //     CreateNewSprite(true);
        //     nextScoreTarget += gameScore.Value / nextScore;
        //     perviousScore = nextScoreTarget - nextScore;
        // }

    }


}
