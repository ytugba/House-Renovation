using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class FlooringUI : MonoBehaviour, IPointerClickHandler
{//This class works with furnituremenupanel to see the page with slots and create object to the scene

    public Floorings floorings;

    private Image spriteImage;
    private GameObject inGame;
    private GameObject[] Rooms;
    private Material prefabFlooring;
    private GameObject flooringBox;
    private Transform flooringInBox;
    private GameObject selectedFlooring;
    private GameObject fpsController;
    private GameObject tablet;
    private DefaultNotificationTexts defaultNotificationTexts;
    private Transform currentRoom;

    private int currentPlayerBudget;
    public bool isJustCreated;

    private void Awake()
    {
        flooringBox = Resources.Load<GameObject>("Prefabs/Flooring Box");
        flooringInBox = flooringBox.transform.GetChild(0).transform.GetChild(0);
        selectedFlooring = GetComponent<GameObject>();
        defaultNotificationTexts = GameObject.Find("GameManager").GetComponent<DefaultNotificationTexts>();
        spriteImage = GetComponent<Image>();
        prefabFlooring = GetComponent<Material>();
        selectedFlooring = GetComponent<GameObject>();

        inGame = GameObject.Find("InGame");
        fpsController = GameObject.FindGameObjectWithTag("Player");
        tablet = GameObject.FindGameObjectWithTag("Tablet");
        UpdateFlooring(null);
    }
    private void Update()
    {
        Rooms = GameObject.FindGameObjectsWithTag("Room");
        currentRoom = GameObject.Find("House").transform;
        foreach (var room in Rooms)
        {
            if (room.GetComponent<RoomCollisionCheck>().inARoom)
            {
                currentRoom = room.transform;
            }
        }
    }

    public void UpdateFlooring(Floorings flooring)
    {
        this.floorings = flooring;

        if (this.floorings != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = Resources.Load<Sprite>("Sprites/Floorings/" + floorings.IconPath);
        }

        else
        {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentPlayerBudget = PlayerHoldings.currentPlayerBudget;
        if (currentPlayerBudget >= floorings.Price)
        {
            tablet.GetComponent<CanvasGroup>().alpha = 0;
            tablet.GetComponent<CanvasGroup>().blocksRaycasts = false;
            tablet.GetComponent<CanvasGroup>().interactable = false;

            fpsController.GetComponent<FirstPersonController>().enabled = false;
            inGame.SetActive(false);
            prefabFlooring = Resources.Load<Material>("Prefabs/Floorings/" + floorings.MaterialPath);
            flooringInBox.gameObject.GetComponent<Renderer>().material = prefabFlooring;
            selectedFlooring = (GameObject)Instantiate(flooringBox, currentRoom.Find("Building").Find("Furns"));
            selectedFlooring.name = selectedFlooring.name.Replace("(Clone)", "");

            fpsController.GetComponent<PlayerFurnitureManipulation>().manipulationFurn = selectedFlooring;
            fpsController.GetComponent<PlayerFurnitureManipulation>().flooringPrice = floorings.Price;
            isJustCreated = true;
            fpsController.GetComponent<PlayerFurnitureManipulation>().isJustCreated = isJustCreated;
        }
        else
        {
            defaultNotificationTexts.notificationTextTablet.gameObject.SetActive(true);
            defaultNotificationTexts.notificationTextTablet.text = string.Format(DefaultNotificationTexts.notEnoughBudgetText, (floorings.Price - currentPlayerBudget).ToString());
        }
    }
}
