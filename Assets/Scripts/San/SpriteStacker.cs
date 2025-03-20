
using System.ComponentModel;
using UnityEngine;

public class SpriteStacker : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private SanAnimation[] sanAnimationsSet;
    private int nextScoreTarget;
    [SerializeField] private int nextScore = 10;
    [Header("Value")]
    [SerializeField] private Vector3Value lastSanTranform;
    [SerializeField] private IntValue gameScore;

    private GameObject lastSprite;
    void Start()
    {
        nextScoreTarget = 10;
        CreateNewSprite();
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
        return sanAnimationsSet[Random.Range(0, sanAnimationsSet.Length - 1)].gameObject;
    }

    [ContextMenu("Create New Sprite")]
    private void CreateNewSprite()
    {
        if (lastSprite == null)
        {
            CreateNewSprite(startPosition.position);
        }
        else
        {
            float height = lastSprite.GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 newPosition = lastSprite.transform.position + new Vector3(0, height, 0);

            CreateNewSprite(newPosition);
        }


    }
    private void CreateNewSprite(Vector3 _position)
    {

        lastSprite = Instantiate(RandomSprite(), _position, Quaternion.identity);
        lastSanTranform.Value = _position;
        if (lastSprite.TryGetComponent<SanAnimation>(out var component))
        {
            component.PlayAnimation();
        }

    }

     private void GameScoreUpdate(int _score)
    {
        if(_score >= nextScoreTarget)
        {
            nextScoreTarget += nextScore;
            CreateNewSprite();
        }
    }

}
