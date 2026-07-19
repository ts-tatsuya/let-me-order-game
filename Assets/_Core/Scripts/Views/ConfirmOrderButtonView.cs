using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmOrderButtonView : MonoBehaviour
{
    [SerializeField]
    private Button _confirmOrderButton;
    [SerializeField]
    private TMP_InputField[] _inputFields;
    [SerializeField]
    private Sprite _interactableButton;
    [SerializeField]
    private Sprite _uninteractableButton;
    [SerializeField]
    private String _creditCardNumber;
    [SerializeField]
    private String _expiryDateNumber;
    [SerializeField]
    private String _ccvNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InputCheck());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator InputCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (_inputFields[0].text == _creditCardNumber
            && _inputFields[1].text == _expiryDateNumber
            && _inputFields[2].text == _ccvNumber)
            {
                _confirmOrderButton.interactable = true;
                _confirmOrderButton.image.sprite = _interactableButton;
                Color color;
                if (ColorUtility.TryParseHtmlString("#FFFFFF", out color))
                {
                    _confirmOrderButton.GetComponentInChildren<TextMeshProUGUI>().color = color;
                }
            }
            else
            {


                _confirmOrderButton.interactable = false;
                _confirmOrderButton.image.sprite = _uninteractableButton;
                Color color;
                if (ColorUtility.TryParseHtmlString("#696969", out color))
                {
                    _confirmOrderButton.GetComponentInChildren<TextMeshProUGUI>().color = color;
                }
            }
        }

    }
}
