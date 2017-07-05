using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase, kas izveido sķēpa objektu ģenerēšanu atbilstošajā spēlētāja animācijas stadijā.
public class ThrowStateMachine : StateMachineBehaviour {  
    GameObject spear;    

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (spear == null)
        {
            spear = Resources.Load("prefabs/playerSpear") as GameObject;
        }
        if (Variables.spearTarget != null)
        {
            Vector3 dir = Vector3.Normalize(Variables.spearTarget - Variables.player.position);
            GameObject spearObj = Instantiate(spear, Variables.shotPoint.position, Quaternion.LookRotation(dir));            
            //spearObj.transform.LookAt(Variables.player.position);
            Rigidbody rb = spearObj.GetComponent<Rigidbody>();
            float dist = Vector3.Magnitude(Variables.spearTarget - Variables.player.position);
            var throwSpeed = Helpers.calculateBestThrowSpeed(Variables.player.position, Variables.spearTarget, 0.06f*dist);
            rb.AddForce(throwSpeed, ForceMode.VelocityChange);
            Variables.spearCount--;
        }
    }

}
