using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMenuUI : MonoBehaviour
{
    private GameObject instance;

    public List<PaintingUI> paintingUIs = new List<PaintingUI>();
    public PaintingDatabase paintingDatabase;
    public GameObject slotPrefab;
    public Transform slotPanel;

    void Awake()
    {
        for (int i = 0; i < paintingDatabase.availablePaintingNumber; i++) //Creates the slots according to the number that is held while reading database
        {
            instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            paintingUIs.Add(instance.GetComponentInChildren<PaintingUI>());
            gameObject.SetActive(false);
        }
    }

    public void UpdateSlot(int slot, Paintings paint)
    {
        paintingUIs[slot].UpdatePainting(paint);
    }

    public void AddNewPainting(Paintings paint) //Adds new furniture to the slots not the game itself
    {
            UpdateSlot(paintingUIs.FindIndex(i => i.paintings == null), paint);
    }

    /*
    public void RemoveFurniture(Furnitures furniture) //For inventory system this function can be used in future. DON'T DELETE
    {
        UpdateSlot(uiFurnitures.FindIndex(i => i.furniture == furniture), null);
    }
    */
}
