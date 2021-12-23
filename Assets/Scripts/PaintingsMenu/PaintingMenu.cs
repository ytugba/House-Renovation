using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMenu : MonoBehaviour
{
    public List<Paintings> paintingList = new List<Paintings>();
    public PaintingDatabase paintingDatabase;
    public PaintingMenuUI paintingMenuUI;

    private void Awake()
    {
        paintingDatabase = GameObject.Find("PaintingDatabase").GetComponent<PaintingDatabase>();
        paintingMenuUI = GameObject.Find("PaintingMenuPanel").GetComponent<PaintingMenuUI>();
    }

    void Start() //Reads database values here
    {
       for (int i = 1; i <= paintingDatabase.availablePaintingNumber; i++)
        {
            GivePainting(i);
        }
    }

    public void GivePainting(int id) //using these functions furnitures can be loaded
    {
        Paintings paintingToAdd = paintingDatabase.GetPainting(id);
        paintingList.Add(paintingToAdd);
        paintingMenuUI.AddNewPainting(paintingToAdd);
        //Debug.Log("Added painting: " + paintingToAdd.MaterialPath);
    }
}