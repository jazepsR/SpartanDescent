using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Palīgklase, kas ļauj spēlētājam izvēlēties, kā notiek varoņa nomaiņa- vai nu turpināt spēli ar 
//esošā varoņa brāli, vai apprecēties un spēli turpināt ar esošā varoņa bērniem.
public class FinishMenu : MonoBehaviour {
    public GameObject childList;
    public GameObject retireMenu;
    public GameObject menuObj;
    public Button retireBtn;
	// Use this for initialization
	void Start () {
        Variables.finishDialog = menuObj;
        if (retireBtn != null && Variables.children.Count == 0)
            retireBtn.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CancelBtn()
    {
        if (childList.active)
        {
            childList.SetActive(false);
            retireMenu.SetActive(true);
        }else
        {
            SceneManager.LoadScene("hub");
            menuObj.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
    public void Marry()
    {

        SceneManager.LoadScene("village");
    }
    public void Retire()
    {
        childList.SetActive(true);
        retireMenu.SetActive(false);
    }
}
