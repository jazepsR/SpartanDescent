using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Klase, kas kontrolē bērna pieaugšanas animāciju ainā "Village"

public class childToAdultAnim : MonoBehaviour {
    public Text bio;
    public GameObject AnimMenu;
    public Image crib;
    public Image adult;
    public Text title;
    public Image Bg;
    int animState = 0;
    float currentWaitTime = 0;
    public Image nextBtn;
    public AudioClip babySound;
    public Text btnText;
	// Use this for initialization
	void Start () {
             
    }
    public void StartAnim()
    {
        nextBtn.gameObject.SetActive(true);
        AnimMenu.SetActive(true);
        Variables.mainAudioSource.PlayOneShot(babySound);
        animState = 1;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1.0f;
        if (GoalChecker.CompletedIntro)
        {
            SceneManager.LoadScene("hub");
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }
    void Update()
    {
        switch (animState) {
            case 0:
                break;
                //fade in crib and bg
            case 1:        
            float alpha = crib.color.a + Time.fixedDeltaTime / 3;            
            crib.color = new Color(1, 1, 1, alpha);
            Bg.color = new Color(0, 0, 0, alpha);
                if (alpha > 1)
                {
                    
                    animState = 2;
                }
            break;
                //wait
            case 2:
                currentWaitTime += Time.fixedDeltaTime;
                if (currentWaitTime > 2)
                    animState = 3;
                break;
                // fade out crib, fade in text
            case 3:
                alpha = crib.color.a - Time.fixedDeltaTime / 3;
                crib.color = new Color(1, 1, 1, alpha);
                title.color = new Color(1, 1, 1,1- alpha);
                if (alpha < 0)
                {
                    bio.text = Variables.playerStats.PrintStats();
                    animState = 4;
                }
                break;
            // fade in button, adult sprite and bio
            case 4:
                alpha = adult.color.a + Time.fixedDeltaTime / 3;
                adult.color = new Color(1, 1, 1, alpha);
                bio.color = new Color(1, 1, 1, alpha);
                nextBtn.color = new Color(1, 1, 1, alpha);
                btnText.color = new Color(1, 1, 1, alpha);
                if (alpha > 1)
                    animState = 0;
                break;

        }

    }
    


}

