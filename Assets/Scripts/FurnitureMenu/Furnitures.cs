using System;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Furnitures
{
    public int FurnitureId;
    public string Title;
    public string Description;
    public string IconPath;
    public string PrefabPath;
    public string Category;
    public int Price;
    //x = Resources.Load<Sprite>("Sprites/Furnitures/" + furniture.Iconpath); is used for accessing sprite icons

    #region Constructors

    private Furnitures()
    {
    }

    [JsonConstructor]
    public Furnitures(int id, string title, string description, string icon, string prefab, string category, int price)
    {
        FurnitureId = id;
        Title = title;
        description = Description;
        IconPath = icon;
        PrefabPath = prefab;
        Category = category;
        Price = price;
    }

    [JsonConstructor]
    public Furnitures(Furnitures furnitures)
    {
        this.FurnitureId = furnitures.FurnitureId;
        this.Title = furnitures.Title;
        this.Description = furnitures.Description;
        this.IconPath = furnitures.IconPath;
        this.PrefabPath = furnitures.PrefabPath;
        this.Category = furnitures.Category;
        this.Price = furnitures.Price;
    }
    #endregion
}