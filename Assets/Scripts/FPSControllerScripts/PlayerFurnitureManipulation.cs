using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerFurnitureManipulation : MonoBehaviour
{
    #region variables

    private FurnitureUI furnitureUI;
    private FurnitureDatabase furnitureDatabase;
    private GameObject tablet;
    private GameObject inGame;

    public GameObject placeObject;
    public Furnitures furniture;
    [Header("These lists are only for tracking.")]
    public List<Furnitures> furnituresCreated;
    public List<GameObject> furnitureObjectsCreated;
    public DefaultNotificationTexts defaultNotificationTexts;
    private TextMeshProUGUI sellNotification;
    [Tooltip("Sell Percent")]
    public int percent;
    #region MovementAreaButtons
    [Header("Place and Sell button on moving furnitures")]
    public Button PlaceButton;
    public Button SellButton;

    private GameObject LeftRotateButton;
    private GameObject RightRotateButton;
    private GameObject LeftRotateFineTuneButton;
    private GameObject RightRotatFineTuneButton;

    private GameObject MoveLeftFineTuneButton;
    private GameObject MoveRightFineTuneButton;
    private GameObject MoveForwardFineTuneButton;
    private GameObject MoveBackwardFineTuneButton;
    #endregion

    #region ButtonCheckVariables
    private bool isPlaceClicked;
    private bool isCancelClicked;
    private bool isSellClicked;

    private bool isLeftRotateClicked;
    private bool isRightRotateClicked;

    private bool isMoveForwardClicked;
    private bool isMoveBackwardClicked;
    private bool isMoveLeftClicked;
    private bool isMoveRightClicked;

    private bool isFineTuned;
    private bool isMoveFineTune;

    public bool isJustCreated = false;
    #endregion

    public GameObject manipulationFurn;             //Furniture that manipulating
    private Collider manipulationFurnCollider;

    public Vector3 objectPosition;
    public Quaternion objectRotation;

    private Vector3 groundPosition;
    

    private int currentPlayerBudget;
    private int PlayerFurnitureManipulationLayer = 8;
    private int moveUnit = 5;
    private int minAngle = 5;
    private int maxAngle = 45;
    private float rotateAngle;

    public bool newManipulationFurn;
    [HideInInspector]
    public int paintingPrice;
    [HideInInspector]
    public int flooringPrice;
    #endregion

    void Awake()
    {
        furnitureDatabase = GameObject.Find("FurnitureDatabase").GetComponent<FurnitureDatabase>();
        tablet = GameObject.FindGameObjectWithTag("Tablet");
        inGame = GameObject.Find("InGame");
        groundPosition = GameObject.FindGameObjectWithTag("Floor").transform.position;

        #region ButtonDefinitions
        PlaceButton = GameObject.Find("Place").GetComponent<Button>();

        LeftRotateButton = GameObject.Find("LeftRotate").GetComponent<GameObject>();
        RightRotateButton = GameObject.Find("RightRotate").GetComponent<GameObject>();
        LeftRotateFineTuneButton = GameObject.Find("LeftRotateFineTune").GetComponent<GameObject>();
        RightRotatFineTuneButton = GameObject.Find("RightRotateFineTune").GetComponent<GameObject>();

        MoveForwardFineTuneButton = GameObject.Find("MoveForwardFineTune").GetComponent<GameObject>();
        MoveBackwardFineTuneButton = GameObject.Find("MoveBackwardFineTune").GetComponent<GameObject>();
        MoveLeftFineTuneButton = GameObject.Find("MoveLeftFineTune").GetComponent<GameObject>();
        MoveRightFineTuneButton = GameObject.Find("MoveRightFineTune").GetComponent<GameObject>();
        #endregion
        sellNotification = defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationText;
        currentPlayerBudget = PlayerHoldings.currentPlayerBudget;
        SellButton.interactable = false;

        placeObject.SetActive(false);
    }

    private void Start()
    {
        furnituresCreated = new List<Furnitures>();
        furnitureObjectsCreated = new List<GameObject>();
        foreach (GameObject furnitureExisted in GameObject.FindGameObjectsWithTag("Furniture"))
        {
            if(furnitureExisted != null)
            {
                foreach (Furnitures furniture in furnitureDatabase.furnitures)
                {
                    if (furnitureExisted.name == furniture.PrefabPath)
                    {
                        furnituresCreated.Add(furniture);
                        furnitureObjectsCreated.Add(furnitureExisted);
                    }
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (manipulationFurn != null)
        {
            manipulationFurnCollider = manipulationFurn.GetComponent<Collider>();
            manipulationFurnCollider.isTrigger = true;
            inGame.SetActive(false);
            placeObject.SetActive(true);

            if (!isJustCreated)
            {
                SellButton.interactable = true;
            }
            else
            {
                SellButton.interactable = false;
            }

            ManipulateObject();

            if (!newManipulationFurn)
            {
                #region ButtonClickCallFunctions
                if (isPlaceClicked)
                {
                    PlaceObject();
                    isJustCreated = false;
                }
                
                if(isCancelClicked)
                {
                    CancelObject();
                }

                if (isSellClicked)
                {
                    SellObject();
                }

                if (isFineTuned)
                {
                    rotateAngle = minAngle;
                }

                else
                {
                    rotateAngle = maxAngle;
                }

                if (isLeftRotateClicked)
                {
                    gameObject.GetComponent<PlayerFurnitureManipulation>().Rotate(rotateAngle);
                }
                
                if (isRightRotateClicked)
                {
                    gameObject.GetComponent<PlayerFurnitureManipulation>().Rotate(-rotateAngle);
                }
                #endregion
            }

            #region ButtonBoolCheck
            isPlaceClicked = false;
            isCancelClicked = false;
            isSellClicked = false;
            isLeftRotateClicked = false;
            isRightRotateClicked = false;
            isFineTuned = false;
            isMoveFineTune = false;
            isMoveForwardClicked = false;
            isMoveBackwardClicked = false;
            isMoveLeftClicked = false;
            isMoveRightClicked = false;
            #endregion
        }

        if (Input.GetMouseButtonUp(0) && manipulationFurn != null && newManipulationFurn)
        {
            newManipulationFurn = false;
        }
    }
    
    //Assigned to the related buttons
    #region ButtonClicks 
    public void PlaceOnClick()
    {
        isPlaceClicked = true;
    }

    public void CancelOnClick()
    {
        isCancelClicked = true;
    }

    public void SellOnClick()
    {
        isSellClicked = true;
    }
    
    public void LeftRotateOnClick()
    {
        isLeftRotateClicked = true;
    }
    
    public void RightRotateOnClick()
    {
        isRightRotateClicked = true;
    }

    public void LeftRotateFineTuneOnClick()
    {
        isFineTuned = true;
        isLeftRotateClicked = true;
    }

    public void RightRotateFineTuneOnClick()
    {
        isFineTuned = true;
        isRightRotateClicked = true;
    }

    public void MoveForwardFineTuneOnClick()
    {
        isMoveFineTune = true;
        isMoveForwardClicked = true;
        
    }
    public void MoveBackwardFineTuneOnClick()
    {
        isMoveFineTune = true;
        isMoveBackwardClicked = true;
    }
    public void MoveLeftFineTuneOnClick()
    {
        isMoveFineTune = true;
        isMoveLeftClicked = true;
    }
    public void MoveRightFineTuneOnClick()
    {
        isMoveFineTune = true;
        isMoveRightClicked = true;
    }
    #endregion

    ///<summary> Place manipulationFurn Object
    ///</summary>
    private void PlaceObject()
    {
        if (tablet.activeSelf == true)
        {
            tablet.GetComponent<CanvasGroup>().alpha = 1;
            tablet.GetComponent<CanvasGroup>().interactable = true;
            tablet.GetComponent<CanvasGroup>().blocksRaycasts = true;
            gameObject.GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<FirstPersonController>().enabled = true;
            inGame.SetActive(true);
        }

        if(isJustCreated)
        {
            if(SceneManager.GetActiveScene().name == "UserHouse")
            {
                if (manipulationFurn.name == "Bucket")
                {
                    currentPlayerBudget -= paintingPrice;
                }
                else if (manipulationFurn.tag == "Furniture")
                {
                    currentPlayerBudget -= furniture.Price;
                }
                else if (manipulationFurn.tag == "FlooringBox")
                {
                    currentPlayerBudget -= flooringPrice;
                }

                else if (manipulationFurn.tag == "PlasterBucket")
                {
                    currentPlayerBudget -= paintingPrice;
                }
                else
                {
                    currentPlayerBudget -= 0;
                }
            }
            else
            {
                currentPlayerBudget -= 0;
            }
            PlayerHoldings.currentPlayerBudget = currentPlayerBudget;
            isJustCreated = false;

            furnituresCreated.Add(furniture);
            furnitureObjectsCreated.Add(manipulationFurn);
        }

        manipulationFurn = null;
        manipulationFurnCollider.isTrigger = false;
        placeObject.SetActive(false);
    }

    ///<summary> Manipulate object
    ///</summary>
    private void ManipulateObject(/*GameObject objectToManipulate*/)
    {
        //objectToManipulate.layer = 8;         //layer to manipulationFurn layer
        //manipulationFurn = objectToManipulate;
        manipulationFurn.layer = PlayerFurnitureManipulationLayer;         //layer to manipulationFurn layer
        gameObject.GetComponent<FirstPersonController>().enabled = true;
        var position = manipulationFurn.transform.position;

        if (isMoveFineTune) //moves object for moveunits
            {
                if (isMoveForwardClicked)
                {
                    manipulationFurn.transform.position += Vector3.forward * moveUnit;
                }
                if (isMoveBackwardClicked)
                {
                    manipulationFurn.transform.position += Vector3.back * moveUnit;
                }
                if (isMoveLeftClicked)
                {
                    manipulationFurn.transform.position += Vector3.left * moveUnit;
                }
                if (isMoveRightClicked)
                {
                    manipulationFurn.transform.position += Vector3.right * moveUnit;
                }
            }

            if (GetComponent<PlayerInteraction>().RaycastHitOrNot())
            {
                position.x = GetComponent<PlayerInteraction>().RaycastHitPoint().x;
                position.y = GetComponent<PlayerInteraction>().RaycastHitPoint().y;
                position.z = GetComponent<PlayerInteraction>().RaycastHitPoint().z;
                manipulationFurn.transform.position = position;
            }

    }

    ///<summary> Rotate by angle
    ///<param name="rotateAngle"> Rotation Angle </param>
    ///</summary>
    private void Rotate(float rotateAngle)
    {
        manipulationFurn.transform.Rotate(0, rotateAngle, 0, Space.World);
    }

    ///<summary> Cancel moving manipulationFurn Object
    ///</summary>
    private void CancelObject()
    {
        if(tablet.activeSelf == true)
        {
            tablet.GetComponent<CanvasGroup>().alpha = 1;
            tablet.GetComponent<CanvasGroup>().interactable = true;
            tablet.GetComponent<CanvasGroup>().blocksRaycasts = true;
            gameObject.GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<FirstPersonController>().enabled = true;
            inGame.SetActive(true);
        }

        if(isJustCreated)
        {
            Destroy(manipulationFurn.gameObject);
        }
        else
        {
            manipulationFurn.transform.position = objectPosition;
            manipulationFurn.transform.rotation = objectRotation;
        }
        manipulationFurnCollider.isTrigger = false;
        manipulationFurn = null;
        placeObject.SetActive(false);
    }
    ///<summary> sell Object
    ///</summary>
    private void SellObject()
    {
        int sellPrice = 0;
        if (SceneManager.GetActiveScene().name == "UserHouse")
        {
            if (manipulationFurn.tag == "Bucket")
            {
                sellPrice = paintingPrice * percent / 100;
            }
            else if (manipulationFurn.tag == "Furniture")
            {
                int furniturePrice = 0;
                foreach (Furnitures furniture in furnitureDatabase.furnitures)
                {
                    if (furniture.PrefabPath == manipulationFurn.name)
                    {
                        furniturePrice = furniture.Price;
                    }
                }

                sellPrice = furniturePrice * percent / 100;
            }
            else if (manipulationFurn.tag == "FlooringBox")
            {
                sellPrice = flooringPrice * percent / 100;
            }
            else if (manipulationFurn.tag == "UnsaleableObjects")
            {
                sellPrice = 0;
            }
        }
        else
        {
            sellPrice = 0;
        }

        gameObject.GetComponent<FirstPersonController>().enabled = true;
        inGame.SetActive(true);
        currentPlayerBudget += sellPrice;

        defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationPanel.SetActive(true);
        sellNotification.text = string.Format(DefaultNotificationTexts.itemSoldInformation, manipulationFurn.name, sellPrice.ToString());

        furnituresCreated.Remove(furniture);
        furnitureObjectsCreated.Remove(manipulationFurn);
        Destroy(manipulationFurn.gameObject);

        PlayerHoldings.currentPlayerBudget = currentPlayerBudget;

        manipulationFurnCollider.isTrigger = false;
        manipulationFurn = null;
        placeObject.SetActive(false);
    }
}
