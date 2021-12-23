using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInformation : MonoBehaviour
{
    public bool isPaintable;
    public bool hasPainted;
    void Update()
    {
        foreach(Transform child in gameObject.transform)
        {
            if(child.tag == "Crack")
            {
                isPaintable = false;
                hasPainted = false;
            }
        }
    }
}