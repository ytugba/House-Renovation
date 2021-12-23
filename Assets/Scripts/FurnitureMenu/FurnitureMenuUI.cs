using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureMenuUI : MonoBehaviour
{
    private GameObject instance;

    public List<FurnitureUI> uiFurnitures = new List<FurnitureUI>();
    public FurnitureDatabase furnitureDatabase;
    public GameObject slotPrefab;
    public Transform slotPanel;

    void Awake()
    {
        for (int i = 0; i < furnitureDatabase.availableFurnitureNumber; i++) //Creates the slots according to the number that is held while reading database
        {
            instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            uiFurnitures.Add(instance.GetComponentInChildren<FurnitureUI>());
            gameObject.SetActive(false);
        }
    }

    public void UpdateSlot(int slot, Furnitures furniture)
    {
            uiFurnitures[slot].UpdateFurniture(furniture);
    }

    public void AddNewFurniture(Furnitures furniture) //Adds new furniture to the slots not the game itself
    {
            UpdateSlot(uiFurnitures.FindIndex(i => i.furniture == null), furniture);
    }

    /*
    public void RemoveFurniture(Furnitures furniture) //For inventory system this function can be used in future. DON'T DELETE
    {
        UpdateSlot(uiFurnitures.FindIndex(i => i.furniture == furniture), null);
    }
    */
}
