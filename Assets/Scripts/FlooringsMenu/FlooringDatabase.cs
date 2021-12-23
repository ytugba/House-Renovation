using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FlooringDatabase : MonoBehaviour
{
    [Header("Flooring info's loaded from JSON")]
    public List<Floorings> floorings = new List<Floorings>();
    public int availableFlooringNumber = 0;

    void Awake()
    {
        LoadDatabase();
    }

    //This class Loads the databaase from relevant file
    void LoadDatabase()
    {
        TextAsset file = Resources.Load<TextAsset>("Databases/FlooringDatabaseJSON");
        string jsonString = file.text;

        if (jsonString != null)
        {
            Floorings[] fur = JsonHelperFloor.FromJson<Floorings>(jsonString);

            floorings = new List<Floorings>((Floorings[])fur);
            availableFlooringNumber = floorings.Count;
        }
    }

    //To load furnitures to UI
    public Floorings GetFlooring(int flooringId)
    {
        return floorings.Find(flooring => flooring.FlooringId == flooringId);
    }
}

[Serializable]
public static class JsonHelperFloor
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Floorings;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Floorings = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Floorings = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Floorings;
    }
}