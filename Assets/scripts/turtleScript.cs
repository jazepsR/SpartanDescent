using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turtleScript : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float turnforce;
    public float maxSpeed;
    public bool CanTurn = true;
    public bool should180 = false;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Magnitude(rb.velocity)<maxSpeed)
            rb.AddForce(transform.forward * speed * Time.deltaTime*rb.mass);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "terrain" && CanTurn)
        {
            CanTurn = false;
            rb.AddForce(transform.forward * turnforce,ForceMode.Impulse);
            if(should180)
                LeanTween.rotateLocal(gameObject,new Vector3(0,180,0), 0.7f).setOnComplete(AllowTurn);
            else
                LeanTween.rotateLocal(gameObject, new Vector3(0, 0, 0), 0.7f).setOnComplete(AllowTurn);

            should180 = !should180;

        }
    }
    void AllowTurn()
    {
        CanTurn = true;
    }
}
