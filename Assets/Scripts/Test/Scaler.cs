using UnityEngine;

public class Scaler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float width = ScreenSize.GetScreenToWorldWidth;
        transform.localScale = Vector3.one * width;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
