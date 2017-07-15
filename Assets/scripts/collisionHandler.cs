using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Klase kontrolē spēlētāja laivas objekta sadursmes ar citiem objektiem spēles vidē.
public class collisionHandler : MonoBehaviour {
    public Transform boatFront;
    public GameObject explosion;
    GameObject explosionGreen;
    public proximityScript ProxScript;
    public AudioClip hitRockSound;
    public AudioClip getSpearSound;
    public AudioClip explosionSound;
    public AudioClip defuseSound;
    //public AudioClip gateSound;
    Rigidbody rb;    
    // Use this for initialization
    void Start () {
        explosionGreen = Resources.Load("prefabs/explosionGreen") as GameObject;      
        rb = GetComponent<Rigidbody>();
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "rock")
        {
            Debug.Log("Hit Rock");     
                   
            col.gameObject.tag = "Untagged";
            Variables.health--;
            Variables.mainAudioSource.PlayOneShot(hitRockSound);
            if (Variables.player.position.z > col.gameObject.transform.position.z)
            {
                Debug.Log("push left");
                rb.AddForceAtPosition(boatFront.position, new Vector3(0, 0, 1));
            }else
            {
                Debug.Log("push right");
                rb.AddForceAtPosition(boatFront.position, new Vector3(0, 0, -1));
            }

        }
        if(col.gameObject.tag == "FameBoost")
        {
            Variables.playerStats.fame += 15;
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "barrelTrigger")
        {
            float blowUpChance = 0.7f;
            if (Variables.playerStats.activeTraits.Contains("Lucky"))
                blowUpChance = 0.9f;
            if (Variables.playerStats.activeTraits.Contains("Unlucky"))
                blowUpChance = 1.0f;
            if (Random.Range(0.0f, 1.0f) < blowUpChance)
            {
                Vector3 expDir = Vector3.Normalize(transform.position - col.gameObject.transform.position);
                rb.AddForce(2000 * rb.mass * expDir);
                Variables.health--;
                GameObject exp = Instantiate(explosion, col.transform.position, Quaternion.identity);
                Variables.mainAudioSource.PlayOneShot(explosionSound);
                Destroy(exp, 3.0f);
                Destroy(col.gameObject);
            }
            else
            {
                Variables.mainAudioSource.PlayOneShot(defuseSound);
            }
        }
        if (col.gameObject.tag == "barrel")
        {
            float blowUpChance = 0.7f;
            if (Variables.playerStats.activeTraits.Contains("Lucky"))
                blowUpChance = 0.9f;
            if (Variables.playerStats.activeTraits.Contains("Unlucky"))
                blowUpChance = 1.0f;
            if (Random.Range(0.0f, 1.0f) < blowUpChance)
            {
                Vector3 expDir = Vector3.Normalize(transform.position - col.gameObject.transform.position);
                rb.AddForce(2000 * rb.mass * expDir);
                Variables.health = Variables.health - 2;
                GameObject exp = Instantiate(explosionGreen, col.transform.position, Quaternion.identity);
                Variables.mainAudioSource.PlayOneShot(explosionSound);
                Destroy(exp, 3.0f);
                Destroy(col.gameObject);
            }
            else
            {
                Variables.mainAudioSource.PlayOneShot(defuseSound);
            }
        }



    }
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "quiver")
        {
            Variables.spearCount += 3;
            Destroy(col.gameObject);
            Variables.mainAudioSource.PlayOneShot(getSpearSound);
            Variables.playerStats.fame += 3;
        }
        if(col.tag == "fire" || col.tag == "fire2")
        {
            col.gameObject.tag = "Untagged";
            Variables.health--;
            Debug.Log("Hit fire");
            if (ProxScript != null)
            {
                ProxScript.fireProx += 0.40f;
            }
        }
        if (col.tag == "exitTrigger")
        {
            Variables.health = 5;
            SaveLoad.Save();
            SceneManager.LoadScene("village");
            Debug.Log("Hit exit trigger");
        }
        if (col.tag == "waterTurn")
        {
            
            float rotation = col.gameObject.GetComponent<waterScript>().rotation;
            Destroy(col.gameObject, 5.0f);
            GetComponent<PlayerController>().ChangeLocalRot(rotation);
            DoNextWater(col);
            // Debug.Log("Hit turn");
        }
        if (col.tag == "splitR")
        {
            
            col.gameObject.transform.parent.gameObject.GetComponent<waterScript>().nextWater1.Destroy(2f);
            Destroy(col.gameObject.transform.parent.gameObject, 2.2f);
            DoNextWater(col);
            // Debug.Log("Hit turn");
        }
        if (col.tag == "splitL")
        {
                   
            col.gameObject.GetComponent<waterScript>().nextWater2.Destroy(2f);
            Destroy(col.gameObject, 2.2f);
            DoNextWater(col);
            // Debug.Log("Hit turn");
        }

        if (col.tag == "drop")
        {
            Variables.waterLevel -= 5.65f;
            zombieHandsScript[] hands = FindObjectsOfType<zombieHandsScript>();
            foreach(zombieHandsScript hand in hands)
            {
                hand.HitWaterFall();
            }
        }

        
        if (col.tag.StartsWith("to"))
        {

            SaveLoad.Save();
            //Add move to palce sound
            //Variables.mainAudioSource.PlayOneShot(gateSound);
        }

        if (col.tag == "toHub")
        {           
            SceneManager.LoadScene("hub");
            Debug.Log("Hit hub entrance trigger");
            if (SceneManager.GetActiveScene().name == "Level1")
            {
                GoalChecker.CompletedIntro = true;
            }
            if (SceneManager.GetActiveScene().name == "Level2")
            {
                GoalChecker.CompletedFire = true;
            }
            if (SceneManager.GetActiveScene().name == "Level3")
            {
                GoalChecker.CompletedDesolate = true;
            }
            GoalChecker.blockedWolves = 0;
        }
        if(col.tag == "toFire")
        {
            Variables.currentLVL = Variables.levels.fire;
            LevelSetup.SetLVL(Variables.levels.fire);
            Destroy(col.gameObject, 5.0f);         
            skyboxControl.Instance.ChangeSkybox(1);
            Variables.playerStats.fame += 20;
            DoNextWater(col);
            /*
            Variables.currentLVL = Variables.levels.fire;
            SceneManager.LoadScene("Level2");
            Debug.Log("Hit fire entrance trigger");
            */
        }
        if(col.tag == "fromHub")
        {
            Variables.finishDialog.SetActive(true);
            Time.timeScale = 0.0f;
        }
        if (col.tag == "toDesolate")
        {
            Variables.currentLVL = Variables.levels.desolate;
            LevelSetup.SetLVL(Variables.levels.desolate);
            Destroy(col.gameObject, 5.0f);           
            skyboxControl.Instance.ChangeSkybox(2);
            Variables.playerStats.fame += 35;
            DoNextWater(col);
            /*
            Variables.currentLVL = Variables.levels.desolate;          
            SceneManager.LoadScene("Level3");
            Debug.Log("Hit desolate entrance trigger");
            */
        }
        if(col.tag == "GotItem")
        {
            Variables.GotItemMenu.SetActive(true);
            Time.timeScale = 0.0f;          
            
            
        }
        if(col.tag == "toItem")
        {
            Variables.currentLVL = Variables.levels.item;
            switch (Random.Range(0, 3)){
                case 0:
                    SceneManager.LoadScene("Level1");
                    break;
                case 1:
                    SceneManager.LoadScene("Level2");
                    break;
                case 2:
                    SceneManager.LoadScene("Level3");
                    break;
            }


            Debug.Log("Hit random level trigger");
        }
        if(col.tag == "Win")
        {
            Debug.Log("You won");
            GameObject textObj = Resources.Load("prefabs/winMenu") as GameObject;            
            GameObject InstantiatedObj = Instantiate(textObj, GameObject.Find("Canvas").transform, false);            
            Time.timeScale = 0.0f;
        }
        if (col.tag == "WildFiretrigger")
        {
            Vector3 expDir = Vector3.Normalize(transform.position - col.gameObject.transform.position);
            rb.AddForce(2000 * rb.mass * expDir);
            Variables.health--;
            GameObject exp = Instantiate(explosionGreen, col.transform.position, Quaternion.identity);
            Variables.mainAudioSource.PlayOneShot(explosionSound);
            Destroy(exp, 3.0f);
            Destroy(col.gameObject);
        }
       


        /*if (col.tag == "spear")
        {
            guiControl.health -= 2;
            Destroy(col.gameObject);
            Debug.Log("Hit by spear!");
        }*/

    }
    void DoNextWater(Collider col)
    {
        col.tag = "Untagged";
        Variables.WaterGen.GenRandomWater();
        Variables.distance++;
        Variables.playerStats.fame++;
    }
}
