using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// this class loads missions to the scene
/// </summary>
public class OnMissionStart : MonoBehaviour
{
    string missionName;
    public GameObject missionCompletedPanel;
    public TextMeshProUGUI rewardText;
    GameObject fps;
    public GameObject tablet;
    public GameObject[] tabletButtonsHiddenInMission;
    [Header("Beginner Cleaning Mission")]
    public int BeginnerCleaningDirt;
    public int BeginnerCleaningGarbage;
    public int BeginnerCleaningCrack;
    [Header("Intermediate Cleaning Mission")]
    public int IntermediateCleaningDirt;
    public int IntermediateCleaningGarbage;
    public int IntermediateCleaningCrack;

    [Header("Advanced Cleaning Mission")]
    public int AdvancedCleaningDirt;
    public int AdvancedCleaningGarbage;
    public int AdvancedCleaningCrack;

    [Header("HouseDesign101 Mission")]
    public int HouseDesign101Dirt;
    public int HouseDesign101Garbage;
    public int HouseDesign101Crack;

    public TextMeshProUGUI garbageLeft;
    public TextMeshProUGUI dirtLeft;
    public TextMeshProUGUI crackLeft;

    void Awake()
    {
        missionCompletedPanel.SetActive(false);
        fps = GameObject.FindGameObjectWithTag("Player");
        missionName = MissionComplete.missionName;
        switch(missionName)
        {
            case "Beginner Cleaning":
                BeginnerCleaning();
                break;
            case "Intermediate Cleaning":
                IntermediateCleaning();
                break;
            case "Advanced Cleaning":
                AdvancedCleaning();
                break;
            case "House Design 101":
                HouseDesign101();
                break;
            default:
                Debug.Log("Mission Selection error.");
                break;
        }
    }

    private void HouseDesign101()
    {
        GetComponent<DirtSpawner>().defaultQuantity = 0;
        GetComponent<SpawnRandomGarbage>().defaultQuantity = 0;
        GetComponent<CrackSpawner>().spawnRatio = 0;
        
        foreach(GameObject furniture in GameObject.FindGameObjectsWithTag("Furniture"))
        {
            Destroy(furniture);
        }
        fps.GetComponent<PlayerFurnitureManipulation>().furnituresCreated = null;
        fps.GetComponent<PlayerFurnitureManipulation>().furnitureObjectsCreated = null;
    }

    private void AdvancedCleaning()
    {
        GetComponent<DirtSpawner>().defaultQuantity = AdvancedCleaningDirt;
        GetComponent<SpawnRandomGarbage>().defaultQuantity = AdvancedCleaningGarbage;
        GetComponent<CrackSpawner>().spawnRatio = AdvancedCleaningCrack;
    }

    private void IntermediateCleaning()
    {
        GetComponent<DirtSpawner>().defaultQuantity = IntermediateCleaningDirt;
        GetComponent<SpawnRandomGarbage>().defaultQuantity = IntermediateCleaningGarbage;
        GetComponent<CrackSpawner>().spawnRatio = IntermediateCleaningCrack;
    }

    private void BeginnerCleaning()
    {
        GetComponent<DirtSpawner>().defaultQuantity = BeginnerCleaningDirt;
        GetComponent<SpawnRandomGarbage>().defaultQuantity = BeginnerCleaningGarbage;
        GetComponent<CrackSpawner>().spawnRatio = BeginnerCleaningCrack;
    }

    private void Update()
    {

        garbageLeft.text = GetComponent<SpawnRandomGarbage>().garbagesCreatedOnScene.Count.ToString();
        crackLeft.text = GetComponent<CrackSpawner>().cracksCreatedOnScene.Count.ToString();
        dirtLeft.text = GetComponent<DirtSpawner>().dirtsCreatedOnScene.Count.ToString();

        if (GetComponent<CrackSpawner>().cracksCreatedOnScene.Count == 0 && GetComponent<SpawnRandomGarbage>().garbagesCreatedOnScene.Count == 0 && GetComponent<DirtSpawner>().dirtsCreatedOnScene.Count == 0 && !MissionComplete.isCompleted && GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHoldings>().currentGarbage == 0)
        {
            Debug.Log("Mission is completed!");
            MissionComplete.isCompleted = true;
            StartCoroutine(LoadAscnchronously("UserHouse"));    
        }

        foreach (var button in tabletButtonsHiddenInMission)
        {
            if (SceneManager.GetActiveScene().name == "MissionScene")
            {
                button.SetActive(false);
            }

            else
            {
                button.SetActive(true);
            }
        }

    }

    IEnumerator LoadAscnchronously(string scene)
    {
        missionCompletedPanel.SetActive(true);
        rewardText.text = MissionComplete.expectedReward.ToString();
        yield return new WaitForSeconds(3);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
    }
}
