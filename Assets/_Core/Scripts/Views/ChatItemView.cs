using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItemView : MonoBehaviour
{
    [SerializeField]
    private Image _potraitImage;
    [SerializeField]
    private TextMeshProUGUI _chatNameText;
    [SerializeField]
    private TextMeshProUGUI _chatDescText;

    public void Init(Sprite potrait, String chatName, String chatDesc)
    {
        _potraitImage.sprite = potrait;
        _chatNameText.text = chatName;
        _chatDescText.text = chatDesc;
    }
}
