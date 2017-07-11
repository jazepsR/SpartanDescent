using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase, kas iestata globalos mainīgos atbilstoši izvēlētā līmeņa parametriem
public class LevelSetup : MonoBehaviour {
	public Variables.levels currentLVL;
	// Use this for initialization
	void Awake () {
		Variables.spearCount = 12;
		Variables.waterLevel = 0.53f;
		if (Variables.currentLVL != Variables.levels.item)
		{
			Variables.currentLVL = currentLVL;
		}
        SetLVL(Variables.currentLVL);
		
		

	}
	
	public static void SetLVL(Variables.levels level)
    {
        switch (level)
        {
            case Variables.levels.normal:
                Variables.hasBarrel = true;
                Variables.hasWolves = false;
                Variables.hasGhosts = false;
                Variables.hasHands = false;
                Variables.hasWildfire = false;
                Variables.levelLength = 250;
                if (Variables.shownLevelHelp)
                    Helpers.ShowGUIText("Entering the river Styx", 3.5f);
                break;
            case Variables.levels.fire:
                Variables.hasBarrel = true;
                Variables.hasWolves = false;
                Variables.hasGhosts = true;
                Variables.hasHands = false;
                Variables.hasWildfire = true;
                Variables.levelLength = 35;
                Helpers.ShowGUIText("Entering the river Tartarus", 3.5f);
                break;
            case Variables.levels.desolate:
                Variables.hasBarrel = false;
                Variables.hasWolves = true;
                Variables.hasGhosts = true;
                Variables.hasHands = true;
                Variables.hasWildfire = true;
                Variables.levelLength = 45;
                Helpers.ShowGUIText("Entering the river Lethe", 3.5f);
                break;
            case Variables.levels.item:
                Variables.hasBarrel = Helpers.RandomBool();
                Variables.hasBarrel = true;
                Variables.hasGhosts = Helpers.RandomBool();
                Variables.hasWolves = Helpers.RandomBool();
                Variables.hasHands = Helpers.RandomBool();
                Variables.levelLength = 20f;
                Debug.Log("Barrels: " + Variables.hasBarrel + " Ghosts: " + Variables.hasGhosts + " Wolves: " + Variables.hasWolves + " Hands: " + Variables.hasHands);
                Helpers.ShowGUIText("Entering the river Acheron", 3.5f);
                break;
        }
    }
}
