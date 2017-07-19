using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Klase, kas kontrolē spēlētāja mestā šķēpa sadursmes ar objektiem spēles vidē
public class PlayerSpearScript : MonoBehaviour
{
    GameObject explosion;
    AudioClip explosionSound;
    AudioClip spearHitWood;
    AudioClip wolfHurt;
    AudioClip fleshHit;
    Rigidbody rb;
    void Start()
    {
        explosion = Resources.Load("prefabs/explosion") as GameObject;
        explosionSound = Resources.Load("sounds/explosion") as AudioClip;
        spearHitWood = Resources.Load("sounds/spearHitWood") as AudioClip;
        wolfHurt = Resources.Load("sounds/wolfHurt") as AudioClip;
        fleshHit = Resources.Load("sounds/fleshHit") as AudioClip;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity)*Quaternion.Euler(90,0,0);
    }
   
    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "pan")
        {
            col.gameObject.GetComponent<PanScript>().GotHit(1);
            Variables.playerStats.fame += 8;
            Destroy(gameObject);
        }
       if ( col.gameObject.tag ==  "wildFiretrigger" )
        {
            Instantiate(explosion, col.transform.position,Quaternion.identity);
            //Destroy(col.transform.parent.gameObject);
            Variables.mainAudioSource.PlayOneShot(explosionSound);
            Variables.mainAudioSource.PlayOneShot(spearHitWood);
            
            Variables.playerStats.fame += 2;
            /*if (col.gameObject.tag == "barrelTrigger")
            {
                GoalChecker.blownUpBarrels++;
                if (GoalChecker.blownUpBarrels == 50)
                {
                    GoalChecker.BlewUp50Barrels = true;
                    Helpers.ShowGUIText("Goal completed", 3.5f);
                    Variables.playerStats.fame += 50;
                }
                Debug.Log("Hit Barrel");
            }*/
            Destroy(gameObject);
        }
        if (col.gameObject.tag == "wolf")
        {
            Variables.mainAudioSource.PlayOneShot(wolfHurt);
            Variables.mainAudioSource.PlayOneShot(fleshHit);
            Destroy(col.transform.parent.gameObject);
            Debug.Log("Hit wolf");
            Destroy(gameObject);
            Variables.playerStats.fame += 15;
        }
    }
}
