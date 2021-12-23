using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DefaultNotificationTexts : MonoBehaviour
{
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public TextMeshProUGUI notificationTextTablet;
    public GameObject placeObject;

    public static string notEnoughBudgetText = "You don't have enough budget. You need {0}."; // 0: Player Budget.
    public static string itemSoldInformation = "You sold {0} to {1}."; //0: Item name. 1: Item Sell Price.
    public static string paintingNotification = "You need to repair the wall first.";

    private void Awake()
    {
        notificationPanel = GameObject.Find("NotificationPanel");
        notificationTextTablet = GameObject.Find("NotificationTextTablet").GetComponent<TextMeshProUGUI>();
        notificationPanel.SetActive(false);
        notificationTextTablet.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(notificationPanel.activeSelf)
        {
            StartCoroutine(CloseNotification());
        }

        if (notificationTextTablet.gameObject.activeSelf)
        {
            StartCoroutine(CloseNotificationOfTablet());
        }
    }

    private IEnumerator CloseNotification()
    {
        yield return new WaitForSeconds(2f);
        notificationPanel.SetActive(false);
    }

    private IEnumerator CloseNotificationOfTablet()
    {
        yield return new WaitForSeconds(2f);
        notificationTextTablet.gameObject.SetActive(false);
    }
}
