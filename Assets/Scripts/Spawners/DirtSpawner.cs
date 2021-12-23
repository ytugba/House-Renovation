using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSpawner : MonoBehaviour
{
    [Header("Spawn Area is transparent and called DeformationArea.")]
    public GameObject[] spawnArea;
    public GameObject[] dirtPrefabs;
    public List<GameObject> dirtsCreated;
    public List<GameObject> dirtsCreatedOnScene;

    [HideInInspector]
    public int randomDirtIndex;
    [Tooltip("Quantity will be set according to the difficulity.")]
    public int defaultQuantity;
    private int quantity;

    // Start is called before the first frame update
    void Start()
    {
        dirtsCreated = new List<GameObject>();
        dirtsCreatedOnScene = new List<GameObject>();
        spawnArea = GameObject.FindGameObjectsWithTag("DeformationArea");
        quantity = defaultQuantity;
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < spawnArea.Length; i++)
        {
            int collisions = 0;

            float[] DirtSize = { dirtPrefabs[randomDirtIndex].GetComponent<Renderer>().bounds.size.x,
                             dirtPrefabs[randomDirtIndex].GetComponent<Renderer>().bounds.size.y,
                             dirtPrefabs[randomDirtIndex].GetComponent<Renderer>().bounds.size.z
                           };
            float[] areaSize = { spawnArea[i].GetComponent<Renderer>().bounds.size.x / 2 - DirtSize[0] / 2,
                             spawnArea[i].GetComponent<Renderer>().bounds.size.z / 2 - DirtSize[2] / 2
                           };

            while (quantity > 0 && dirtsCreated.Count < defaultQuantity * spawnArea.Length)
            {
                float x = Random.Range(areaSize[0], -areaSize[0]);
                float z = Random.Range(-areaSize[1], areaSize[1]);
                Vector3 target = new Vector3(x, 0, z);
                target.y = 0.38f;
                bool isCollided = false;

                if (dirtsCreated.Count > 0)
                {
                    foreach (GameObject dirt in dirtsCreated)
                    {
                        if (x + DirtSize[2] > dirt.transform.position.x && x < dirt.transform.position.x || x - DirtSize[2] < dirt.transform.position.x && x > dirt.transform.position.x)
                        {
                            if ((z - DirtSize[2]) < dirt.transform.position.z && z > dirt.transform.position.z)
                            {
                                isCollided = true;
                                collisions = 1;
                            }

                            if ((z + DirtSize[2]) > dirt.transform.position.z && z < dirt.transform.position.z)
                            {
                                isCollided = true;
                                collisions++;
                            }
                        }
                    }
                }

                if (!isCollided)
                {
                    randomDirtIndex = Random.Range(0, dirtPrefabs.Length);
                    var dirt = Instantiate(dirtPrefabs[randomDirtIndex], target + spawnArea[i].transform.position, Quaternion.identity, spawnArea[i].transform);
                    dirt.name = dirt.name.Replace("(Clone)", "");
                    dirt.transform.Rotate(-90, Random.Range(0, 360), dirt.transform.rotation.z);
                    dirt.transform.SetParent(spawnArea[i].transform);
                    quantity--;
                    dirtsCreated.Add(dirt);
                    dirtsCreatedOnScene.Add(dirt);
                }
            }
            quantity = defaultQuantity;
        }
    }
}
