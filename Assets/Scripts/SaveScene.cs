using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class SaveScene : MonoBehaviour
{
    /*public PlayerFurnitureManipulation playerFurnitureManipulation;
    private Furnitures[] createdFurnituresArray;
    public GameObject objectPositionTest;
    public float[] objectPosition = new float[3];
    

    public void SaveData()
    {
        if (playerFurnitureManipulation.furnituresCreated != null)
        {
            objectPosition = new float[] { objectPositionTest.transform.position.x, objectPositionTest.transform.position.y, objectPositionTest.transform.position.z };
            //SERIALIZED JSON FILE WILL BE SENT TO THE DIFFERENT SAVELOGS
            //TO BE ABLE TO SAVE GAMEOBJECTS THAT HAS BEEN CREATED IN THE SCENE A OBJECTSINSCENE CLASS NEEDS TO BE CREATED
            //THAT CLASS WILL HOLD A SERIALIZABLE POSITION, ROTATION, SCALE INTEGERS; MATERIAL, PREFAB AND COMPONENT PATHS
            //createdFurnituresArray = playerFurnitureManipulation.furnituresCreated.ToArray();
            //string savedJson = JsonHelper.ToJson(createdFurnituresArray, true);
            string savedJson = JsonHelper.ToJson(objectPosition, true);
            File.WriteAllText(Application.dataPath + "/Resources/SavedData.json", savedJson);

        }
    }
    

    public void LoadData()
    {
        //Loading saved data
        TextAsset file = Resources.Load<TextAsset>("SavedData");
        string jsonString = file.text;

        if (jsonString != null)
        {
            Vector3 position;
            objectPosition = JsonHelper.FromJson<float>(jsonString);
            position.x = (float)(Math.Round((double)objectPosition[0], 15));
            position.y = (float)(Math.Round((double)objectPosition[1], 15));
            position.z = (float)(Math.Round((double)objectPosition[2], 15));

            objectPositionTest.transform.position = position;
            //createdFurnituresArray = JsonHelper.FromJson<Furnitures>(jsonString);
            //playerFurnitureManipulation.furnituresCreated = new List<Furnitures>((Furnitures[])createdFurnituresArray);
        }
    }*/
}

