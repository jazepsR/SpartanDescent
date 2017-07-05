using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase kas glabā vārtu stāvokli un ainu, uz informāciju par to uz kuru ainu vārti ved
public class gateScript : MonoBehaviour {
    public GameObject grate;
    public bool hasGrate = false;
    public string tagName="toHub";
    // Use this for initialization
    void Awake () {
        grate.SetActive(hasGrate);
        gameObject.tag = tagName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
