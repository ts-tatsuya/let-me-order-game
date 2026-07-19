using System;
using UnityEngine;

public class ChatAppScreenView : MonoBehaviour
{
    [SerializeField]
    private ChatAppScreenData _chatAppScreenData;
    [SerializeField]
    private ChatItemView[] _chatItemViews;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        for (int i = 0; i < _chatAppScreenData.chatItems.Length; i++)
        {
            _chatItemViews[i].Init(_chatAppScreenData.chatItems[i].potrait, _chatAppScreenData.chatItems[i].chatName, _chatAppScreenData.chatItems[i].chatDesc);
        }
    }
}

[Serializable]
public class ChatAppScreenData : ScriptableObject
{
    public ChatItemData[] chatItems;
}

public struct ChatItemData
{
    public String chatName;
    public String chatDesc;
    public Sprite potrait;
}