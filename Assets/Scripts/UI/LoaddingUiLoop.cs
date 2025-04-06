using UnityEngine;

public class LoaddingUiLoop : MonoBehaviour
{

    [SerializeField] private RectTransform uiElement;
    [SerializeField] private float moveDistance = 20f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool canPlay = false;
    private Vector2 startPos;

    void Start()
    {
        startPos = uiElement.anchoredPosition;
    }

    void Update()
    {
        if (!canPlay) return;
        float offset = Mathf.Sin(Time.time * speed) * moveDistance;
        uiElement.anchoredPosition = startPos + new Vector2(0, offset);
    }

    public void Stop()
    {
        canPlay = false;
    }
    public void Play()
    {
        canPlay = true;
    }
}
