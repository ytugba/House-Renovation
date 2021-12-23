using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class MissionGenerator : MonoBehaviour
{ //This class is on mission display panel

    #region missioninitialization
    public int missionID { set; get; }
    public string missionName { set; get; }
    public string missionDescription { set; get; }
    public int missionReward { set; get; }
    public bool isCompleted { set; get; }
    private Text debug;
    private List<MissionGenerator> missions;

    public MissionGenerator(int id, string name, string description, int reward, bool isCompleted)
    {
        this.missionID = id;
        this.missionName = name;
        this.missionDescription = description;
        this.missionReward = reward;
        this.isCompleted = isCompleted;
    }
    #endregion
    
    #region variables
    public GameObject missionListPanel;
    public GameObject missionButtonPrefab;
    public List<GameObject> missionButtonList;
    private Queue<MissionGenerator> missionQueue;
    IOrderedEnumerable<MissionGenerator> shuffledMissions;

    public GameObject missionLoadingPanel;
    public GameObject tablet;
    #endregion

    private void Awake()
    {
        missionLoadingPanel.SetActive(false);
        missions = new List<MissionGenerator>();
        missionButtonList = new List<GameObject>();
        missionQueue = new Queue<MissionGenerator>();
        debug = GameObject.Find("debug").GetComponent<Text>();
        AddMissions();
        shuffledMissions = missions.OrderBy(item => System.Guid.NewGuid());
        foreach (var mission in shuffledMissions)
        {
            missionQueue.Enqueue(mission);
        }
        GenerateMissionButtons();
    }

    private void AddMissions()
    {
        missions.Add(new MissionGenerator(0, "Beginner Cleaning", "Owners of the house are coming back from holiday. They want the house clean.", 250, false));
        missions.Add(new MissionGenerator(1, "Intermediate Cleaning", "An unpleasant house for a newly-married couple.", 500, false));
        missions.Add(new MissionGenerator(2, "Advanced Cleaning", "Bachelor battlefield. Time to gather the loot!", 1000, false));
        missions.Add(new MissionGenerator(3, "House Design 101", "They want a new living room. Nothing more.", 300, false));
    }
    private void GenerateMissionButtons()
    {
        for(int i = 0; i < 3;  i++)
        { 
            MissionGenerator mission = missionQueue.Dequeue();
            GameObject missionButton = Instantiate(missionButtonPrefab);

            missionButton.name = missionButton.name.Replace("(Clone)", "");
            missionButton.transform.SetParent(missionListPanel.transform, false);
            Button tempButton = missionButton.GetComponent<Button>();

            missionButton.transform.Find("MissionName").GetComponent<TextMeshProUGUI>().text = mission.missionName;
            missionButton.transform.Find("MissionDescription").GetComponent<TextMeshProUGUI>().text = mission.missionDescription;
            missionButton.transform.Find("MissionReward").GetComponent<TextMeshProUGUI>().text = mission.missionReward.ToString();

            tempButton.onClick.AddListener(() => OnMissionClick(missionButton, mission, missionQueue));
            missionButtonList.Add(missionButton);
        }
    }

    private void OnMissionClick(GameObject missionButton, MissionGenerator mission, Queue<MissionGenerator> missionQueue)
    {
        MissionComplete.missionName = missionButton.transform.Find("MissionName").GetComponent<TextMeshProUGUI>().text;
        MissionComplete.expectedReward = int.Parse(missionButton.transform.Find("MissionReward").GetComponent<TextMeshProUGUI>().text);

        missionButton.transform.Find("MissionDescription").GetComponent<TextMeshProUGUI>().text = "Mission is loading...";
        missionButton.transform.Find("MissionReward").GetComponent<TextMeshProUGUI>().text = "";

        missionQueue.Enqueue(mission);
        StartCoroutine(LoadAscnchronously("MissionScene"));
    }

    private void Update()
    {
        if(MissionComplete.isCompleted)
        {
            MissionComplete.isCompleted = false;
            PlayerHoldings.currentPlayerBudget += MissionComplete.expectedReward;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHoldings>().UpdateBudget();
            MissionComplete.expectedReward = 0;
        }
    }

    IEnumerator LoadAscnchronously(string scene)
    {
        missionLoadingPanel.SetActive(true);
        tablet.SetActive(false);

        yield return new WaitForSeconds(3);
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
    }
}
