using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRandomGarbage : MonoBehaviour
{
    public GameObject[] spawnArea;
    public GameObject[] garbagePrefabs;
    public int randomGarbageIndex;
    public int defaultQuantity;
    private int quantity;
    public List<GameObject> garbagesCreated;
    public List<GameObject> garbagesCreatedOnScene;
    // Start is called before the first frame update
    void Start()
    {
        garbagesCreated = new List<GameObject>();
        garbagesCreatedOnScene = new List<GameObject>();
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

            float[] garbageSize = { garbagePrefabs[randomGarbageIndex].GetComponent<Renderer>().bounds.size.x,
                        garbagePrefabs[randomGarbageIndex].GetComponent<Renderer>().bounds.size.y,
                        garbagePrefabs[randomGarbageIndex].GetComponent<Renderer>().bounds.size.z
                        };
            float[] areaSize = { spawnArea[i].GetComponent<Renderer>().bounds.size.x / 2 - garbageSize[0] / 2,
                        spawnArea[i].GetComponent<Renderer>().bounds.size.z / 2 - garbageSize[2] / 2 };

            while (quantity > 0 && garbagesCreated.Count < defaultQuantity * spawnArea.Length)
            {
                float x = Random.Range(areaSize[0], -areaSize[0]);
                float z = Random.Range(-areaSize[1], areaSize[1]);
                Vector3 target = new Vector3(x, 1f, z);
                //target.y = 1f;
                Vector3 garbageScale = new Vector3(garbageSize[0], garbageSize[1], garbageSize[2]);
                bool isCollided = false;

                if (garbagesCreated.Count > 0)
                {
                    foreach (GameObject garbage in garbagesCreated)
                    {
                        if (x + garbageSize[2] > garbage.transform.position.x && x < garbage.transform.position.x || x - garbageSize[2] < garbage.transform.position.x && x > garbage.transform.position.x)
                        {
                            if ((z - garbageSize[2]) < garbage.transform.position.z && z > garbage.transform.position.z)
                            {
                                isCollided = true;
                                collisions = 1;
                            }

                            if ((z + garbageSize[2]) > garbage.transform.position.z && z < garbage.transform.position.z)
                            {
                                isCollided = true;
                                collisions++;
                            }
                        }
                    }
                }

                if (!isCollided)
                {
                    randomGarbageIndex = Random.Range(0, garbagePrefabs.Length);
                    var garbage = Instantiate(garbagePrefabs[randomGarbageIndex], target + spawnArea[i].transform.position, Quaternion.identity);
                    garbage.name = garbage.name.Replace("(Clone)", "");
                    garbage.transform.Rotate(garbage.transform.rotation.x, Random.Range(0, 360), garbage.transform.rotation.z);
                    garbage.transform.SetParent(spawnArea[i].transform);
                    quantity--;
                    garbagesCreated.Add(garbage);
                    garbagesCreatedOnScene.Add(garbage);
                }
            }
            quantity = defaultQuantity;
        }
     }
}
