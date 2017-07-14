using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Klase, kas parāda spēlētāja dzīvības vizuālajā saskarnē
public class guiControl : MonoBehaviour
{
    
    public static int health = 5;
    public static int fame = 0;
    private int prevHp = 5;
    public static int score = 0;
    public Text healthTxt;    
    public AudioClip deathSound;
    public AudioClip painSound;   
    void Update()
    {

        try
        {
           healthTxt.text = "Health: " + Variables.health.ToString();
        }
        catch { }
       
        if(prevHp> Variables.health)
        {

            Variables.mainAudioSource.PlayOneShot(painSound);            
        }
        
        if (Variables.health <= 0 && prevHp != Variables.health)
        {
            
            Variables.deathPositions.Add(Variables.distance);
            Variables.playerStats = null;
            Variables.DeathMenu.menu.SetActive(true);
            Variables.mainAudioSource.PlayOneShot(deathSound);
            Time.timeScale = 0.0f;
            //Scene scene = SceneManager.GetActiveScene();
            
        }
        prevHp = Variables.health;
    }
}