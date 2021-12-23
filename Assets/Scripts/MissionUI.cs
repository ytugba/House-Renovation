using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    MissionGenerator missionGenerator;

    private void Awake()
    {
        missionGenerator = GameObject.Find("GameManager").GetComponent<MissionGenerator>();
    }
    public void OnClick()
    {
        
    }
}
