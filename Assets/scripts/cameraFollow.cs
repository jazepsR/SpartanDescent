using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Klase kontrolē kameras pozīciju
public class cameraFollow : MonoBehaviour {
    [SerializeField]
    private Transform target;    
    [SerializeField]
    private Vector3 offsetPosition;

    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    [SerializeField]
    private bool lookAt = true;
   

    private void Update()
    {
        Refresh();
        
    }

    

    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        if (offsetPositionSpace == Space.Self)
        {
            // transform.position =new Vector3( target.TransformPoint(offsetPosition).x, transform.position.y, target.TransformPoint(offsetPosition).z) ;
            float yPos = Mathf.MoveTowards(transform.position.y, target.TransformPoint(offsetPosition).y, Time.deltaTime * 1.7f);
            transform.position = new Vector3(target.TransformPoint(offsetPosition).x, yPos, target.TransformPoint(offsetPosition).z);
        }
        else
        {
            transform.position = target.position + offsetPosition;
        }
      
        if (lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}

