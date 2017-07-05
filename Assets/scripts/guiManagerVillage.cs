using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Klase, kas kontorlē vizuālo lietotāja saskarni ainā "village"
public class guiManagerVillage : MonoBehaviour {
    public Text playerStatsText;
    public ScrollRect childListGUI;
    public List<character> childList = new List<character>();
    public GameObject childPrefab;
    public GameObject marryBtn;
    public GameObject dialog;
    public AudioSource source;
    public AudioClip yesMarrige;
    public AudioClip noMarrige;
    public AudioClip noMarrige2;
    public GameObject BrotherList;
    public GameObject[] playerChars;
    public childToAdultAnim childAnim;
	// Use this for initialization
	void Start () {
        Variables.mainAudioSource = source;       
       playerStatsText.text = Variables.playerStats.PrintBio();        
        if(Variables.playerStats.currentStage== character.ageStage.oldAge)
        {
            playerChars[2].SetActive(true);
            tavernPerson[] persons = FindObjectsOfType(typeof(tavernPerson)) as tavernPerson[];
            foreach(tavernPerson person in persons)
            {
                Destroy(person.transform.gameObject);
            }
            Helpers.ShowGUIText("You are too old to marry", 5.0f);
            marryBtn.SetActive(false);            
            BrotherList.SetActive(true);

        }
        if (Variables.playerStats.currentStage == character.ageStage.middleAge)
        {
            Helpers.ShowGUIText("Middle aged: will have less kids", 5.0f);
            playerChars[1].SetActive(true);
        }
        if (Variables.playerStats.currentStage == character.ageStage.adulthood)
        {           
            playerChars[0].SetActive(true);
        }


    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void MarryPerson()
    {
        if (tavernPerson.selectedPerson == null)
        {
            Debug.Log("No one selected for marrige");
        }
        else
        {
            //TODO: marrige minigame
            float marrigeChance= Random.Range(0.0f, 1.0f);
            if (marrigeChance > 1-tavernPerson.selectedPerson.marrigeChance)
            {
                marryBtn.gameObject.SetActive(false);
                if (tavernPerson.selectedPerson.ch.beauty >= 12)
                {
                    GoalChecker.Marry12Beauty = true;
                    Helpers.ShowGUIText("Goal completed", 3.5f);
                }
                tavernPerson.selectedPerson.YesMarrige();
                GenChildList();
                source.PlayOneShot(yesMarrige);
                Debug.Log("You married " + tavernPerson.selectedPerson.charName);
                
                foreach (GameObject woman in GameObject.FindGameObjectsWithTag("woman"))
                {
                    if(woman.GetComponent<tavernPerson>()!= tavernPerson.selectedPerson)
                    {
                        woman.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.Log("You didnt impress the lady");
                if (Helpers.RandomBool())
                    source.PlayOneShot(noMarrige);
                else
                    source.PlayOneShot(noMarrige2);
                tavernPerson[] persons = FindObjectsOfType(typeof(tavernPerson)) as tavernPerson[];
                if(persons.Length == 1)
                {
                    marryBtn.SetActive(false);
                    BrotherList.SetActive(true);
                }
                foreach (tavernPerson pers in persons)
                {
                    pers.ch.beauty += 2;
                    pers.SetTexts();
                }
                //dialog.SetActive(true);
                tavernPerson.selectedPerson.NoMarrige();

            }
            //tavernPerson.selectedPerson.gameObject.SetActive(false);
            tavernPerson.selectedPerson = null;
        }
    }
    void GenChildList()
    {
        //Create children list;
        int ChildCount = 3;
        if (Variables.playerStats.activeTraits.Contains("Bountiful"))
            ChildCount++;
        if (Variables.playerStats.activeTraits.Contains("Barren"))
            ChildCount--;
        if(Variables.playerStats.currentStage == character.ageStage.middleAge)
        {
            ChildCount--;
        }
        if (tavernPerson.selectedPerson.ch.activeTraits.Contains("Bountiful"))
            ChildCount++;
        if (tavernPerson.selectedPerson.ch.activeTraits.Contains("Barren"))
            ChildCount--;
        
        for(int i = 0; i < ChildCount;i++)
        {
            string childName = Helpers.GetName(true);
            character ch = new character(Variables.playerStats, tavernPerson.selectedPerson.ch, childName);
            childList.Add(ch);
            string childStats = childName;
            if(ch.activeTraits.Count>0)
                childStats += "\n ";
            GameObject childInfo = Instantiate(childPrefab);
            foreach (string activeTrait in ch.activeTraits)
            {
                //'\u2022' + " " +
                childStats += '\u2022'+activeTrait+"  ";
            }

            childStats += "\n Beauty: " + ch.beauty.ToString();



            childInfo.GetComponentInChildren<Text>().text = childStats;
            Button btn = childInfo.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => SelectChild(childName));
            childInfo.transform.SetParent(childListGUI.content.transform, false);
        }
        childListGUI.gameObject.SetActive(true);

    }
    private void SelectChild(string name)
    {
        Debug.Log(name);
        Helpers.PlayButtonSound();
        
        foreach(character ch in childList)
        {
            if(ch.name==name)
            {
                ch.inventory = Variables.playerStats.inventory;
                Variables.playerStats = ch;
                foreach (Item item in ch.inventory)
                {
                    item.applyItem();
                }
                Variables.children = childList;
                childList.Remove(ch);                
                childListGUI.gameObject.SetActive(false);
                Variables.playerStats.PrintTraits();
                break;
            }
        }
        childAnim.StartAnim();


    }
    public void closeDialog()
    {
        dialog.SetActive(false);
    }
}
