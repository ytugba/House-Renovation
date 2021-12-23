using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlooringMenuUI : MonoBehaviour
{
    private GameObject instance;

    public List<FlooringUI> flooringUIs = new List<FlooringUI>();
    public FlooringDatabase flooringDatabase;
    public GameObject slotPrefab;
    public Transform slotPanel;

    void Awake()
    {
        for (int i = 0; i < flooringDatabase.availableFlooringNumber; i++) //Creates the slots according to the number that is held while reading database
        {
            instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel);
            flooringUIs.Add(instance.GetComponentInChildren<FlooringUI>());
            gameObject.SetActive(false);
        }
    }

    public void UpdateSlot(int slot, Floorings floor)
    {
        flooringUIs[slot].UpdateFlooring(floor);
    }

    public void AddNewFlooring(Floorings floor)
    {
        UpdateSlot(flooringUIs.FindIndex(i => i.floorings == null), floor);
    }
}
