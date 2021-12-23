using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrackCollisionDetection : MonoBehaviour
{
    CrackSpawner crackSpawner;

    private bool alreadyDead = false;

    private void Awake()
    {
        crackSpawner = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CrackSpawner>();
    }

    private void Update()
    {
        if(gameObject.name == "PlasteredCrack")
        {
            gameObject.tag = "PlasteredCrack";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Window"))
        {
            gameObject.SetActive(false);
            crackSpawner.cracksCreated.Remove(this.gameObject);
            crackSpawner.cracksCreatedOnScene.Remove(this.gameObject);
            Destroy(this.gameObject);
            crackSpawner.maxCrackCreated++;
        }

        if(collision.collider.tag == "Crack")
        {
            if (!alreadyDead)
            {
                CrackCollisionDetection script = collision.collider.gameObject.GetComponent<CrackCollisionDetection>();
                if (script != null)
                {
                    script.alreadyDead = true;
                    collision.collider.gameObject.SetActive(false);
                    crackSpawner.cracksCreated.Remove(collision.collider.gameObject);
                    crackSpawner.cracksCreatedOnScene.Remove(collision.collider.gameObject);
                    crackSpawner.maxCrackCreated++;
                    Destroy(collision.collider.gameObject);
                }
            }
        }
    }
}
