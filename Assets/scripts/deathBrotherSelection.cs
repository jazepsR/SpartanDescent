using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Klase vada jauna varoņa izvēli pēc esošā varoņa nāves
public class deathBrotherSelection : MonoBehaviour {
    public GameObject childPrefab;
    public ScrollRect childListGUI;
    public locationType location = locationType.world;
    public enum locationType { world,hub,village};
	// Use this for initialization
	void Awake () {
       
        //Variables.children.Add(new character("testDude"));
        foreach (character child in Variables.children)
        {
            GameObject childInfo = Instantiate(childPrefab);
            string childStats = child.name;
            if (child.activeTraits.Count > 0)
                childStats += "\n ";
            foreach (string activeTrait in child.activeTraits)
            {
               
                //'\u2022' + " " +
                childStats += '\u2022' + activeTrait + "  ";
            }

            childStats += "\n Beauty: " + child.beauty.ToString();



            childInfo.GetComponentInChildren<Text>().text = childStats;
            Button btn = childInfo.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => SelectChild(child.name));
            childInfo.transform.SetParent(childListGUI.content.transform, false);
        }
        if (Variables.children.Count == 0)
        {
            childListGUI.gameObject.SetActive(false);
        }
    }

    private void SelectChild(string name)
    {
        Debug.Log(name);
        Helpers.PlayButtonSound();
        foreach (character ch in Variables.children)
        {
            if (ch.name == name)
            {
                if (location != locationType.world)
                {
                    ch.inventory = Variables.playerStats.inventory;
                    foreach(Item item in ch.inventory)
                    {
                        item.applyItem();
                    }
                }
                Variables.playerStats = ch;
                Variables.children.Remove(ch); 
                Variables.playerStats.PrintTraits();
                break;
            }
        }
        Time.timeScale = 1.0f;
        if (location == locationType.hub)
        {
            SceneManager.LoadScene("hub");
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
       

    }
}
