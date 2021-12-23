using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PaintingDatabase : MonoBehaviour
{
    [Header("Painting info's loaded from JSON")]
    public List<Paintings> paintings = new List<Paintings>();
    public int availablePaintingNumber = 0;

    void Awake()
    {
        LoadDatabase();
    }

    //This class Loads the databaase from relevant file
    void LoadDatabase()
    {
        TextAsset file = Resources.Load<TextAsset>("Databases/PaintingDatabaseJSON");
        string jsonString = file.text;

        if (jsonString != null)
        {
            Paintings[] fur = JsonHelperPaint.FromJson<Paintings>(jsonString);

            paintings = new List<Paintings>((Paintings[])fur);
            availablePaintingNumber = paintings.Count;
        }
    }

    //To load furnitures to UI
    public Paintings GetPainting(int paintingID)
    {
        return paintings.Find(painting => painting.PaintingId == paintingID);
    }
}

[Serializable]
public static class JsonHelperPaint
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Paintings;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Paintings = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Paintings = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Paintings;
    }
}