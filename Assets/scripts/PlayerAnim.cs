using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase, kas atblid par galvenā varoņa animācjām, atblistoši spēlētāja ievadiem

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    public GameObject handsR;
    public GameObject handsL;
    int AttackL = Animator.StringToHash("StabLeft");
    int AttackR = Animator.StringToHash("StabRight");
    int ShieldLift = Animator.StringToHash("ShieldUp");
    int ThrowSpear = Animator.StringToHash("ThrowSpear");
    int InPain = Animator.StringToHash("InPain");
    zombieHandsScript LHandScript;
    zombieHandsScript RHandScript;
    int ThrowState = Animator.StringToHash("Base Layer.SpearThrow");
    int prevHp;
    float rotationSpeed = 500f;
    public AudioClip shieldRaiseSound;
    public AudioClip stabSound;
    public AudioClip throwSpearSound;
    int IdleState = Animator.StringToHash("Base Layer.Idle");

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
                                 // Use this for initialization
    public AudioClip noSpears; 
    void Start()
    {
        prevHp = Variables.health;
        dragDistance = Screen.height*15/ 100; //dragDistance is 15% height of the screen
        anim = GetComponent<Animator>();
        if (handsL != null)
        {
            LHandScript = handsL.GetComponent<zombieHandsScript>();
            RHandScript = handsR.GetComponent<zombieHandsScript>();
        }
    }
      
    
    // Update is called once per frame
    void Update()
    {

        if (Variables.health < prevHp)
        {
            anim.SetTrigger(InPain);
        }

        Rotate();

        prevHp = Variables.health;
#if UNITY_ANDROID
        SwipeInputs();
#endif
#if UNITY_IPHONE
        SwipeInputs();
#endif
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleState)
            {
                anim.SetTrigger(AttackR);
                Variables.mainAudioSource.PlayOneShot(stabSound);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            
            if (anim.GetCurrentAnimatorStateInfo(0).fullPathHash == IdleState)
            {
                anim.SetTrigger(AttackL);
                Variables.mainAudioSource.PlayOneShot(stabSound);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool(ShieldLift, true);
            Variables.mainAudioSource.PlayOneShot(shieldRaiseSound);
            StartCoroutine("ResetShield");
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {

                    //if (hit.collider.gameObject.tag == "barrel" || hit.collider.gameObject.tag == "wolf")
                    //{
                        
                    //}
                    if (hit.collider.gameObject.tag == "zombieHandsL")
                    {
                        anim.SetBool(ShieldLift, false);
                        anim.SetTrigger(AttackR);
                        LHandScript.Descend();
                    }
                    else
                    {
                        if (hit.collider.gameObject.tag == "zombieHandsR")
                        {
                            anim.SetBool(ShieldLift, false);
                            anim.SetTrigger(AttackL);
                            RHandScript.Descend();
                        }else
                        {
                            if (Variables.spearCount > 0)
                            {

                                anim.SetBool(ShieldLift, false);
                                Variables.spearTarget = hit.point;
                                // Variables.spearTarget = hit.collider.gameObject.transform;                            
                                Variables.mainAudioSource.PlayOneShot(stabSound);
                                //Variables.mainAudioSource.PlayOneShot(throwSpearSound);
                                anim.SetTrigger(ThrowSpear);
                            }else
                            {
                                Variables.mainAudioSource.PlayOneShot(noSpears);
                            }
                        }
                    }
                }
            }


        }
    }

    void Rotate()
    {

        if(anim.GetCurrentAnimatorStateInfo(0).fullPathHash== ThrowState)
        {
            Vector3 direction = (Variables.spearTarget - transform.position);

            //create the rotation we need to be in to look at the target
            Quaternion lookRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, Variables.player.localRotation.y, 0);
            //transform.parent.rotation = lookRotation // * Quaternion.Euler(-1.842f, 180f, 0.77f);// *Quaternion.Euler(0,Variables.player.localRotation.y,0);
            //rotate us over time according to speed until we are in the required rotation
            transform.parent.rotation = Quaternion.RotateTowards(transform.parent.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        }
        else
        {
            transform.parent.localRotation = Quaternion.RotateTowards(transform.parent.localRotation, Quaternion.Euler(0,0,0), Time.deltaTime * rotationSpeed/2);
        } 



    }
    void SwipeInputs()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 15% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Debug.Log("Right Swipe");
                        }
                        else
                        {   //Left swipe
                            Debug.Log("Left Swipe");
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Debug.Log("Up Swipe");
                            anim.SetBool(ShieldLift, true);
                            Variables.mainAudioSource.PlayOneShot(shieldRaiseSound);
                            StartCoroutine("ResetShield");
                        }
                        else
                        {   //Down swipe
                            Debug.Log("Down Swipe");
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }


    IEnumerator ResetShield()
    {

        
            yield return new WaitForSeconds(3f);
        anim.SetBool(ShieldLift, false);
    }
}
