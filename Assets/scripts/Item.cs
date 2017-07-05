using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase, kas satur spēlētāja savācamo objektu datus un metodes to spēju pielietošanai
public class Item {
    public string itemName;
    public int healthBoost=0;
    public int dexterityBoost= 0;
    public int beautyBoost= 0;
    public Sprite itemPicture;
    // Use this for initialization
    public Item()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                itemName = "Boots";
                itemPicture = Variables.boots[Random.Range(0, Variables.boots.Length - 1)];
                break;
            case 1:
                itemName =  "Shirt";
                itemPicture = Variables.shirts[Random.Range(0, Variables.shirts.Length - 1)];
                break;
            case 2:
                itemName = "Ring";
                itemPicture = Variables.rings[Random.Range(0, Variables.rings.Length - 1)];
                break;
            case 3:
                itemName = "Necklace";
                itemPicture = Variables.amulets[Random.Range(0, Variables.amulets.Length - 1)];
                break;
        }


        switch (Random.Range(0, 3))
        {
            case 0:
                healthBoost = Random.Range(1, 3);
                itemName += " of fortitude";
                break;
            case 1:
                dexterityBoost = Random.Range(2, 8) * 10;
                itemName += " of the nimble";
                break;
            case 2:
                beautyBoost = Random.Range(2, 5);
                itemName += " of radiance";
                break;
            
        }
    }
    public void applyItem()
    {
        Variables.playerStats.beauty += beautyBoost;
        Variables.turnSpeed += dexterityBoost;
        Variables.health += healthBoost;
    }
    public void unapplyItem()
    {
        Variables.playerStats.beauty -= beautyBoost;
        Variables.turnSpeed -= dexterityBoost;
        Variables.health -= healthBoost;
    }
    public string createDescription()
    {
        string desc = "";
        if (healthBoost > 0)
        {
            desc += '\u2022'+"Health +" +healthBoost +"\n";
        }
        if(beautyBoost > 0)
        {
            desc += '\u2022'+"Beauty +" + beautyBoost + "\n";
        }
        if (dexterityBoost > 0)
        {
            desc += '\u2022' + "Maneuverability +" + dexterityBoost + "\n";
        }
        return desc;
    }
}
