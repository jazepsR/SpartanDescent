using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase, kontolē vilka uzvedību un animāciju
public class wolfScript : MonoBehaviour
{
    Animator anim;
    int canRun = Animator.StringToHash("canRun");
    int canJump = Animator.StringToHash("canJump");
    int run = Animator.StringToHash("Base Layer.Run");
    //int run1 = Animator.StringToHash("Base Layer.Run 1");
    int jump = Animator.StringToHash("Base Layer.Jump");
    int midAir = Animator.StringToHash("Base Layer.MidAir");
    float speed = 6f;
    float jumpDist = 15f;
    Rigidbody rb = null;
    Vector3 lookPos;
    bool playedSound = false;
    public AudioClip wolfStartSound;
    public AudioClip wolfJumpSound1;
    public AudioClip wolfJumpSound2;
    // Use this for initialization
    void Start()
    {
       
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, Variables.player.position);


        if (dist < jumpDist)
        {
            anim.SetTrigger(canJump);
            

        }
        if (dist < 30f&& dist > jumpDist)
        {
            anim.SetTrigger(canRun);
            if (!playedSound)
            {
                Variables.mainAudioSource.PlayOneShot(wolfStartSound);
                playedSound = true;
            }

        }
        
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == run )
        {
            transform.position = transform.position + transform.right * Time.deltaTime * speed;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == jump)
        {

            lookPos = Variables.player.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation *= Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 4f);
        }
        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash == midAir)
        {
            if (rb == null)
            {
                
                    Variables.mainAudioSource.PlayOneShot(wolfJumpSound1);
                    Variables.mainAudioSource.PlayOneShot(wolfJumpSound2);
                    playedSound = true;
                
                rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
                rb.mass = 10f;
                Vector3 target = Variables.player.position + Variables.player.forward * 2.7f;

                Vector3 throwSpeed = Helpers.calculateBestThrowSpeed(transform.position, target, 0.6f);
                rb.AddForce(throwSpeed, ForceMode.VelocityChange);
                Destroy(gameObject, 2.0f);
                /*lookPos.y = 0;
                
                var jumpdir = Vector3.Normalize(lookPos);
                jumpdir.y += 0.2f;
                rb.AddForce(jumpdir * 400);*/
            }
        }

    }
}
