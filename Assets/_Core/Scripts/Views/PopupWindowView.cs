using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindowView : MonoBehaviour
{
    [SerializeField]
    private Button _button;
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private Sprite _background;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button.onClick.AddListener(DestroyThis);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(Sprite background, String buttonText)
    {
        _background = background;
        _backgroundImage.sprite = _background;
        _button.GetComponent<TextMeshProUGUI>().text = buttonText;
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
