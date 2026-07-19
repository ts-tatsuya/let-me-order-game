using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static IPhoneScreen currentScreen;
    [SerializeField]
    private GameObject _startingScreen;
    [SerializeField]
    private IPhoneScreen[] _screens;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
