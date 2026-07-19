using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PINController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _pinText;
    [SerializeField]
    private String _correctPin;
    [SerializeField]
    private UnityEvent _onCorrectPin = new UnityEvent();
    [SerializeField]
    private Button[] _numpads;
    private String _pin = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pinText.text = "";
        String numpadValue = "1234567890";
        for (int i = 0; i < _numpads.Length; i++)
        {
            Debug.Log(numpadValue.Substring(i, 1));
            String value = numpadValue.Substring(i, 1);
            _numpads[i].onClick.AddListener(() => TypePIN(value));

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TypePIN(String pin)
    {
        if (_pin.Length < 5)
        {
            _pin += pin;
        }
        else
        {
            _pin += pin;
            if (_pin == _correctPin)
            {
                _onCorrectPin.Invoke();
            }

            _pin = "";

        }
        Debug.Log(_pin);
        _pinText.text = _pin;
    }
}
