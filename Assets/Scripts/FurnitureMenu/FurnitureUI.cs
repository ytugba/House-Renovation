using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class FurnitureUI : MonoBehaviour, IPointerClickHandler
{//This class works with furnituremenupanel to see the page with slots and create object to the scene

    private Image spriteImage;
    private GameObject inGame;
    private GameObject prefabFurniture;
    private GameObject selectedFurniture;
    private GameObject fpsController;
    private GameObject tablet;
    private GameObject[] Rooms;
    private Transform currentRoom;
    public Furnitures furniture;
    public DefaultNotificationTexts defaultNotificationTexts;
    private int currentPlayerBudget;
    public bool isJustCreated;

    void Awake()
    {
        spriteImage = GetComponent<Image>();
        prefabFurniture = GetComponent<GameObject>();
        selectedFurniture = GetComponent<GameObject>();
        inGame = GameObject.Find("InGame");
        fpsController = GameObject.FindGameObjectWithTag("Player");
        tablet = GameObject.FindGameObjectWithTag("Tablet");
        defaultNotificationTexts = GameObject.Find("GameManager").GetComponent<DefaultNotificationTexts>();
        UpdateFurniture(null);
    }

    private void Update()
    {
        Rooms = GameObject.FindGameObjectsWithTag("Room");
        currentRoom = GameObject.Find("House").transform;
        foreach (var room in Rooms)
        {
            if(room.GetComponent<RoomCollisionCheck>().inARoom)
            {
                currentRoom = room.transform;
            }
        }
    }

    //Can also be used for inventory system in future
    public void UpdateFurniture(Furnitures furniture)
    {
        this.furniture = furniture;

        if (this.furniture != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = Resources.Load<Sprite>("Sprites/Furnitures/" + furniture.IconPath);
            prefabFurniture = Resources.Load<GameObject>("Prefabs/Furnitures/" + furniture.PrefabPath);
        }

        else
        {
            spriteImage.color = Color.clear;
        }
    }

    //Click on slots
    public void OnPointerClick(PointerEventData eventData)
    {
        currentPlayerBudget = PlayerHoldings.currentPlayerBudget;

        if (currentPlayerBudget >= furniture.Price)
        {
            tablet.GetComponent<CanvasGroup>().alpha = 0;
            tablet.GetComponent<CanvasGroup>().blocksRaycasts = false;
            tablet.GetComponent<CanvasGroup>().interactable = false;

            fpsController.GetComponent<FirstPersonController>().enabled = false;
            inGame.SetActive(false);

            selectedFurniture = (GameObject)Instantiate(prefabFurniture);
            selectedFurniture.transform.SetParent(currentRoom.Find("Building").Find("Furns"));
            selectedFurniture.name = selectedFurniture.name.Replace("(Clone)", "");
            fpsController.GetComponent<PlayerFurnitureManipulation>().furniture = furniture;
            fpsController.GetComponent<PlayerFurnitureManipulation>().manipulationFurn = selectedFurniture;

            isJustCreated = true;
            fpsController.GetComponent<PlayerFurnitureManipulation>().isJustCreated = isJustCreated;
        }
        else
        {
            defaultNotificationTexts.notificationTextTablet.gameObject.SetActive(true);
            defaultNotificationTexts.notificationTextTablet.text = string.Format(DefaultNotificationTexts.notEnoughBudgetText, (furniture.Price - currentPlayerBudget).ToString());
        }
    }
}
