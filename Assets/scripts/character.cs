using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


//Klase glabā varoņa datus un satur metodes to apstrādei
[Serializable]
public class character 
{
    public string name;
    public int middleAge;
    public int oldAge;
    public int deathAge;
    public List<Trait> charTraits = new List<Trait>();
    private string[] goodTraits = new string[] { "Strong", "Bountiful", "Dexterious", "Tough", "Youthful", "Beautiful" };
    private string[] badTraits = new string[] { "Frail", "Barren", "Clumsy", "Brittle", "Old before their time", "Unsightly" };
    private int[] traitRandomSeed = new int[] { 0, 1, 1, 2 };
    public List<string> activeTraits = new List<string>();
    public int beauty;
    public List<Item> inventory = new List<Item>();
    public int extraChildren = 0;
    public int age = 20;
    private int traitTier;
    public ageStage currentStage = ageStage.adulthood;
    public enum ageStage {adulthood,middleAge, oldAge, dead };     
	public character(string name)
	{
		this.name = name;        
        GenTraits();
        SetActiveTraits();        
		SetBeauty(false);        
        SetAges();
       // InvokeRepeating("AgeChar", 2.0f, 2.0f);
        // PrintTraits();
    }

       
    

 

    public character(character parent1, character parent2,string name)
	{
		this.name = name;
		for(int i = 0; i < parent1.charTraits.Count; i++)
		{
		   charTraits.Add( parent1.charTraits[i].genTrait(parent1.charTraits[i], parent2.charTraits[i]));
		}       
		SetActiveTraits();
        int parentBeauty = (parent1.beauty + parent2.beauty) / 2 + UnityEngine.Random.Range(-2, 2);
        SetBeauty(true,parentBeauty);
		SetAges();
       // PrintTraits();
    }
   
    public ageStage SetAging()
    {
        
        if (age > deathAge)
        {
            //Add death stuff
            currentStage = ageStage.dead;
            Variables.health = 0;
            return ageStage.dead;
        }else
        {
            if (age > oldAge)
            {
                currentStage = ageStage.oldAge;
                if (GoalChecker.ReachOld == false)
                {
                    GoalChecker.ReachOld = true;
                    Helpers.ShowGUIText("Goal Completed", 3.5f);
                }
                return ageStage.oldAge;
            }else
            {
                if (age > middleAge)
                {
                    currentStage = ageStage.middleAge;
                    return ageStage.middleAge;

                }else
                {
                    currentStage = ageStage.adulthood;
                    return ageStage.adulthood;
                }
            }
        }
    }

	void GenTraits()
	{
		
		for (int i = 0; i < badTraits.Length; i++)
		{
			charTraits.Add(new Trait(goodTraits[i], badTraits[i], (Variables.traitStrenght)traitRandomSeed[UnityEngine.Random.Range(0, traitRandomSeed.Length)],
				(Variables.traitStrenght)traitRandomSeed[UnityEngine.Random.Range(0, traitRandomSeed.Length)]));
		   
		}
		
	}
    void SetBeauty(bool hasPatents,int parentBeauty = 0)
	{      
        beauty = 0;
		if (activeTraits.Contains("Beautiful"))
			beauty += 5;
		if (activeTraits.Contains("Unsightly"))
			beauty = Mathf.Max(0, beauty - 5);
        if (hasPatents)
        {
            beauty = Mathf.Max(0, parentBeauty + traitTier * 3);
        }
        else
        {
            beauty = Mathf.Max(0, beauty + UnityEngine.Random.Range(4, 9) + traitTier * 3);
        }
        
    }
	void SetAges()
	{
		middleAge = UnityEngine.Random.Range(40,47);
		oldAge = UnityEngine.Random.Range(60, 70);
		deathAge = UnityEngine.Random.Range(77, 90);

	}
	void SetActiveTraits()
	{
		foreach(Trait tr in charTraits)
		{
            
			if (tr.name != "none")
			{
				activeTraits.Add(tr.name);
                
                if (goodTraits.Contains<string>(tr.name))
                {
                    traitTier++;
                }
                if (goodTraits.Contains<string>(tr.name))
                {
                    traitTier--;
                }
            }
		}
	}
	

	public void PrintTraits()
	{
		Debug.Log("Traits for " + name);
		foreach (Trait tr in charTraits)
		{

			if (tr.name != "none")
			{
				Debug.Log(tr.name);
			}

		}
        Debug.Log("Trait tier: " + traitTier.ToString());
        Debug.Log("Beauty: "+ beauty.ToString());
       
	}
    public string PrintBio()
    {
        string bio = name;
        bio += "\nBeauty: " + beauty.ToString() + "\n";
        foreach (string activeTrait in activeTraits)
        {
            bio += '\u2022' + " " + activeTrait + "\n";
        }
        return bio;
    }
    public string PrintStats()
    {
        string bio = name;
        bio += "\n\nAge: " + age.ToString() + ", " + currentStage.ToString() + "\n";
        bio += "\nBeauty: " + beauty.ToString() + "\n";
        foreach (string activeTrait in activeTraits)
        {
            bio += '\u2022' + " " + activeTrait + "\n";
        }
        return bio;
    }

}
