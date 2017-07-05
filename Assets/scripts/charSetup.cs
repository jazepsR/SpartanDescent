using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase izveido "character" klases objektus balstoties uz vecāku parametriem atbilstoši ģenētiskajiem algoritmiem
public class charSetup : MonoBehaviour {

    // Use this for initialization
    private string[] goodTraits = new string[] { "Strong", "Bountiful", "Dexterious", "Tough", "Youthful", "Beautiful" };
    private string[] badTraits = new string[] { "Frail", "Barren", "Clumsy", "Brittle", "Old before their time", "Unsightly" };
 
    void Awake () {
        if(Variables.playerStats == null) 
            Variables.playerStats = new character(Helpers.GetName(true));
        SetupChar();
        Debug.Log( Application.persistentDataPath);
	}
	public void SetupChar()
    {
        Variables.health = 5;
        Debug.Log("Did setupChar for " + Variables.playerStats.name);
        List<string> traits = Variables.playerStats.activeTraits;
        //Beautiful and Unsightly traits applied in "character" class
        float bonusTurnSpeed = 0;

        if (traits.Contains("Strong"))
        {
            //TODO: implement damage
        }
        if (traits.Contains("Frail"))
        {
            //TODO: implement damage
        }
        if (traits.Contains("Bountiful"))
        {
            //Stat applied at child creation
        }
        if (traits.Contains("Barren"))
        {
            //Stat applied at child creation
        }
        if (traits.Contains("Dexterious"))
        {
            bonusTurnSpeed = 75;
        }
        if (traits.Contains("Tough"))
        {
            Variables.health += 2;
        }
        if (traits.Contains("Brittle"))
        {
            Variables.health -= 2;
        }
        if (traits.Contains("Clumsy"))
        {
            bonusTurnSpeed = 50;
        }
        if (traits.Contains("Youthful"))
        {
            Variables.playerStats.middleAge += 5;
            Variables.playerStats.oldAge += 7;
        }
        if (traits.Contains("Old before their time"))
        {
            Variables.playerStats.middleAge -= 5;
            Variables.playerStats.oldAge -= 5;
        }
        Variables.turnSpeed = Variables.NormalTurnSpeed + bonusTurnSpeed;
       
    }
	// Update is called once per frame
	void Update () {
		
	}
}
