using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Palīgklase, kas parāda varoņa vārdu grafiskajā lietotāja saskarnē.
public class NameScript : MonoBehaviour {
    int age = 0;
    Text nameText;
	// Use this for initialization
	void Start () {
        nameText = GetComponent<Text>();
        nameText.text = Variables.playerStats.name;
	}
	

}
