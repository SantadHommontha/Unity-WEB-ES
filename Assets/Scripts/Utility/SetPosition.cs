using UnityEngine;

public class SetPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 position;
    [SerializeField] private Transform positionPoint;
    [SerializeField] private bool useLocalPosition;

    public void MoveTargetToPosition()
    {
        if (position != null)
        {
            if (useLocalPosition)
            {
                target.localPosition = position;
            }
            else
            {
                target.position = position;
            }

        }
        else
        {
            if (useLocalPosition)
            {
                target.localPosition = positionPoint.localPosition;
            }
            else
            {
                target.position = positionPoint.position;
            }
        }
    }
}
