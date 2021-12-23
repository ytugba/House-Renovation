using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureMenu : MonoBehaviour
{
    public List<Furnitures> furnitureList = new List<Furnitures>();
    public FurnitureDatabase furnitureDatabase;
    public FurnitureMenuUI furnitureMenuUI;

    private void Awake()
    {
        furnitureDatabase = GameObject.Find("FurnitureDatabase").GetComponent<FurnitureDatabase>();
        furnitureMenuUI = GameObject.Find("FurnitureMenuPanel").GetComponent<FurnitureMenuUI>();
    }

    void Start() //Reads database values here
    {
       for (int i = 1; i <= furnitureDatabase.availableFurnitureNumber; i++)
        {
            GiveFurniture(i);
        }
    }

    public void GiveFurniture(int id) //using these functions furnitures can be loaded
    {
        Furnitures furnitureToAdd = furnitureDatabase.GetFurniture(id);
        furnitureList.Add(furnitureToAdd);
        furnitureMenuUI.AddNewFurniture(furnitureToAdd);
        //Debug.Log("Added furniture: " + furnitureToAdd.Title);
    }

    public void GiveFurniture(string furnitureName)
    {
        Furnitures furnitureToAdd = furnitureDatabase.GetFurniture(furnitureName);
        furnitureList.Add(furnitureToAdd);
        furnitureMenuUI.AddNewFurniture(furnitureToAdd);
        //Debug.Log("Added furniture: " + furnitureToAdd.Title);
    }

    public Furnitures CheckForFurniture(int id)
    {
        return furnitureList.Find(furniture => furniture.FurnitureId == id);
    }

    /*public void RemoveFurniture(int id) //delete furnitures if you want
    {
        Furnitures furnitureToRemove = CheckForFurniture(id);
        if (furnitureToRemove != null)
        {
            furnitureList.Remove(furnitureToRemove);
            furnitureMenuUI.RemoveFurniture(furnitureToRemove);
            Debug.Log("Removed furniture: " + furnitureToRemove.Title);
        }
    }*/
}