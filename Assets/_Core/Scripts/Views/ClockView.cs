using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ClockView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _clockText;
    [SerializeField]
    private TextMeshProUGUI _dateText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeSync();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator TimeUpdate()
    {
        while (true)
        {

            DateTime now = DateTime.Now;
            yield return new WaitForSecondsRealtime(
                1f - now.Millisecond / 1000f);
        }
    }

    private void TimeSync()
    {
        DateTime now = DateTime.Now;
        _dateText.text = now.ToString("dddd, dd MMMM");
        _clockText.text = now.ToString("HH:mm");
    }
}
