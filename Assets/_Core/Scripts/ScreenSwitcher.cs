using UnityEngine;

public class ScreenSwitcher : MonoBehaviour
{
    [SerializeField]
    private ScriptableObject _scriptableObject;
    [SerializeField]
    private IPhoneScreen targetScreen;

    public void SwitchScreen()
    {
        if (_scriptableObject != null)
        {
            targetScreen.InitScreen(_scriptableObject);
        }
        else
        {
            targetScreen.InitScreen();
        }
        ScreenManager.currentScreen.GetGameObject().SetActive(false);
        ScreenManager.currentScreen = targetScreen;
        ScreenManager.currentScreen.GetGameObject().SetActive(true);
    }
}
