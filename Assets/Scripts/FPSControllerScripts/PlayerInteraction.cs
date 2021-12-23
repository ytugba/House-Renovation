using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{
    #region variables
    public PlayerTools currentPlayerTool;
    public TextMeshProUGUI focusedAction;

    public Camera fpsCamera;
    private GameObject gameManager;
    public GameObject focusedObject;
    public GameObject interactionCycle;             //UI object to display click down time
    public TMP_Dropdown playerToolsDropdown;

    public Animator broomAnimator;
    public Animator paintAnimator;
    public Animator trowelAnimator;

    public GameObject defaultNotificationTexts;

    public GameObject broom;
    public GameObject trowel;
    private GameObject paintRollerPrefab;

    bool isDust = false;
    bool isPainting = false;
    bool isPlasting = false;

    /// <summary>
    /// required time for interactions and
    /// times that will pass
    /// </summary>
    #region interactionTimes
    public float interactionRange;
    private float interactionMaxTime;          

    private float furnMoveTime;
    public float furnMoveCompleteTime;     

    private float paintingTime;
    public float requiredPaintingTime;

    private float deformationTime;
    public float requiredDeformationTime;

    private float plasteringTime;
    public float requiredPlasteringTime;

    private float gettingPlasterTime;
    public float requiredGettingPlasterTime;

    private float gettingPaintTime;
    public float requiredGettingPaintingTime;

    private float gettingFloorTime;
    public float requiredGettingFloorTime;

    private float flooringTime;
    public float requiredFlooringTime;
    #endregion

    private Material selectedPaintingMaterial;
    private Material plasteredCrack;
    private Material selectedFlooringMaterial;
    public Material defaultRollerMaterial;
    public Material plasterOnTrowel;
    public Material defaultTrowel;

    #endregion

    private void Awake()
    {
        paintRollerPrefab = GameObject.Find("Paint Roller");
        paintRollerPrefab.SetActive(false);
        broom.SetActive(false);
        broom.transform.Find("DustFlying").gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        playerToolsDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { PlayerToolsDropdownChanged(); });
        ClickDownReset();               //Reset click down at beginning
    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, interactionRange))
            {
                focusedObject = hit.collider.gameObject;
            }

            else
            {
                focusedObject = null;
            }


            if (currentPlayerTool == PlayerTools.Hand)
            {
                if (focusedObject != null && hit.distance <= 3f)
                {
                    if (focusedObject.tag == "Garbage")
                    {
                        focusedAction.text = "Collect Garbage";
                    }
                    else if (focusedObject.tag == "Furniture" || focusedObject.tag == "UnsaleableObjects")
                    {
                        focusedAction.text = "Move";
                    }
                    else if(focusedObject.tag == "trashCan")
                    {
                        focusedAction.text = "Throw Garbage";
                    }
                }
                else
                {
                    focusedAction.text = "";
                }
            }


            if (currentPlayerTool == PlayerTools.Painting)
            {
                if (focusedObject != null && hit.distance <= 3f)
                {
                    if (focusedObject.tag == "Wall" || focusedObject.tag == "PlasteredCrack")
                    {
                        focusedAction.text = "Paint Wall";
                    }
                    else if (focusedObject.name == "Bucket")
                    {
                        focusedAction.text = "Get Paint";
                    }
                }
                else
                {
                    focusedAction.text = "";
                }

                paintRollerPrefab.SetActive(true);
            }
            else
            {
                paintRollerPrefab.SetActive(false);
            }

            if (currentPlayerTool == PlayerTools.Cleaning)
            {
                if (focusedObject != null && hit.distance <= 4f)
                {
                    if (focusedObject.tag == "Dirt")
                    {
                        focusedAction.text = "Clean Floor";
                    }
                }
                else
                {
                    focusedAction.text = "";
                }

                broom.SetActive(true);
            }
            else
            {
                broom.SetActive(false);
            }


            if (currentPlayerTool == PlayerTools.CrackRemover)
            {
                trowel.SetActive(true);

                if (focusedObject != null && hit.distance <= 3f)
                {
                    if (focusedObject.tag == "Crack")
                    {
                        focusedAction.text = "Plaster Crack";
                    }
                    else if(focusedObject.name == "Plaster Bucket")
                    {
                        focusedAction.text = "Get Plaster";
                    }
                }
                else
                {
                    focusedAction.text = "";
                }
            }
            else
            {
                trowel.SetActive(false);
            }


            if (currentPlayerTool == PlayerTools.Flooring)
            {
                if (focusedObject != null && hit.distance <= 3f)
                {
                    if (focusedObject.name == "Flooring Box")
                    {
                        focusedAction.text = " Get Floor";
                    }
                }
                else
                {
                    focusedAction.text = "";
                }
            }

            if (Input.GetMouseButton(0))
            {
                Interaction();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ClickDownReset();
                flooringTime =0;
                deformationTime = 0;
                paintingTime = 0;
                furnMoveTime = 0;
                plasteringTime = 0;
                gettingFloorTime = 0;
                gettingPlasterTime =0;
                gettingPaintTime = 0;
                isDust = false;
                broomAnimator.SetBool("isDust", isDust);
                isPlasting = false;
                trowelAnimator.SetBool("isPlasting", isPlasting);
                isPainting = false;
                paintAnimator.SetBool("isPainting", isPainting);
            }
        }
    }

    ///<summary> This function will call when player tool has changed.
    ///</summary>
    private void PlayerToolsDropdownChanged()
    {
        currentPlayerTool = (PlayerTools)playerToolsDropdown.GetComponent<TMP_Dropdown>().value;
    }

    #region interactionFunctions
    /********************** Interaction Functions *************************/
    ///<summary> Interaction with any object
    ///</summary>
    private void Interaction()
    {
        if (focusedObject != null)
        {
            if (focusedObject.layer == 8) //Add this tag to manipulate the object
            {
                InteractionWithFurn();
            }

            if(currentPlayerTool == PlayerTools.Flooring)
            {
                if(focusedObject.tag == "Floor")
                {
                    InteractionWithFloor();
                }
                else if(focusedObject.name == "Flooring Box")
                {
                    InteractionForGettingFlooring();
                }
            }

            if(currentPlayerTool == PlayerTools.Painting)
            {
                if(focusedObject.tag == "Crack")
                {
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationPanel.SetActive(true);
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationText.text = DefaultNotificationTexts.paintingNotification;
                }

                else
                {
                    InteractionForGettingPaint();
                }
            }

            if (focusedObject.GetComponent<Deformation>() != null)
            {
                InteractionWithDeform();
            }

            if (focusedObject.tag == "trashCan")
            {
                GetComponent<PlayerHoldings>().currentGarbage = 0;
                GetComponent<PlayerHoldings>().UpdateBars();
            }

            if(currentPlayerTool == PlayerTools.CrackRemover)
            {
                Debug.Log(focusedObject.name);
                if (focusedObject.tag == "Crack")
                {
                    InteractionWithCrack();
                }
                else if(focusedObject.name == "Plaster Bucket")
                {
                    InteractionWithPlasterBucket();
                }

            }
        }
    }

    ///<summary> Interaction with deformation
    ///</summary>
    private void InteractionWithDeform()
    {
        if (currentPlayerTool == focusedObject.GetComponent<Deformation>().fixerTool)
        {
            if (CheckHoldingsForDeform(focusedObject))
            {
                deformationTime += Time.deltaTime;
                ClickDownShow(requiredDeformationTime, deformationTime);
                CheckDeformFixComplete(focusedObject);
            }
        }
    }

    ///<summary> Check interaction complete
    ///</param name="deformation"> Deformation that will process
    ///</summary>
    ///
    private void CheckDeformFixComplete(GameObject deformation)
    {
        if (deformation.GetComponent<Deformation>() != null)
        {

            broomAnimator.enabled = true;
            broomAnimator = broom.GetComponent<Animator>();
            isDust = true;
            if (deformationTime < requiredDeformationTime)
            {
                broom.transform.Find("DustFlying").gameObject.SetActive(true);
                isDust = true;
                broomAnimator.SetBool("isDust", isDust);
            }

            if (deformationTime >= requiredDeformationTime)
            {
                if (deformation.tag == "Garbage")
                {
                    gameManager.GetComponent<SpawnRandomGarbage>().garbagesCreatedOnScene.Remove(deformation.gameObject);
                    RemoveGarbage(deformation);
                }

                if (deformation.tag == "Dirt")
                {
                    gameManager.GetComponent<DirtSpawner>().dirtsCreatedOnScene.Remove(deformation.gameObject);
                    isDust = false;
                    broomAnimator.SetBool("isDust", isDust);
                    RemoveDirt(deformation, isDust);
                }
            }
        }
    }

    ///<summary> Check holdings are enough for remove deformation, return true (holdings enough) or return false (not enough) 
    ///</param name="deformation"> Deformation that will process
    ///</summary>
    private bool CheckHoldingsForDeform(GameObject deformation)
    {
        if (deformation.tag == "Garbage")
        {
            if (deformation.GetComponent<Deformation>().garbageAmount < GetComponent<PlayerHoldings>().maxGarbage - GetComponent<PlayerHoldings>().currentGarbage)
            {
                return true;
            }
            else
            {
                Debug.Log("Not enough place to remove garbage");
                return false;
            }
        }

        return true;
    }

    ///<summary> Process' after deformation removed
    ///</param name="deformation"> Deformation that will process
    ///</summary>
    private void RemoveGarbage(GameObject deformation)
    {
        GetComponent<PlayerHoldings>().currentGarbage += deformation.GetComponent<Deformation>().garbageAmount;
        Destroy(deformation);
        ClickDownReset();
        GetComponent<PlayerHoldings>().UpdateBars();
        deformationTime = 0;
    }

    /// <summary>
    /// Remove dirt on the floor
    /// </summary>
    /// <param name="dirt"></param>
    /// <param name="isDust"></param>
    private void RemoveDirt(GameObject dirt, bool isDust)
    {
        broom.transform.Find("DustFlying").gameObject.SetActive(false);
        Debug.Log("Dirt cleaned");
        gameManager.GetComponent<DirtSpawner>().dirtsCreatedOnScene.Remove(dirt);
        isDust = false;
        deformationTime = 0;
        Destroy(dirt);
        ClickDownReset();
    }

    ///<summary> Check interaction complete
    ///</param name="FlooringBox"> Flooring that will process
    ///</summary>
    private void InteractionForGettingFlooring()
    {
        if (selectedFlooringMaterial == null)
        {
            gettingFloorTime += Time.deltaTime;
            ClickDownShow(requiredGettingFloorTime, gettingFloorTime);
            GameObject floorInBox = focusedObject.transform.GetChild(0).transform.GetChild(0).gameObject;
            GameObject bottomOfBox = focusedObject.transform.GetChild(0).transform.GetChild(1).gameObject;

            ClickDownShow(requiredFlooringTime, gettingFloorTime);

            if (requiredFlooringTime < gettingFloorTime && GetComponent<PlayerHoldings>().currentFloor == 0)
            {
                Vector3 floorInBoxPosition = floorInBox.transform.position;
                floorInBoxPosition.y -= 0.2f;

                if (floorInBoxPosition.y >= bottomOfBox.transform.position.y && floorInBox.activeSelf)
                {
                    selectedFlooringMaterial = floorInBox.GetComponent<Renderer>().sharedMaterial;
                    GetComponent<PlayerHoldings>().currentFloor = 1;
                    floorInBox.transform.position = floorInBoxPosition;
                }
                else if (floorInBoxPosition.y < bottomOfBox.transform.position.y)
                {
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationPanel.SetActive(true);
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationText.text = "Empty Box";
                    selectedFlooringMaterial = null;
                    floorInBox.SetActive(false);
                    GetComponent<PlayerHoldings>().currentFloor = 0;
                }
                focusedObject.tag = "UnsaleableObjects";
                gettingFloorTime = 0;
            }
        }
    }

    /// <summary>
    /// Interact with the floor for flooring
    /// </summary>
    /// 
    private void InteractionWithFloor()
    {
        if(selectedFlooringMaterial != null && focusedObject.GetComponent<Renderer>().sharedMaterial != selectedFlooringMaterial)
        {
                flooringTime += Time.deltaTime;
                ClickDownShow(requiredFlooringTime, flooringTime);
                CheckFlooringComplete(focusedObject);
        }
    }

    /// <summary>
    /// Check if the flooring is completed
    /// </summary>
    /// <param name="floor"></param>
    /// 
    private void CheckFlooringComplete(GameObject floor)
    {
        if (requiredFlooringTime <= flooringTime)
        {
            floor.GetComponent<Renderer>().sharedMaterial = selectedFlooringMaterial;
            floor.GetComponent<FloorInformation>().hasFloored = true;
            selectedFlooringMaterial = null;
            flooringTime = 0;
            GetComponent<PlayerHoldings>().currentFloor = 0;
        }
    }

   /// <summary>
   /// Get plaster from the bucket
   /// </summary>
    private void InteractionWithPlasterBucket()
    {
        gettingPlasterTime += Time.deltaTime;

        GameObject plasterInBucket = focusedObject.transform.GetChild(0).gameObject;
        GameObject bottomOfBucket = focusedObject.transform.GetChild(1).gameObject;

        ClickDownShow(requiredGettingPlasterTime, gettingPlasterTime);

        if (requiredGettingPlasterTime < gettingPlasterTime)
        {
            if(GetComponent<PlayerHoldings>().currentPlaster == 0)
            {
                Vector3 plasterInBucketPosition = plasterInBucket.transform.position;
                plasterInBucketPosition.y -= 0.2f;

                if (plasterInBucketPosition.y >= bottomOfBucket.transform.position.y && plasterInBucket.activeSelf)
                {
                    GetComponent<PlayerHoldings>().currentPlaster = 1;
                    trowel.transform.Find("Trowel").Find("Plaster").GetComponent<Renderer>().sharedMaterial = plasterOnTrowel;
                    plasterInBucket.transform.position = plasterInBucketPosition;
                }
                else if (plasterInBucketPosition.y < bottomOfBucket.transform.position.y)
                {
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationPanel.SetActive(true);
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationText.text = "Empty Bucket";
                    plasterInBucket.SetActive(false);
                    GetComponent<PlayerHoldings>().currentPlaster = 0;
                }
                focusedObject.tag = "UnsaleableObjects";
                gettingPlasterTime = 0;
            }
        }
        
    }

    ///<summary> Interaction with crack
    ///</summary>
    private void InteractionWithCrack()
    {
        if(GetComponent<PlayerHoldings>().currentPlaster == 1)
        {
            plasteredCrack = Resources.Load<Material>("Prefabs/Plasters/Plastered" + focusedObject.name);
        }

        if (plasteredCrack != null)
        {
            plasteringTime += Time.deltaTime;
            ClickDownShow(requiredPlasteringTime, plasteringTime);
            CheckPlasteringComplete(focusedObject);
        }
    }

    /// <summary>
    /// Check if the crack plaster time is completed
    /// </summary>
    /// <param name="crack"></param>
    private void CheckPlasteringComplete(GameObject crack)
    {
        isPlasting = trowelAnimator.GetBool("trovelAnimation");
        if (deformationTime < requiredDeformationTime)
        {
            trowelAnimator.SetBool("isPlasting", !isPlasting);
        }

        if (requiredPlasteringTime <= plasteringTime)
        {
            trowelAnimator.SetBool("isPlasting", isPlasting);
            trowel.transform.Find("Trowel").Find("Plaster").GetComponent<Renderer>().sharedMaterial = defaultTrowel;
            crack.GetComponent<Renderer>().material = plasteredCrack;
            GetComponent<PlayerHoldings>().currentPlaster = 0;
            crack.tag = "PlasteredCrack";
            gameManager.GetComponent<CrackSpawner>().cracksCreatedOnScene.Remove(crack);
            plasteringTime = 0;
            plasteredCrack = null;
        }
    }

    ///<summary> Interaction with furnitures
    ///</summary>
    private void InteractionWithFurn()
    {
        if (currentPlayerTool == focusedObject.GetComponent<FurnitureCollisionCheck>().fixerTool)
        {
            furnMoveTime += Time.deltaTime;
            ClickDownShow(furnMoveCompleteTime, furnMoveTime);
            //CheckDeformFixComplete(focusedObject);
            CheckFurnMoveComplete(focusedObject);
        }
    }

    ///<summary> Check move interaction complete
    ///</param name="furniture"> Deformation that will process
    ///</summary>
    private void CheckFurnMoveComplete(GameObject furniture)
    {
        if (furniture.layer == 8)
        {
            if (furnMoveCompleteTime <= furnMoveTime)
            {
                GetComponent<PlayerFurnitureManipulation>().objectPosition = focusedObject.transform.position;
                GetComponent<PlayerFurnitureManipulation>().objectRotation = focusedObject.transform.rotation;
                GetComponent<PlayerFurnitureManipulation>().manipulationFurn = furniture;
                furnMoveTime = 0;
                GetComponent<PlayerFurnitureManipulation>().newManipulationFurn = true;
            }
        }
    }

    /// <summary>
    /// Get Paint from the bucket and paint if the focused obj is wall
    /// </summary>
    private void InteractionForGettingPaint()
    {
        if (focusedObject.name == "Bucket")
        {
            if(selectedPaintingMaterial == null)
            {
                gettingPaintTime += Time.deltaTime;

                GameObject paintInBucket = focusedObject.transform.GetChild(0).gameObject;
                GameObject bottomOfBucket = focusedObject.transform.GetChild(1).gameObject;

                ClickDownShow(requiredGettingPaintingTime, gettingPaintTime);

                if (requiredGettingPaintingTime < gettingPaintTime && GetComponent<PlayerHoldings>().currentPaint == 0)
                {
                    Vector3 paintInBucketPosition = paintInBucket.transform.position;
                    paintInBucketPosition.y -= 0.2f;

                    if (paintInBucketPosition.y >= bottomOfBucket.transform.position.y && paintInBucket.activeSelf)
                    {
                        selectedPaintingMaterial = paintInBucket.GetComponent<Renderer>().sharedMaterial;
                        paintRollerPrefab.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = selectedPaintingMaterial;
                        GetComponent<PlayerHoldings>().currentPaint = 1;
                        paintInBucket.transform.position = paintInBucketPosition;
                    }
                    else if (paintInBucketPosition.y < bottomOfBucket.transform.position.y)
                    {
                        selectedPaintingMaterial = null;
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationPanel.SetActive(true);
                    defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationText.text = "Empty Bucket";
                        paintRollerPrefab.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = defaultRollerMaterial;
                        paintInBucket.SetActive(false);
                        GetComponent<PlayerHoldings>().currentPaint = 0;
                    }
                    focusedObject.tag = "UnsaleableObjects";
                    gettingPaintTime = 0;
                }
            }
        }

        else if (selectedPaintingMaterial != null && (focusedObject.tag == "Wall" || focusedObject.tag == "PlasteredCrack"))
        {
            focusedObject.GetComponent<WallInformation>().isPaintable = true;

            foreach(Transform transform in focusedObject.transform)
            {
                if(transform.CompareTag("Crack"))
                {
                    focusedObject.GetComponent<WallInformation>().isPaintable = false;
                    focusedObject.GetComponent<WallInformation>().hasPainted = false;
                }
            }

            if (focusedObject.GetComponent<WallInformation>().isPaintable && GetComponent<PlayerHoldings>().currentPaint == 1)
            {
                paintingTime += Time.deltaTime;

                paintAnimator = paintRollerPrefab.GetComponent<Animator>();
                isPainting = true;

                if (requiredPaintingTime <= paintingTime)
                {
                    isPainting = false;
                    paintAnimator.SetBool("isPainting", isPainting);
                }

                else if (requiredPaintingTime > paintingTime)
                {
                    isPainting = true;
                    paintAnimator.SetBool("isPainting", isPainting);
                }

                ClickDownShow(requiredPaintingTime, paintingTime);
                CheckPaintingComplete(focusedObject, isPainting);
            }
            else
            {
                defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationPanel.SetActive(true);
                defaultNotificationTexts.GetComponent<DefaultNotificationTexts>().notificationText.text = DefaultNotificationTexts.paintingNotification;
            }
        }
    }

    /// <summary>
    /// check if the wall is painted
    /// </summary>
    /// <param name="wall"> Focused Object </param>
    /// <param name="isPainting"> Animation</param>
    private void CheckPaintingComplete(GameObject wall, bool isPainting)
    {
        if (requiredPaintingTime <= paintingTime)
        {
            wall.GetComponent<Renderer>().sharedMaterial = selectedPaintingMaterial;
            paintRollerPrefab.transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial = defaultRollerMaterial;
            isPainting = false;
            ClickDownReset();
            foreach (Transform crackDeformation in wall.transform)
            {
                if (crackDeformation.tag == "PlasteredCrack")
                {
                    Destroy(crackDeformation.gameObject);
                }
            }
            paintingTime = 0;
            wall.GetComponent<WallInformation>().hasPainted = true;
            selectedPaintingMaterial = null;
            GetComponent<PlayerHoldings>().currentPaint = 0;
        }
    }
    #endregion

    #region interactionDisplay
    /********************** Interaction Functions *************************/
    ///<summary> Click down show
    ///<param name="maxTime"> Maximum time to finish click down time
    ///<param name="completedTime"> Completed time
    ///</summary>
    private void ClickDownShow(float maxTime, float completedTime)
    {
        interactionCycle.SetActive(true);
        interactionCycle.GetComponent<Image>().fillAmount = completedTime / maxTime;
    }

    ///<summary> Reset Click down display
    ///</summary>
    private void ClickDownReset()
    {
        interactionCycle.SetActive(false);
        flooringTime = 0;
        paintingTime = 0;
        plasteringTime = 0;
        furnMoveTime = 0;
        deformationTime = 0;
        broom.transform.Find("DustFlying").gameObject.SetActive(false);
        if(broomAnimator != null)
        {
            broomAnimator.enabled = false;
        }
        //interactionCounter = 0;
    }
    #endregion

    ///<summary> Returns raycast hit point. Returns Vector3(0, 0, 0) when not hitting any object, so use it with RaycastHitOrNot
    ///</summary>
    public Vector3 RaycastHitPoint()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))
            {
                return hit.point;
            }
        }
        return new Vector3(0, 0, 0);
    }

    ///<summary> Returns raycast hit an object or not
    ///</summary>
    public bool RaycastHitOrNot()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = fpsCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))
            {
                return true;
            }
        }
        return false;
    }
}

public enum PlayerTools{
    Hand,
    CrackRemover,
    Cleaning,
    Painting,
    Flooring
}
