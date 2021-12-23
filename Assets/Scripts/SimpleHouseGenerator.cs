using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHouseGenerator : MonoBehaviour
{
    public GameObject standartWallPrefab;
    public GameObject playArea;

    // Start is called before the first frame update
    void Start()
    {
        CreateSimpleHome1();
    }

    ///<summary> Create 4 wall house
    ///</summary>
    public void CreateSimpleHome1()
    {
        for(int i = -5; i < 5; i++)
        {
            if(i != 0 &&  i != -1 && i != 1){
                GameObject newWall = Instantiate(standartWallPrefab, new Vector3(i, 0, -5f), Quaternion.identity);
                newWall.transform.SetParent(playArea.transform);
            }
        }

        for(int i = -5; i < 5; i++)
        {
            GameObject newWall = Instantiate(standartWallPrefab, new Vector3(i, 0, 5f), Quaternion.identity);
            newWall.transform.SetParent(playArea.transform);
        }

        for(int i = -5; i < 5; i++)
        {
            GameObject newWall = Instantiate(standartWallPrefab, new Vector3(-5.5f, 0, i + 0.5f), Quaternion.identity);
            newWall.transform.Rotate(0, 90, 0, Space.Self);
            newWall.transform.SetParent(playArea.transform);
        }

        for(int i = -5; i < 5; i++)
        {
            GameObject newWall = Instantiate(standartWallPrefab, new Vector3(4.5f, 0, i+ 0.5f), Quaternion.identity);
            newWall.transform.Rotate(0, 90, 0, Space.Self);
            newWall.transform.SetParent(playArea.transform);
        }
    }
        
    
}
