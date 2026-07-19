using UnityEngine;

public class PhoneScreenBase : MonoBehaviour, IPhoneScreen
{
    public virtual GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }

    public virtual void InitScreen()
    {
        throw new System.NotImplementedException();
    }

    public virtual void InitScreen(ScriptableObject scriptableObject)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Init()
    {
        throw new System.NotImplementedException();
    }

}
