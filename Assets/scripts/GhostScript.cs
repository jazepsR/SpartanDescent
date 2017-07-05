using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase, kas kontolē spēlētāja spoku uzvedību
public class GhostScript : MonoBehaviour {
    Animator anim;
    AnimatorStateInfo animInfo;
    GameObject spear;
    public float force;
    public Transform shotPoint;
    int ThrowUpStateHash = Animator.StringToHash("Base Layer.SpearThrowUp");
    int ThrowDownStateHash = Animator.StringToHash("Base Layer.SpearThrowDown");
    bool inState = true;
    AudioClip ghostAwakeSound;
    AudioClip ghostShootSound;
    bool playedSound = false;
	// Use this for initialization
	void Start () {
        spear = Resources.Load("prefabs/spear") as GameObject;        
        anim = GetComponent<Animator>();
        //ghostAwakeSound = Resources.Load("sound/ghostAwake") as AudioClip;
        ghostShootSound = Resources.Load("sounds/ghostShoot") as AudioClip;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(transform.position, Variables.player.position)>20f)
            return;
        if (!playedSound)
        {
            //Variables.mainAudioSource.PlayOneShot(ghostAwakeSound);
        }
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == ThrowDownStateHash && inState)
        {
            GameObject spearObj = Instantiate(spear,shotPoint.position,Quaternion.identity);
            var throwSpeed = Helpers.calculateBestThrowSpeed(shotPoint.position, Variables.player.position, 1f);            
            spearObj.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.VelocityChange);
            spearObj.transform.LookAt(Variables.player);
            Variables.mainAudioSource.PlayOneShot(ghostShootSound);
            inState = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == ThrowUpStateHash && !inState)
        {
            inState = true;
        }
     }
        
    
}
