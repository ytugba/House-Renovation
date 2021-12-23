using Newtonsoft.Json;
using System;

[Serializable]
public class Floorings
{
    public int FlooringId;
    public string MaterialPath;
    public int Price;
    public int Amount;
    public string IconPath;
    //x = Resources.Load<Sprite>("Sprites/Floorings/" + floorings.iconpath); is used for accessing sprite icons

    #region Constructors

    private Floorings()
    {
    }

    [JsonConstructor]
    public Floorings(int id, string materialPath, int price, int amount, string iconPath)
    {
        FlooringId = id;
        MaterialPath = materialPath;
        Amount = amount;
        IconPath = iconPath;
        Price = price;
    }

    [JsonConstructor]
    public Floorings(Floorings paintings)
    {
        this.FlooringId = paintings.FlooringId;
        this.MaterialPath = paintings.MaterialPath;
        this.Amount = paintings.Amount;
        this.IconPath = paintings.IconPath;
        this.Price = paintings.Price;
    }
    #endregion
}
