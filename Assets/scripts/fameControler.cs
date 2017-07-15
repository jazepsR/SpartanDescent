using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fameControler : MonoBehaviour {
    int prevFame = 0;
    public Slider slider;
    public Text fameText;
    float moveToVal = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Variables.playerStats == null)
            return;
		if(prevFame != Variables.playerStats.fame)
        {
            if (Variables.playerStats.fame >= 100)
            {
                Variables.playerStats.fame -= 100;
                Variables.playerStats.fameLVL++;
                fameText.text = "Fame: " + Variables.playerStats.fameLVL;
            }
            Hashtable options = new Hashtable();
            float fameGain = Variables.playerStats.fame - (float)prevFame;
            LeanTween.value(slider.gameObject, setSliderVal, (float)prevFame, (float)Variables.playerStats.fame, 0.1f * fameGain / 2);//.setEase(LeanTweenType.easeInBack);
        }       
        prevFame = Variables.playerStats.fame;
	}
    void setSliderVal(float val)
    {
        slider.value = val;
    }
}
