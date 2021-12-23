using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectInformation
{
    //This Class will be on every object to keep their positions, materials etc.
    //What else should be added on the list [DISCUSS]

    public long ID; //Each object in the game will have a spesific id

    public int positionx;
    public int positiony;
    public int positionz;
    public int rotationx;
    public int rotationy;
    public int rotationz;
    public int scalex;
    public int scaley;
    public int scalez;

    public string name;
    public string materialPath; //materials should be edited in a proper folder
    public string prefabPath; //for colliders, scalers etc. a prefab can be used
    public string[] componentName;
}
