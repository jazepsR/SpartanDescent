using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanThrowStateMachine : StateMachineBehaviour {
    GameObject barrel;
    public bool isPickup;
    Transform barrelPoint;
    GameObject newBarrel;
    PanScript panScript;
     // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
     //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
     //
     //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (barrel == null)
        {
            barrel = Resources.Load("prefabs/barrel") as GameObject;
            panScript = animator.gameObject.GetComponent<PanScript>();
            barrelPoint = panScript.barrelPoint;
        }


        if (isPickup)
        {
            newBarrel = Instantiate(barrel, barrelPoint);
            panScript.activeBarrel = newBarrel;
            newBarrel.AddComponent<Rigidbody>();
            newBarrel.GetComponent<Rigidbody>().isKinematic = true;
            newBarrel.transform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
            newBarrel.GetComponent<moveUpDown>().enabled = false;
        }
        else
        {
            newBarrel = panScript.activeBarrel;
            newBarrel.GetComponent<moveUpDown>().enabled = true;
            Rigidbody rb = newBarrel.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            newBarrel.GetComponent<BarrelScript>().thrown = true;
            newBarrel.GetComponent<BarrelScript>().waterLVL = panScript.waterLevel-0.2f;
            newBarrel.transform.parent = null;
            Destroy(newBarrel, 25f);
            rb.AddForce((-barrelPoint.forward/2 + barrelPoint.up/2) * Random.Range(5,10), ForceMode.VelocityChange);

                // rb.AddForce(Helpers.calculateBestThrowSpeed(newBarrel.transform.position,Variables.player.position,0.4f), ForceMode.VelocityChange);
           // Time.timeScale = 0.0f;
        }       

	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
