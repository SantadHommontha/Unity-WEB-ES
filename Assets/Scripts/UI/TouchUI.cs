using UnityEngine;
using UnityEngine.EventSystems;
public class TouchUI : MonoBehaviour, IPointerDownHandler
{
    [Header("Event")]
    [SerializeField] private GameEvent touchAreaCkicl;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("TouchUI");
        touchAreaCkicl.Raise(this,-999);
    }

}

