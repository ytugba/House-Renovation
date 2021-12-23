using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlooringMenu : MonoBehaviour
{
    public List<Floorings> flooringList = new List<Floorings>();
    public FlooringDatabase flooringDatabase;
    public FlooringMenuUI flooringMenuUI;

    private void Awake()
    {
        flooringDatabase = GameObject.Find("FlooringDatabase").GetComponent<FlooringDatabase>();
        flooringMenuUI = GameObject.Find("FlooringMenuPanel").GetComponent<FlooringMenuUI>();
    }

    void Start() //Reads database values here
    {
        for (int i = 1; i <= flooringDatabase.availableFlooringNumber; i++)
        {
            GiveFlooring(i);
        }
    }

    public void GiveFlooring(int id) //using these functions furnitures can be loaded
    {
        Floorings flooringToAdd = flooringDatabase.GetFlooring(id);
        flooringList.Add(flooringToAdd);
        flooringMenuUI.AddNewFlooring(flooringToAdd);
        //Debug.Log("Added flooring: " + flooringToAdd.MaterialPath);
    }
}