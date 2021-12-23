using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackSpawner : MonoBehaviour
{
    public List<GameObject> Walls;
    public List<GameObject> cracksCreated;
    public List<GameObject> cracksCreatedOnScene;
    public GameObject[] crackTypes;
    [Header("Don't change this value. Only to watch")]
    public int spawnUpdate;
    private int randomWallIndex;
    private int randomCrackType;
    private float scaleOfCrack;

    [Header("Walls.Count * spawnRatio / 100")]
    public int spawnRatio;
    [HideInInspector]
    public int maxCrackCreated;
    [Header("Don't change this value. Only to watch")]
    public int spawnCheck;

    private void Start()
    {
        foreach (var wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Walls.Add(wall);
        }
        cracksCreated = new List<GameObject>();
        cracksCreatedOnScene = new List<GameObject>();
        maxCrackCreated = (int)(Walls.Count * spawnRatio / 100);
        spawnCheck = maxCrackCreated;
    }

    private void FixedUpdate()
    {
        if(spawnCheck > cracksCreated.Count)
        {
            Spawn();
            spawnUpdate++;
        }
        else
        {
            maxCrackCreated = 0;
        }

    }

    void Spawn()
    {

        while (maxCrackCreated > 0 && spawnCheck > cracksCreated.Count)
        {
            scaleOfCrack = Random.Range(0.3f, 0.5f);
            randomWallIndex = Random.Range(0, Walls.Count);
            randomCrackType = Random.Range(0, crackTypes.Length);

            GameObject crack;
            
            Transform parent = Walls[randomWallIndex].transform;
            crackTypes[randomCrackType].transform.localScale = new Vector3(scaleOfCrack, scaleOfCrack, scaleOfCrack);
            float[] WallBounds =
            {
                Walls[randomWallIndex].GetComponent<Renderer>().bounds.size.x,
                Walls[randomWallIndex].GetComponent<Renderer>().bounds.size.y,
                Walls[randomWallIndex].GetComponent<Renderer>().bounds.size.z
            }; //size x,y,z
            float[] CrackBounds =
            {
                crackTypes[randomCrackType].GetComponent<Renderer>().bounds.size.x,
                crackTypes[randomCrackType].GetComponent<Renderer>().bounds.size.y,
                crackTypes[randomCrackType].GetComponent<Renderer>().bounds.size.z
            }; //size x,y,z
            float[] AvailableAreaX =
            {
                -WallBounds[0]/2,
                 WallBounds[0]/2
            }; //minx,maxx area for a wall
            float[] AvailableAreaZ =
                {
                -WallBounds[2]/2,
                WallBounds[2]/2
            }; //minx, maxx area for a wall

            Vector3 crackPos;
            crackPos.x = Random.Range(AvailableAreaX[0], AvailableAreaX[1]);
            crackPos.z = Random.Range(AvailableAreaZ[0], AvailableAreaZ[1]);
            crackPos.y = 0.2f;
            
            crack = Instantiate(crackTypes[randomCrackType], crackPos, parent.rotation, parent);
            crack.name = crack.name.Replace("(Clone)", "");
            crack.transform.localPosition = crackPos;
            //crack.transform.Rotate(0, Random.Range(0, 360), 0);
            cracksCreated.Add(crack);
            cracksCreatedOnScene.Add(crack);
            maxCrackCreated--;
        }
    }
    
}
