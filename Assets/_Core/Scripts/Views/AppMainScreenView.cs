using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppMainScreenView : PhoneScreenBase
{
    [SerializeField]
    private AppMainScreenData _appMainScreenData;
    [SerializeField]
    private Button[] _serviceMenus;
    [SerializeField]
    private Button[] _tabBarMenus;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Init()
    {
        for (int i = 0; i < _appMainScreenData.serviceMenus.Length; i++)
        {
            _serviceMenus[i].GetComponent<Image>().sprite = _appMainScreenData.serviceMenus[i].image;
            _serviceMenus[i].GetComponentInChildren<TextMeshProUGUI>().text = _appMainScreenData.serviceMenus[i].title;
        }
        for (int i = 0; i < _appMainScreenData.tabBarMenus.Length; i++)
        {
            _tabBarMenus[i].GetComponent<Image>().sprite = _appMainScreenData.tabBarMenus[i].image;
            _tabBarMenus[i].GetComponentInChildren<TextMeshProUGUI>().text = _appMainScreenData.tabBarMenus[i].title;
        }
    }

    public override void InitScreen(ScriptableObject scriptableObject)
    {
        if (scriptableObject is AppMainScreenData data)
        {
            _appMainScreenData = data;
        }
        Init();
    }

    public override void InitScreen()
    {
        Init();
    }

    public override GameObject GetGameObject()
    {
        return gameObject;
    }
}

[CreateAssetMenu(fileName = "AppMainScreenData", menuName = "ScriptableObjects/NewAppMainScreenData")]
public class AppMainScreenData : ScriptableObject
{
    public AppMainScreenMenuData[] serviceMenus;
    public AppMainScreenMenuData[] tabBarMenus;
}

[Serializable]
public struct AppMainScreenMenuData
{
    public String title;
    public Sprite image;
}
