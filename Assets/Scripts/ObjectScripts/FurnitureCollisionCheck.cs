using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureCollisionCheck : MonoBehaviour
{
    [HideInInspector]
    public int furniturePrice;
    private GameObject fps;
    private FurnitureDatabase furnitureDatabase;
    private PlayerFurnitureManipulation playerfurnitureManipulation;
    private List<string> colliderTags;

    public PlayerTools fixerTool;
    private void Awake()
    {
        fps = GameObject.FindGameObjectWithTag("Player");
        furnitureDatabase = GameObject.Find("FurnitureDatabase").GetComponent<FurnitureDatabase>();

        colliderTags = new List<string>() { "Garbage", "Wall", "UnsaleableObjects", "Window", "Furniture", "FlooringBox", "Player", "PaintRoller" };
        playerfurnitureManipulation = fps.GetComponent<PlayerFurnitureManipulation>();
        fixerTool = PlayerTools.Hand;
    }

    private void Start()
    {
        if(gameObject.activeInHierarchy)
        {
            foreach (Furnitures furniture in furnitureDatabase.furnitures)
            {
                if (furniture.Title == gameObject.name)
                {
                    furniturePrice = furniture.Price;
                }
            }
        }
    }
    //Colliders we don't want to place object on them
    private void OnTriggerStay(Collider collider)
    {
        if(gameObject.activeInHierarchy && colliderTags.Contains(collider.tag))
        {
            playerfurnitureManipulation.PlaceButton.interactable = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (gameObject.activeInHierarchy && colliderTags.Contains(collider.tag))
        {
            playerfurnitureManipulation.PlaceButton.interactable = true;
        }
    }
}
