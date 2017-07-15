using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Klase, kas satur spēles globālos mainīgos
public static class Variables {
	public enum traitStrenght { recessive, both, dominant };
    public enum dir { x,xNeg,z,zNeg};
    public static character playerStats;
    public static float marrigeDiff = 2.0f;
    public static List<float> deathPositions = new List<float>();
    public static Transform player;
    public static int health = 5;
    public static int fame = 0;
    public static int fameLvl = 0;
    public static int spearCount = 20;
    public static int FireTreshold = 29;
    public static int LonelyTreshold = 30;
    public static float waterLevel = 0.53f;
    public static float NormalTurnSpeed = 225;
    public static float turnSpeed = 225;
    public static float minSpeed = 4f;
    public static float normMaxSpeed = 8.5f;
    public static float maxSpeed = 15f;
    //public static float rotationY = 0;
    public static waterGen WaterGen;
    public static int distance = 0;
    public static float levelLength = 20;
    public static bool[] gatePos = { true,true,true,true };
    public static bool LevelDone = false;
    public static Transform shotPoint;
    public static float PlayerSpeed;
    public static bool hasBarrel = true;
    public static bool hasWolves = true;
    public static bool hasGhosts = false;
    public static bool hasHands = false;
    public static bool hasWildfire = true;
    public static bool hasBarrel2 = true;
    public static int currentArea = 0;

    public static bool shownLevelHelp = false;
    public static bool shownHubHelp = false;
    public static bool shownMarrigeHelp = false;

    public static AudioSource mainAudioSource;
    public static AudioSource mainMusicSource;
    public enum levels { normal,fire,desolate,item};
    public static levels currentLVL = levels.normal;
    public static Vector3 spearTarget;
    public static menuScript DeathMenu;
    public static List<character> children = new List<character>();
    public static Sprite[] rings;
    public static Sprite[] amulets;
    public static Sprite[] boots;
    public static Sprite[] shirts;
    public static GameObject finishDialog;
    public static GameObject GotItemMenu;
    public static string[] maleNames = new string[] {"Teiresias","Erechtheus","Erginus","Hippon","Ephialtes","Amphidamos","Archesilaus","Pheronactus",
"Nausithous","Meliboeus","Brygos","Rhadamanthos","Parmenides","Deiphonous","Clisthenes","Deon","Aetolos","Megistias","Zenodoros","Alcandros",
    "Hieronymus","Peolpidas","Simo","Helgesippos","Alcandros","Agetos","Callimorphus","Socus","Labotas","Panionos","Medios","Pidytes","Herakleitos",
"Icarius","Dieneces","Koinos","Euryleon","Peleus","Astrabacus","Tiro"};
    public static string[] femaleNames = new string[] { "Philea", "Clio", "Roxane", "Euodias", "Zoe", "Ismene", "Anteia", "Hecuba", "Polymede", "Antheia",
        "Caleope","Antiope","Rhodope","Leucothoë","Kynthia","Epicaste","Cilissa","Lampetie","Kydilla","Thetis"};

}

//Datu klase, kas satur spēles stāvokļa saglabāšanai nepieciešamos datus
[Serializable]
public class SaveData
{

    //From variables
    public character playerStats;
    public List<float> deathPositions;
    public int health;
    public int fame;
    public float turnSpeed;
    public bool shownLevelHelp;
    public bool shownHubHelp;
    public bool shownMarrigeHelp;

    //From goalChecker
    public bool CompletedIntro;
    public bool CompletedDesolate;
    public bool CompletedFire;
    public bool Got3Items;
    public bool Blocked5Wolves;
    public bool BlewUp50Barrels;
    public bool Marry12Beauty;
    public bool ReachOld;
    public int blownUpBarrels;
    public int blockedWolves;
    

    public SaveData()
    {
        playerStats = Variables.playerStats;
        deathPositions = Variables.deathPositions;
        health = Variables.health;
        turnSpeed = Variables.turnSpeed;
        shownLevelHelp = Variables.shownLevelHelp;
        shownHubHelp = Variables.shownHubHelp;
        shownMarrigeHelp = Variables.shownMarrigeHelp;

        CompletedIntro = GoalChecker.CompletedIntro;
        CompletedDesolate = GoalChecker.CompletedDesolate;
        CompletedFire = GoalChecker.CompletedFire;
        Got3Items = GoalChecker.Got3Items;
        Blocked5Wolves = GoalChecker.Blocked5Wolves;
        Marry12Beauty = GoalChecker.Marry12Beauty;
        ReachOld = GoalChecker.ReachOld;
        blownUpBarrels = GoalChecker.blownUpBarrels;
        blockedWolves = GoalChecker.blockedWolves;



    }




}
