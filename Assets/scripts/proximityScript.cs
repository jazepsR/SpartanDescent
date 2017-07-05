using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase, kas reģistrē objektus spēlētāja tuvumā un satur metodes objektu tuvuma informācijas apstrādei
public class proximityScript : MonoBehaviour {
    public AudioSource proximitySound;
    // Use this for initialization
    [HideInInspector]
    public float fireProx = 0.3f;
	void Start () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "fire")
        {
            col.tag = "fire2";
            fireProx += 0.2f;
        }
    }
    void Update()
    {
        transform.position = Variables.player.position;
        if (Variables.currentLVL == Variables.levels.fire)
        {
            fireProx -= Time.deltaTime/7;
            fireProx = Mathf.Clamp(fireProx, 0.3f, 1f);
        }
        else {
            fireProx = 0;
        }
        proximitySound.volume = fireProx;
    }
}
