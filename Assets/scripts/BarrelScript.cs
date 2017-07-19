using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    [HideInInspector]
    public bool thrown = false;
    [HideInInspector]
    public float waterLVL;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "terrain")
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null && thrown)
            {
                thrown = false;
                LeanTween.moveY(gameObject, Mathf.Min(transform.position.y + 0.8f,waterLVL), 0.6f).setEase(LeanTweenType.easeInOutBounce);
                Destroy(rb);

            }
        }
    }
}
