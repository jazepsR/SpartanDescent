using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Palīgklase, kas parāda spēlētājam informatīvu tekstu, ka šī ir pirmā reize, kad viņš ir 
//nonācis atbilstošajā līmenī
public class firstTimeHelp : MonoBehaviour {
    public GameObject Help;
    public helpType type;
    public enum helpType { level,hub,marrige};

    void Start()
    {
        switch (type)
        {
            case helpType.level:
                if (!Variables.shownLevelHelp)
                    ShowHelp();
                break;
            case helpType.hub:
                if (!Variables.shownHubHelp)
                    ShowHelp();
                break;
            case helpType.marrige:
                if (!Variables.shownMarrigeHelp)
                    ShowHelp();
                break;
        }





    }




    public void ShowHelp()
    {
        Help.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void hideHelp()
    {
        switch (type)
        {
            case helpType.level:
                Variables.shownLevelHelp = true;
                break;
            case helpType.hub:
                Variables.shownHubHelp = true;
                break;
            case helpType.marrige:
                Variables.shownMarrigeHelp = true;
                break;                
        }
        Time.timeScale = 1.0f;
        Help.SetActive(false);


    }

}
