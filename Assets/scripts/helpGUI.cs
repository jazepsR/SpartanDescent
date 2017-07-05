using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Klase, kas kontorlē palīdzības informācijas vizuālās saskarnes elementus
public class helpGUI : MonoBehaviour {
    public Image pcControls;
    public Image touchControls;
    public GameObject help;

    void Start()
    {
        if (pcControls != null)
        {
#if UNITY_STANDALONE
            pcControls.gameObject.SetActive(true);
#else
        touchControls.gameObject.SetActive(true);
#endif
        }
    }

    public void clickedhelpBtn()
    {
        if (help.active)
        {
            hideHelp();
        }
        else
        {
            showHelp();

        }
    }

    public void showHelp()
    {
        help.SetActive(true);
        Time.timeScale = 0.0f;

    }
    public void hideHelp()
    {
        help.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
