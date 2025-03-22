using System.Collections;
using TMPro;
using UnityEngine;

public class ResponseText : MonoBehaviour
{
    [SerializeField] private float timeShow = 5f;
    [SerializeField] private StringValue response;
    [SerializeField] private TMP_Text text;


    private Coroutine coroutine;

    void OnEnable()
    {
        response.OnValueChange += ResponseUpdate;
    }
    void OnDisable()
    {
        response.OnValueChange -= ResponseUpdate;
    }
    void Awake()
    {
        if (text == null) text = GetComponent<TMP_Text>();

        if (text != null) text.text = "";
    }
    private void ResponseUpdate(string _response)
    {
        text.text = _response;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        yield return new WaitForSeconds(timeShow);
        text.text = "";
    }
}
