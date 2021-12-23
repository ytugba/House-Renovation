using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseEstimation : MonoBehaviour
{ //ON REAL ESTATE AGENT PANEL
    #region variables
    private GameObject currentHouse;
    private int playerBudget;

    public GameObject tabletButton;
    public GameObject tablet;

    public TextMeshProUGUI offerText;
    public TextMeshProUGUI offer;

    public GameObject interestedButton;
    public GameObject maybeLaterButton;

    [Header("Enter the values you wish")]
    [Header("Ratio: %X")]
    [Header("Value: count * perValue")]
    public int baseValue;
    private int priceRandomizationRatio;

    public int perFurnitureValueRatio;
    public int perFloorValue;
    public int perWallValue;
    public int perGarbageValue;
    public int perDirtValue;
    public int perBucketValue;
    public int perPlasterBucketValue;
    public int perFlooringBoxValue;
    public int perCrackValue;
    public int perPlasteredCrackValue;
    public int perWindowValue;
   
    private int totalValue;
    #endregion

    private void Awake()
    {
        gameObject.SetActive(false);
        priceRandomizationRatio = Random.Range(-10, 10);
        playerBudget = PlayerHoldings.currentPlayerBudget;
        currentHouse = GameObject.Find("UserHouse");

    }
    
    private int SellHouse()
    {//ESTIMATE THE TOTAL VALUE OF THE HOUSE HERE
        totalValue = 0;
        int totalEarning = GetBase() + GetWall() + GetFloor() + GetWindow() + GetFurniture();
        int totalLoss = GetGarbage() + GetDirt() + GetCrack() + GetPlasteredCrack() + GetFlooringBox() + GetBucket() + GetPlasterBucket();
        totalValue +=  totalEarning - totalLoss;
        totalValue += (totalValue / 100) * priceRandomizationRatio;
        return totalValue;
    }

    #region estimationCalculationFunctions
    private int GetPlasteredCrack()
    {
        int totalPlasteredCrackValue = 0;

        GameObject[] plasteredCracks = GameObject.FindGameObjectsWithTag("PlasteredCrack");
        totalPlasteredCrackValue = perPlasteredCrackValue * plasteredCracks.Length;
        return totalPlasteredCrackValue;
    }

    private int GetGarbage()
    {
        int totalGarbageValue = 0;

        GameObject[] garbageAmount = GameObject.FindGameObjectsWithTag("Garbage");
        totalGarbageValue = perGarbageValue * garbageAmount.Length;

        return totalGarbageValue;
    }

    private int GetBucket()
    {
        int totalBucketValue = 0;

        GameObject[] bucketAmount = GameObject.FindGameObjectsWithTag("Bucket");
        totalBucketValue = perBucketValue * bucketAmount.Length;

        return totalBucketValue;
    }

    private int GetPlasterBucket()
    {
        int totalPlasterBucketValue = 0;

        GameObject[] bucketAmount = GameObject.FindGameObjectsWithTag("PlasterBucket");
        totalPlasterBucketValue = perPlasterBucketValue * bucketAmount.Length;

        return totalPlasterBucketValue;
    }

    private int GetFlooringBox()
    {
        int totalFlooringBoxValue = 0;

        GameObject[] boxAmount = GameObject.FindGameObjectsWithTag("FlooringBox");
        totalFlooringBoxValue = perFlooringBoxValue * boxAmount.Length;

        return totalFlooringBoxValue;
    }

    private int GetCrack()
    {
        int totalCrackValue = 0;

        GameObject[] crackAmount = GameObject.FindGameObjectsWithTag("Crack");
        totalCrackValue = perCrackValue * crackAmount.Length;

        return totalCrackValue;
    }

    private int GetDirt()
    {
        int totalDirtValue = 0;

        GameObject[] dirtAmount = GameObject.FindGameObjectsWithTag("Dirt");
        totalDirtValue = perDirtValue * dirtAmount.Length;

        return totalDirtValue;
    }

    private int GetFurniture()
    {
        int totalFurnitureValue = 0;

        GameObject[] furnituresInHouse = GameObject.FindGameObjectsWithTag("Furniture");
        foreach(var furniture in furnituresInHouse)
        {
            totalFurnitureValue += (furniture.GetComponent<FurnitureCollisionCheck>().furniturePrice * perFurnitureValueRatio / 100);
        }
        return totalFurnitureValue;
    }

    private int GetWindow()
    {
        int totalWindowValue = 0;

        GameObject[] windowAmount = GameObject.FindGameObjectsWithTag("Window");
        totalWindowValue = perWindowValue * windowAmount.Length;

        return totalWindowValue;
    }

    private int GetFloor()
    {
        int totalFloorValue = 0;
        int modified = 0;
        int unmodified = 0;
        GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");

        foreach (var floor in floors)
        {
            if (floor.GetComponent<FloorInformation>().hasFloored)
            {
                modified++;
            }
            else
            {
                unmodified++;
            }
        }
        totalFloorValue -= (perFloorValue * unmodified);
        totalFloorValue += (perFloorValue * modified);
        return totalFloorValue;
    }

    private int GetWall()
    {
        int totalWallValue = 0;
        int modified = 0;
        int unmodified = 0;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {
            if(wall.GetComponent<WallInformation>().hasPainted)
            {
                modified++;
            }
            else
            {
                unmodified++;
            }
        }

        totalWallValue -=(perWallValue * unmodified);
        totalWallValue += (perWallValue * modified);
        return totalWallValue;
    }

    private int GetBase()
    {
        return baseValue;
    }
    #endregion

    public void OnAgentClick()
    {
        if (!gameObject.activeSelf)
        {
            totalValue = SellHouse();
            offer.text = totalValue.ToString();

            gameObject.SetActive(true);
            offer.gameObject.SetActive(true);
            interestedButton.SetActive(true);
            maybeLaterButton.SetActive(true);
            tablet.GetComponent<CanvasGroup>().alpha = 0;
            tablet.GetComponent<CanvasGroup>().blocksRaycasts = false;
            tablet.GetComponent<CanvasGroup>().interactable = false;
            tabletButton.SetActive(false);
            offerText.text = "Hello host! You have a very nice house. Here is the offer I can give you for this house:";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnInterestedClick()
    {
        Debug.Log("sale's completed.");
        offer.gameObject.SetActive(false);
        offerText.text = "Great! It was a pleasure doing business with you. See you later!";
        Destroy(currentHouse);
        PlayerHoldings.currentPlayerBudget += totalValue;
        //DESTROY THE SOLD HOUSE HERE
        Debug.Log("Scene loading: " + "AfterSellScene");
        SceneManager.LoadScene("AfterSellScene");
        StartCoroutine(ClosePanel());
    }

    public void OnMaybeLaterClick()
    {
        Debug.Log("sale's cancelled.");
        offer.gameObject.SetActive(false);
        offerText.text = "Alright! If you think about selling, you know where to find me.";
        StartCoroutine(ClosePanel());
    }

    private IEnumerator ClosePanel()
    {
        interestedButton.SetActive(false);
        maybeLaterButton.SetActive(false);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        tablet.GetComponent<CanvasGroup>().alpha = 1;
        tablet.GetComponent<CanvasGroup>().blocksRaycasts = true;
        tablet.GetComponent<CanvasGroup>().interactable = true;
        tabletButton.SetActive(true);
    }
}
