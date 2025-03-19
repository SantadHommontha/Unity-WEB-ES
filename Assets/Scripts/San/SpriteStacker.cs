
using UnityEngine;

public class SpriteStacker : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private SanAnimation[] sanAnimationsSet;
   


    private GameObject lastSprite;
    void Start()
    {
        CreateNewSprite();
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
        if (lastSprite.TryGetComponent<SanAnimation>(out var component))
        {
            component.PlayAnimation();
        }

    }



}
