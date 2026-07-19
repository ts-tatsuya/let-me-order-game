using UnityEngine.UI;
using UnityEngine;


public class LockScreenView : PhoneScreenBase
{
    [SerializeField]
    private LockScreenData _lockScreenData;
    [SerializeField]
    private Image _backgroundImage;
    [SerializeField]
    private ChatItemView[] _notifications;
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
        for (int i = 0; i < _lockScreenData.notifications.Length; i++)
        {
            _notifications[i].Init(
                _lockScreenData.notifications[i].potrait,
                _lockScreenData.notifications[i].chatName,
                _lockScreenData.notifications[i].chatDesc
            );
        }
        _backgroundImage.sprite = _lockScreenData.background;
    }

    public override void InitScreen(ScriptableObject scriptableObject)
    {
        if (scriptableObject is LockScreenData data)
        {
            _lockScreenData = data;
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

[CreateAssetMenu(fileName = "LockScreenData", menuName = "ScriptableObjects/NewLockScreenData")]
public class LockScreenData : ScriptableObject
{
    public Sprite background;
    public ChatItemData[] notifications;
}
