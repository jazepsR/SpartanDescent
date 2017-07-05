using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Klase, kas kontrolē pogas "MainMenu" ainā.
public class MainMenu : MonoBehaviour {
    public Button continueBtn;
    public AudioSource source;
    public AudioClip btnSound;
	// Use this for initialization
	void Start () {
        bool hasSave = SaveLoad.Load();
        Variables.mainAudioSource = source;
        continueBtn.interactable = hasSave;
	}
	
	public void ContinueBtn()
    {
        source.PlayOneShot(btnSound);
        if (GoalChecker.CompletedIntro)
            SceneManager.LoadScene("hub");
        else
            SceneManager.LoadScene("Level1");
    }
    public void NewGameBtn()
    {
        source.PlayOneShot(btnSound);
        Variables.playerStats = new character(Helpers.GetName(true));
        GoalChecker.ClearGoals();        
        SceneManager.LoadScene("Level1");        
    }
}
