using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class SampleCount : MonoBehaviour
{
    private int _value;
    private Text _text;

    private void Start()
    {
        _value = 0;
        _text = GetComponent<Text>();
        StartCoroutine(UpdateCount());
    }

    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    private IEnumerator UpdateCount()
    {
        while (true)
        {
            _value++;
            _text.text = _value.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
