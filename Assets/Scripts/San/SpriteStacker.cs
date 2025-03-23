
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SpriteStacker : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private SanAnimation[] sanAnimationsSet;
  [SerializeField]  private int nextScoreTarget;
   [SerializeField] private int perviousScore;
    [SerializeField] private int nextScore = 10;
    [SerializeField] private Transform parentSpawn;
    [SerializeField] private List<SanAnimation> allSanAnimation = new List<SanAnimation>();
    [Header("Value")]
    [SerializeField] private Vector3Value lastSanTranform;
    [SerializeField] private IntValue gameScore;

    private GameObject lastSprite;
    void Start()
    {
        nextScoreTarget = 10;
        perviousScore = 0;
    }

    private void OnEnable()
    {
        gameScore.OnValueChange += GameScoreUpdate;
    }

    private void OnDisable()
    {
        gameScore.OnValueChange -= GameScoreUpdate;
    }

    private void Update()
    {
       /* if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            CreateNewSprite();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            DestroySprite();
        }*/
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
         //   Debug.Log("111");
            CreateNewSprite(startPosition.position);
        }
        else
        {
          //  Debug.Log("222");
            float height = allSanAnimation[allSanAnimation.Count-1].GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 newPosition = allSanAnimation[allSanAnimation.Count - 1].transform.position + new Vector3(0, height, 0);

            CreateNewSprite(newPosition);
        }
    }
    private void CreateNewSprite(Vector3 _position)
    {
        var t = Instantiate(RandomSprite(), _position, Quaternion.identity, parentSpawn);
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
        ls.PlayAnimationDown(() => { Destroy(ls.gameObject); });
        lastSanTranform.Value = ls.transform.position;
        allSanAnimation.Remove(ls);          
    }


     private void GameScoreUpdate(int _score)
    {
        


        if(_score >= nextScoreTarget)
        {   
            perviousScore = nextScoreTarget - nextScore;
            nextScoreTarget += nextScore;
            CreateNewSprite();
        }
       else if(_score <= perviousScore)
        {
            nextScoreTarget = perviousScore;
            perviousScore -= nextScore;
            DestroySprite();
        }
        else
        {
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

        var num = parentSpawn.childCount;


        for (int i =0; i < num; i++)
        {
            Destroy(parentSpawn.GetChild(i));    
        }

        allSanAnimation.Clear();

    }
}
