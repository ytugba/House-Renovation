using UnityEngine;

public class MenuButton : MonoBehaviour
{//Tablet Application Buttons
    public GameObject browser;

    private void Awake()
    {
        browser.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OnClick()
    {
        gameObject.SetActive(true);
        browser.SetActive(true);
    }
}
