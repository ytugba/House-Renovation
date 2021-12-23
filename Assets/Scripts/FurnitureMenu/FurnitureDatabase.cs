using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FurnitureDatabase : MonoBehaviour
{
    [Header("Furniture info's loaded from JSON")]
    public List<Furnitures> furnitures = new List<Furnitures>();
    public int availableFurnitureNumber = 0;

    void Awake()
    {
        LoadDatabase();
    }

    //This class Loads the databaase from relevant file
    void LoadDatabase()
    {
        TextAsset file = Resources.Load<TextAsset>("Databases/FurnitureDatabaseJSON");
        string jsonString = file.text;

        if (jsonString != null)
        {
            Furnitures[] fur = JsonHelperFurniture.FromJson<Furnitures>(jsonString);
            furnitures = new List<Furnitures>((Furnitures[])fur);

            availableFurnitureNumber = furnitures.Count;
        }
    }

    //To load furnitures to UI
    public Furnitures GetFurniture(int furnitureId)
    {
        return furnitures.Find(furniture => furniture.FurnitureId == furnitureId);
    }

    public Furnitures GetFurniture(string furnitureName)
    {
        return furnitures.Find(furniture => furniture.Title == furnitureName);
    }
}

[Serializable]
public static class JsonHelperFurniture
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Furnitures;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Furnitures = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Furnitures = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Furnitures;
    }
}