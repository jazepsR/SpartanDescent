using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanScript : MonoBehaviour {
    Animator anim;
    public Transform barrelPoint;
    int Idle = Animator.StringToHash("Idle");
    int Throw = Animator.StringToHash("Throw");
    int Dance = Animator.StringToHash("Dance");
    int Hurt = Animator.StringToHash("Hurt");
    [HideInInspector]
    public float waterLevel;
    [HideInInspector]    
    public GameObject activeBarrel;
    public int health = 2;
    GameObject smoke;
    // Use this for initialization
    void Start () {
        smoke = Resources.Load("prefabs/smoke") as GameObject;
        waterLevel = Variables.waterLevel;
        InvokeRepeating("SetTrigger", 0.0f, 1.0f);
        anim = GetComponent<Animator>();
        
        
    }
	void SetTrigger()
    {
        float dist = Vector3.Distance(transform.position, Variables.player.position);
        int max = 3;
        if ( dist >5  && dist<100)
            max = 10;
        int rnd = Random.Range(1, max);
        if (rnd == 1)
            anim.SetTrigger(Idle);
        if (rnd == 2)
            anim.SetTrigger(Dance);
        if (rnd >= 3)
            anim.SetTrigger(Throw);
       // print("setting trigger");
       
    }
	// Update is called once per frame
	void Update () {
        Rotate();
        
		
	}
    public void GotHit(int dmg)
    {
        health -= dmg;
        anim.SetTrigger(Hurt);
        Destroy(activeBarrel);
        if (health <= 0) {
            Variables.playerStats.fame += 10;            
            Destroy(gameObject, 1f);
            var smokeObj =Instantiate(smoke, new Vector3(transform.position.x, transform.position.y - 1.3f, transform.position.z), Quaternion.identity);
            Destroy(smokeObj, 3.5f);
        }

    }

    void Rotate()
    {
        Vector3 direction = (Variables.player.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0,300, 0);       
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * 500f);
        //transform.Rotate(new Vector3(0, -90, 0));

    }
}
