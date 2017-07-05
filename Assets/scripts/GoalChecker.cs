using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Klase, kas glabā informāciju par izpidītajiem uzdevumiem un satur metodes šīs informācijas parādīšanai
public class GoalChecker :MonoBehaviour{
    public static bool CompletedIntro = false;
    public static bool CompletedDesolate = false;
    public static bool CompletedFire = false;
    public static bool Got3Items = false;
    public static bool Blocked5Wolves = false;
    public static bool BlewUp50Barrels = false;
    public static bool Marry12Beauty = false;
    public static bool ReachOld = false;

    public static int blownUpBarrels = 0;
    public static int blockedWolves = 0;
    float waitTime = 0.025f;
    float moveSpeed = 6f;
    public Transform[] heads;
    public Transform column;
    static int completedTasks = 0;
    public GateController gateController;
    void Start()
    {
        Debug.Log("thingWorks");
        UpdateStatus();
        CheckStatus();
        SetEnvironment();
    }

    public static void ClearGoals()
    {
    blownUpBarrels = 0;
    CompletedIntro = false;
    CompletedDesolate = false;
    CompletedFire = false;
    Got3Items = false;
    Blocked5Wolves = false;
    BlewUp50Barrels = false;
    Marry12Beauty = false;
    ReachOld = false;
    }

    static void UpdateStatus()
    {
        try
        {
            if (Variables.playerStats.inventory.Count > 2)
                Got3Items = true;
            else
                Got3Items = false;
        }
        catch
        {

        }
    }
    static void CheckStatus()
    {
        completedTasks = 0;
        if (CompletedIntro)        
            completedTasks++;        
        if(CompletedDesolate)
            completedTasks++;
        if (CompletedFire)
            completedTasks++;
        if (Got3Items)
            completedTasks++;
        if (Blocked5Wolves)
            completedTasks++;
        if (BlewUp50Barrels)
            completedTasks++;
        if (Marry12Beauty)
            completedTasks++;
        if (ReachOld)
            completedTasks++;
        completedTasks = 8;   
    }
    void SetEnvironment()
    {
        if (completedTasks == 8)
        {
          //  gateController.CloseGates();
        }
        StartCoroutine("StartSetup");
        StartCoroutine("MoveColumn");
        
    }

    public static string GetGoalList()
    {
        UpdateStatus();
        CheckStatus();
        string GoalList = "";
        GoalList += "Goals completed: " + completedTasks + "/8\n\n";
        GoalList += '\u2022' + "Complete Styx stage" + Helpers.GetStatusSymbol(CompletedIntro) +"\n";
        GoalList += '\u2022' + "Complete Tartarus stage" + Helpers.GetStatusSymbol(CompletedFire) + "\n";
        GoalList += '\u2022' + "Complete Lethe stage" + Helpers.GetStatusSymbol(CompletedDesolate) + "\n";
        GoalList += '\u2022' + "Have 3 heirlooms in your inventory" + Helpers.GetStatusSymbol(Got3Items) + "\n";
        GoalList += '\u2022' + "Block 5 wolves in one run" + Helpers.GetStatusSymbol(Blocked5Wolves) + "\n";
        GoalList += '\u2022' + "Shoot 50 barrels in total" + Helpers.GetStatusSymbol(BlewUp50Barrels) + " currently: "+ blownUpBarrels+"\n";
        GoalList += '\u2022' + "Marry a gorgeous woman (12 beauty) " + Helpers.GetStatusSymbol(Marry12Beauty) + "\n";
        GoalList += '\u2022' + "Have a hero reach old age " + Helpers.GetStatusSymbol(ReachOld);
        
        return GoalList;
    }


    IEnumerator StartSetup()
    {

        for (int i = 0; i < completedTasks; i++)
        {
            yield return StartCoroutine(MoveHead(heads[i]));
        }
       
    }
    IEnumerator MoveHead(Transform head)
    {
        while (head.position.y >= -2.5f)
        {
            head.Translate(Vector3.forward * -waitTime * moveSpeed);
            yield return new WaitForSeconds(waitTime);
        }
    }
    IEnumerator MoveColumn()
    {
        float downDist = completedTasks * -1.73f;
        float downdist = downDist/8.0f;
        while (column.position.y >= downDist)
        {
            column.Translate(Vector3.forward * -waitTime * moveSpeed*completedTasks/48);
            yield return new WaitForSeconds(waitTime);
        }
    }


}
