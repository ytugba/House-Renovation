using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformation : MonoBehaviour
{
    [Header("Fill the fix time and garbage amount")]
    public PlayerTools fixerTool;           //tool to fix that deformation

    [HideInInspector]
    public float garbageAmount;             //if this deformation is a garbage, this amount will add to playerHoldings currentGarbage

    private SpawnRandomGarbage spawnRandomGarbage;
    private DirtSpawner dirtSpawner;

    private void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag == "Garbage")
        {
             if (collision.collider.gameObject != GameObject.FindGameObjectWithTag("Player") || collision.collider.gameObject != GameObject.FindGameObjectWithTag("Room") || gameObject.activeInHierarchy)
            {
                spawnRandomGarbage = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SpawnRandomGarbage>();
                gameObject.SetActive(false);
                spawnRandomGarbage.garbagesCreated.Remove(this.gameObject);
                spawnRandomGarbage.garbagesCreatedOnScene.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
        else if(gameObject.tag == "Dirt")
        {
             if (collision.collider.gameObject != GameObject.FindGameObjectWithTag("Player") || collision.collider.gameObject != GameObject.FindGameObjectWithTag("Room") || gameObject.activeInHierarchy)
            {
                dirtSpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DirtSpawner>();
                gameObject.SetActive(false);
                dirtSpawner.dirtsCreated.Remove(this.gameObject);
                dirtSpawner.dirtsCreatedOnScene.Remove(this.gameObject);
                Destroy(this.gameObject);
            }   
        }
    }
}
