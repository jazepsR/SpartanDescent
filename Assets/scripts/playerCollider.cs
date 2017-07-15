using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase, kas apstrādā sadursmes starp spēlētāja modeli un spēles vides objektiem
public class playerCollider : MonoBehaviour
{
    Animator anim;
    int ShieldUp = Animator.StringToHash("Base Layer.ShieldUp");
    public AudioClip wolfBiteSound;
    public AudioClip wolfDeflectSound;
    public AudioClip spearHitSound;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    

void OnTriggerEnter(Collider col)
    {
        if (col.tag == "wolf") 
        {
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != ShieldUp)
            {
                Destroy(col.gameObject, 0.5f);
                if (Variables.playerStats.activeTraits.Contains("Frail"))
                    Variables.health -= 2;
                else
                    Variables.health--;
                Debug.Log("Bitten by wolf");
                Variables.mainAudioSource.PlayOneShot(wolfBiteSound);
            }
            else
            {
                Debug.Log("Blocked wolf");
                Variables.mainAudioSource.PlayOneShot(wolfDeflectSound);
                Variables.playerStats.fame += 10;
                GoalChecker.blockedWolves++;
                if (GoalChecker.blockedWolves == 5)
                {
                    GoalChecker.Blocked5Wolves = true;
                    Helpers.ShowGUIText("Goal Completed", 3.5f);
                }
            }
        }


        if (col.tag == "spear")
        {
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash != ShieldUp)
            {
                if (Variables.playerStats.activeTraits.Contains("Strong"))
                    Variables.health--;
                else
                    Variables.health -= 2;                
                Debug.Log("Hit by spear!");
                Variables.mainAudioSource.PlayOneShot(spearHitSound);

            }else
            {
                Debug.Log("Deflected spear!");
                Variables.mainAudioSource.PlayOneShot(wolfDeflectSound);
            }
            Destroy(col.gameObject);
        }
    }
}
