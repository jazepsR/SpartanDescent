using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Klase, kas satur informāciju par ģenētiskajā iezīmēm, kas piemīt tēlam un ģenerē tās.
[Serializable]
public class Trait  {    
    public string name;
    public string posName;
    public string negName;
    public Variables.traitStrenght negTraitType;
    public Variables.traitStrenght posTraitType;
	public Trait(string posName,string negName, Variables.traitStrenght posTraitType, Variables.traitStrenght negTraitType)
	{
        this.posName = posName;
        this.negName = negName;
        this.posTraitType = posTraitType;
        this.negTraitType = negTraitType;
        name = findName();     
        
	}

    public Trait genTrait(Trait firstTrait,Trait secondTrait)
    {
        float rand = UnityEngine.Random.Range(0.0f, 1f);
        Variables.traitStrenght tempPosTrait;
        Variables.traitStrenght tempNegTrait;
        if (rand > 0.5f)
            tempPosTrait = firstTrait.posTraitType;
        else
            tempPosTrait = secondTrait.posTraitType;
        rand = UnityEngine.Random.Range(0.0f, 1.0f);
        if (rand > 0.5f)
            tempNegTrait = firstTrait.negTraitType;        
        else
            tempNegTrait = secondTrait.negTraitType;

        return new Trait(firstTrait.posName, firstTrait.negName, tempPosTrait, tempNegTrait);

    }
	private string findName()
    {
        string newName = "none";
        if (posTraitType == negTraitType || ((int)posTraitType == 1 && (int)negTraitType == 2) || ((int)posTraitType == 2 && (int)negTraitType == 1))
        {
            newName = "none";
        }
        else
        {
            if (posTraitType == Variables.traitStrenght.recessive)
                newName = negName;
            if (negTraitType == Variables.traitStrenght.recessive)
                newName = posName;
        }
        return newName;
    }
}
