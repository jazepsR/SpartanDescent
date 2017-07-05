using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

//Statiska klase, kas satur dažādas palīgmetodes, ko izmanto citas klases
public class Helpers : MonoBehaviour {

	 //This returns the angle in radians
 public static float AngleInRad(Vector3 vec1, Vector3 vec2) {
     return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
 }
 public static void ShowGUIText(string text,float duration)
    {
        GameObject textObj = Resources.Load("prefabs/GUIText") as GameObject;
        textObj.GetComponentInChildren<Text>().text = text;
        GameObject InstantiatedObj = Instantiate(textObj, GameObject.Find("Canvas").transform,false);
        Destroy(InstantiatedObj, duration);

    }

    public static void PlayButtonSound()
    {
        AudioClip btn2 = Resources.Load("sounds/button2") as AudioClip;
        Variables.mainAudioSource.PlayOneShot(btn2);
    }
 //This returns the angle in degrees
     public static float AngleInDeg(Vector3 vec1, Vector3 vec2) {
         return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
     }
    public static bool RandomBool()
    {
        float randFloat = UnityEngine.Random.Range(0.0f, 1.0f);
        bool random = (randFloat > 0.5f);
        return random;
    }

    

    public static string GetStatusSymbol(bool status)
    {
        if (status)
        {
            return " "+'\u2713';
        }else
        {
            return " " + '\u2717';
        }
    }


    public static Vector2 CalculatePos(Vector2 currentPos, float rot)
    {
        switch ((int)Mathf.Abs(rot % 360))
        {
            case 0:
                currentPos = new Vector2(currentPos.x + 1, currentPos.y);
                break;
            case 90:
                currentPos = new Vector2(currentPos.x, currentPos.y + 1);
                break;
            case 180:
                currentPos = new Vector2(currentPos.x - 1, currentPos.y);
                break;
            case 270:
                currentPos = new Vector2(currentPos.x, currentPos.y - 1);
                break;
        }
        return currentPos;
    }
    public static string GetName(bool isMale)
    {
        if (isMale)
        {
            return Variables.maleNames[UnityEngine.Random.Range(0, Variables.maleNames.Length - 1)];
        }
        else
        {
            return Variables.femaleNames[UnityEngine.Random.Range(0, Variables.femaleNames.Length - 1)];
        }
    }

    public static Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
        float t = Mathf.Sqrt(timeToTarget);
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }
}

//Klase, kas kontrolē spēles stāvokļa saglabāšanu starp sesijām un šīs informācijas ielādēšanu
public class SaveLoad{
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.dat");

        SaveData saveData = new SaveData();
        bf.Serialize(file, saveData);
        file.Close();
    }
    public static bool Load()
    {
        if(File.Exists(Application.persistentDataPath+ "/saveGame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            ApplyLoadedFile(data);
            return true;
        }else
        {
            return false;
        }
    }
    private static void ApplyLoadedFile(SaveData data)
    {
        Variables.playerStats = data.playerStats;
        Variables.deathPositions = data.deathPositions;
        Variables.health = data.health;
        Variables.turnSpeed = data.turnSpeed;
        Variables.shownLevelHelp = data.shownLevelHelp;
        Variables.shownHubHelp = data.shownHubHelp;
        Variables.shownMarrigeHelp = data.shownMarrigeHelp;

        GoalChecker.CompletedIntro = data.CompletedIntro;
        GoalChecker.CompletedDesolate = data.CompletedDesolate;
        GoalChecker.CompletedFire = data.CompletedFire;
        GoalChecker.Got3Items = data.Got3Items;
        GoalChecker.Blocked5Wolves = data.Blocked5Wolves;
        GoalChecker.Marry12Beauty = data.Marry12Beauty;
        GoalChecker.ReachOld = data.ReachOld;
        GoalChecker.blownUpBarrels = data.blownUpBarrels;
        GoalChecker.blockedWolves = data.blockedWolves;
    }
}