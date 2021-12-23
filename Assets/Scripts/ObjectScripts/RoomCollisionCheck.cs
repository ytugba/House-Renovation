using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollisionCheck : MonoBehaviour
{
    public GameObject room;
    public bool inARoom;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            room = gameObject;
            inARoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            room = null;
            inARoom = false;
        }
    }
}
