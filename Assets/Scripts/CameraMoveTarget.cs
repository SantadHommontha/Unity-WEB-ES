using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMoveTarget : MonoBehaviour
{
    [SerializeField] private float minYPosition;
    [SerializeField] private float speed = 5f;
    [Header("Value")]
    [SerializeField] private Vector3Value lastSanValue;


    private Vector3 targetPosition;


    private void OnEnable()
    {
        lastSanValue.OnValueChange += OnSanTranformChange;
    }
    private void OnDisable()
    {
        lastSanValue.OnValueChange -= OnSanTranformChange;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }


    private void OnSanTranformChange(Vector3 _newTranform)
    {
        var newPosition = new Vector3(0f, Mathf.Clamp(_newTranform.y,minYPosition,float.MaxValue),-10f);

        targetPosition = newPosition;
    }
}
