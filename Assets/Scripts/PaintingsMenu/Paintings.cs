using System;
using Newtonsoft.Json;

[Serializable]
public class Paintings
{
    public int PaintingId;
    public string MaterialPath;
    public int Price;
    public int Amount;
    public string IconPath;
    //x = Resources.Load<Sprite>("Sprites/Paintings/" + paintings.iconpath); is used for accessing sprite icons

    #region Constructors

    private Paintings()
    {
    }

    [JsonConstructor]
    public Paintings(int id, string materialPath, int price, int amount, string iconPath)
    {
        PaintingId = id;
        MaterialPath = materialPath;
        Amount = amount;
        IconPath = iconPath;
        Price = price;
    }

    [JsonConstructor]
    public Paintings(Paintings paintings)
    {
        this.PaintingId = paintings.PaintingId;
        this.MaterialPath = paintings.MaterialPath;
        this.Amount = paintings.Amount;
        this.IconPath = paintings.IconPath;
        this.Price = paintings.Price;
    }
    #endregion
}