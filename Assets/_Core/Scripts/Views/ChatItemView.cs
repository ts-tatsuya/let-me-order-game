using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatItemView : MonoBehaviour
{
    [SerializeField]
    private RawImage _potraitImage;
    [SerializeField]
    private TextMeshProUGUI _chatNameText;
    [SerializeField]
    private TextMeshProUGUI _chatDescText;

    public void Init(Texture2D potrait, String chatName, String chatDesc)
    {
        _potraitImage.texture = potrait;
        _chatNameText.text = chatName;
        _chatDescText.text = chatDesc;
    }
}
