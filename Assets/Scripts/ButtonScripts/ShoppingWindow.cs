using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
public class ShoppingWindow : MonoBehaviour
{
    //Tablet Button and animation

    private GameObject inGame;
    public GameObject FPSController;
    bool isHidden;
    bool isOpen;
    
    void Start()
    {
        gameObject.SetActive(false);
        inGame = GameObject.Find("InGame");
        isHidden = true;
    }

    public void OnClick()
    {

        if (isHidden)
        {
            gameObject.SetActive(true);
            isHidden = false;
            inGame.SetActive(false);
            FPSController.GetComponent<FirstPersonController>().enabled = false;
        }

        else
        {
            StartCoroutine(CloseMenu());
        }
    }

    private IEnumerator CloseMenu()
    {
        yield return new WaitForSeconds(0.65f);
        isHidden = true;
        gameObject.SetActive(false);
        inGame.SetActive(true);
        FPSController.GetComponent<FirstPersonController>().enabled = true;
    }
}
