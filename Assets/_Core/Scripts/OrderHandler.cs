using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderHandler : MonoBehaviour
{
    public Button button;
    public Button burger;
    public Sprite interactableButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PickBurger()
    {
        button.interactable = true;
        button.image.sprite = interactableButton;
        Color color;
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out color))
        {
            button.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }
        burger.gameObject.SetActive(false);
    }
}
