using UnityEngine;


public interface IPhoneScreen
{
    public void InitScreen();
    public void InitScreen(ScriptableObject scriptableObject);

    public GameObject GetGameObject();
}
