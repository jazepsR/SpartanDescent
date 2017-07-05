using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Klase, kas kontrolē pauses izvēlnes funkcionalitāti.
public class menuScript : MonoBehaviour {
	public GameObject menu;
	public GameObject stats;
	public GameObject pauseBtn;
	public MenuType type;
	public Text statText;
	public GameObject[] playerSprites;
	public Sprite[] rings;
	public Sprite[] amulets;
	public Sprite[] boots;
	public Sprite[] shirts;
	public Image[] items;
    public AudioClip buttonSound;
	public GameObject[] itemsToCheck;
	public enum MenuType { Pause, Death};
	public Text GoalText;
	public GameObject GoalList;
    

	void Awake () {
        


		if(type == MenuType.Death)
		{
			Variables.DeathMenu = this;
		}else
		{
			if (Variables.rings == null)
			{
				Variables.rings = rings;
				Variables.amulets = amulets;
				Variables.boots = boots;
				Variables.shirts = shirts;
			}
            /*if (Variables.playerStats.inventory.Count == 0)
			{
				Variables.playerStats.inventory.Add(new Item());
				Variables.playerStats.inventory.Add(new Item());
				Variables.playerStats.inventory.Add(new Item());
			}*/
            try
            {
                for (int i = 0; i < Variables.playerStats.inventory.Count; i++)
                {
                    items[i].overrideSprite = Variables.playerStats.inventory[i].itemPicture;
                    items[i].GetComponent<ItemGUI>().itemStats = Variables.playerStats.inventory[i];
                    items[i].color = new Color(1, 1, 1);
                }
            }
            catch { Debug.LogError("Error in menuscript line 47"); }
		}
		if (pauseBtn != null)
		{
			pauseBtn.SetActive(false);
        }
#if UNITY_ANDROID
			 pauseBtn.SetActive(true);   
#endif

#if UNITY_IOS
        pauseBtn.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) )
		{
			Pause();
		}
	}

	public void ShowGoals()
	{
        playSound();
        menu.SetActive(false);
		GoalText.text = GoalChecker.GetGoalList();
		GoalList.SetActive(true);
	}
    public void playSound()
    {
        Variables.mainAudioSource.PlayOneShot(buttonSound);
    }

	public void ShowStats()
	{
        playSound();
		menu.SetActive(false);
		statText.text = Variables.playerStats.PrintStats();
		stats.SetActive(true);
	}
	public void Pause()
	{
        playSound();
        if (type != MenuType.Pause || Variables.health ==0)
		{
			return;
		}
		if (menu.active || stats.active || GoalList.active)
		{
			ResumeGame();
		}
		else
		{
			Time.timeScale = 0.0f;
			menu.SetActive(true);
		   // Variables.playerStats.currentStage = character.ageStage.middleAge;
			try
			{
				foreach (GameObject img in playerSprites)
				{
					
					if (img.name == Variables.playerStats.currentStage.ToString())
						img.SetActive(true);
					else
						img.SetActive(false);
				}
			}
			catch { }

		}
	}
	public void CloseStats()
	{
        playSound();
        stats.SetActive(false);
		menu.SetActive(true);
	}

	public void CloseGoals()
	{
        playSound();
        GoalList.SetActive(false);
		menu.SetActive(true);
	}


	public bool CanUnpause()
	{
		if (itemsToCheck!= null)
		{
			bool canUnpause = true;
			foreach (GameObject menuItem in itemsToCheck)
			{
				if (menuItem.active)
				{
					canUnpause = false;
				}

			}
			return canUnpause;
		}
		else
		{
			return true;
		}
	}

	public void ResumeGame()
	{
        playSound();
        if (CanUnpause())
		{
			Time.timeScale = 1.0f;
		}

		stats.SetActive(false);
		foreach (GameObject img in playerSprites)
		{
				img.SetActive(false);
		}
		menu.SetActive(false);
	}
	public void QuitGame()
	{
        playSound();
        SaveLoad.Save();
		Application.Quit();
	}
	public void RestartGame()
	{
        playSound();
        Time.timeScale = 1.0f;
		SaveLoad.Save();
		SceneManager.LoadScene("mainMenu");
	}
    public void StartOver()
    {
        playSound();
        Time.timeScale = 1.0f;
        Variables.playerStats = new character(Helpers.GetName(true));
        GoalChecker.ClearGoals();        
        SceneManager.LoadScene("Level1");
    }
}
