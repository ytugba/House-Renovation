using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHoldings : MonoBehaviour
{   [Header("Player's budget can change later")]
    public static int currentPlayerBudget = 1000;

    [Header ("Garbage Amount that we can hold")]
    public GameObject garbageBar;
    public float currentGarbage;
    public float maxGarbage;

    [Header("Paint Amount")]
    public GameObject paintBar;
    public float currentPaint;
    public float maxPaint;

    [Header("Floor Amount")]
    public GameObject floorBar;
    public float currentFloor;
    public float maxFloor;

    [Header("Plaster Amount")]
    public GameObject plasterBar;
    public float currentPlaster;
    public float maxPlaster;

    private float garbageBarZeroPos;                 //this is the position that bar moves for display "no garbage"
    private float paintBarZeroPos;
    private float floorBarZeroPos;
    private float plasterBarZeroPos;

    private GameObject playerBudget;
    private TextMeshProUGUI playerBudgetDisplayer;

    private void Awake()
    {
        playerBudget = GameObject.Find("PlayerBudget");
        playerBudgetDisplayer = playerBudget.GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        garbageBarZeroPos = -garbageBar.GetComponent<RectTransform>().rect.height;          //when bar moves as it's height, bar becomes not showing by mask
        paintBarZeroPos = -paintBar.GetComponent<RectTransform>().rect.height;
        floorBarZeroPos = -floorBar.GetComponent<RectTransform>().rect.height;
        plasterBarZeroPos = -plasterBar.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBars();
        UpdateBudget();
    }

    ///<summary> Update holding bars
    ///</summary>
    public void UpdateBars()
    {
        float garbagePos = ((0 - garbageBarZeroPos)/maxGarbage)*currentGarbage + garbageBarZeroPos;
        garbageBar.transform.localPosition = new Vector2 (0, garbagePos);

        float paintPos = ((0 - paintBarZeroPos) / maxPaint) * currentPaint + paintBarZeroPos;
        paintBar.transform.localPosition = new Vector2(0, paintPos);

        float floorPos = ((0 - floorBarZeroPos) / maxFloor) * currentFloor + floorBarZeroPos;
        floorBar.transform.localPosition = new Vector2(0, floorPos);

        float plasterPos = ((0 - plasterBarZeroPos) / maxPlaster) * currentPlaster + plasterBarZeroPos;
        plasterBar.transform.localPosition = new Vector2(0, plasterPos);
    }

    public void UpdateBudget()
    {
        playerBudgetDisplayer.SetText(currentPlayerBudget.ToString());
    }
}
