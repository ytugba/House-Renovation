using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityStandardAssets.Characters.FirstPerson;

public class PaintingUI : MonoBehaviour, IPointerClickHandler
{//This class works with furnituremenupanel to see the page with slots and create object to the scene

    public Paintings paintings;

    private Image spriteImage;
    private GameObject inGame;

    private Material prefabPainting;
    private Material plasterMaterial;
    private GameObject Bucket;
    private GameObject PlasterBucket;
    private GameObject paintInBucket;
    private GameObject selectedPainting;

    private GameObject fpsController;
    private GameObject tablet;
    private DefaultNotificationTexts defaultNotificationTexts;

    public GameObject[] Rooms { get; private set; }

    private Transform currentRoom;
    private int currentPlayerBudget;
    public bool isJustCreated;

    void Awake()
    {
        Bucket = Resources.Load<GameObject>("Prefabs/Bucket");
        PlasterBucket = Resources.Load<GameObject>("Prefabs/Plaster Bucket");
        paintInBucket = Bucket.transform.GetChild(0).gameObject;
        selectedPainting = GetComponent<GameObject>();
        defaultNotificationTexts = GameObject.Find("GameManager").GetComponent<DefaultNotificationTexts>();
        spriteImage = GetComponent<Image>();
        prefabPainting = GetComponent<Material>();
        selectedPainting = GetComponent<GameObject>();

        inGame = GameObject.Find("InGame");
        fpsController = GameObject.FindGameObjectWithTag("Player");
        tablet = GameObject.FindGameObjectWithTag("Tablet");
        UpdatePainting(null);
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

    //Can also be used for inventory system in future
    public void UpdatePainting(Paintings painting)
    {
        this.paintings = painting;

        if (this.paintings != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = Resources.Load<Sprite>("Sprites/Paintings/" + painting.IconPath);
        }

        else
        {
            spriteImage.color = Color.clear;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        currentPlayerBudget = PlayerHoldings.currentPlayerBudget;
        if (currentPlayerBudget >= paintings.Price)
        {
            tablet.GetComponent<CanvasGroup>().alpha = 0;
            tablet.GetComponent<CanvasGroup>().blocksRaycasts = false;
            tablet.GetComponent<CanvasGroup>().interactable = false;

            fpsController.GetComponent<FirstPersonController>().enabled = false;
            inGame.SetActive(false);

            if(paintings.MaterialPath != "Plaster Bucket")
            {
                prefabPainting = Resources.Load<Material>("Prefabs/Paintings/" + paintings.MaterialPath);
                paintInBucket.GetComponent<Renderer>().sharedMaterial = prefabPainting;
                selectedPainting = (GameObject)Instantiate(Bucket, currentRoom.Find("Building").Find("Furns"));
            }
            else
            {
                selectedPainting = (GameObject)Instantiate(PlasterBucket, currentRoom.Find("Building").Find("Furns"));
            }

            selectedPainting.name = selectedPainting.name.Replace("(Clone)", "");
            fpsController.GetComponent<PlayerFurnitureManipulation>().manipulationFurn = selectedPainting;
            fpsController.GetComponent<PlayerFurnitureManipulation>().paintingPrice = paintings.Price;
            isJustCreated = true;
            fpsController.GetComponent<PlayerFurnitureManipulation>().isJustCreated = isJustCreated;
        }
        else
        {
            defaultNotificationTexts.notificationTextTablet.gameObject.SetActive(true);
            defaultNotificationTexts.notificationTextTablet.text = string.Format(DefaultNotificationTexts.notEnoughBudgetText, (paintings.Price - currentPlayerBudget).ToString());
        }
    }
        
}
