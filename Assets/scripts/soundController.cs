using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Palīgklase, kas nosaka ainā izmantoto galveno audio efektu avotu
public class soundController : MonoBehaviour {

    public  AudioSource mainSource;
    


    void Awake()
    {
        mainSource = GetComponent<AudioSource>();
        Variables.mainAudioSource = mainSource;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
