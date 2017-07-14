using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Palīgklase, maina objekta pozīciju ar sīnusveida svārstībām
public class moveUpDown : MonoBehaviour {

    Vector3 startPos;

    public float amplitudeZ;
    public float periodZ;
    public bool useRandom;
    public bool useRot;
    public float rotSpeed;
    protected void Start()
    {
        startPos = transform.position;
        if (useRandom)
        {
            periodZ = Random.Range(periodZ * 0.85f, periodZ * 1.15f);
            amplitudeZ = Random.Range(amplitudeZ * 0.85f, amplitudeZ * 0.85f);
        }
       
    }

    protected void Update()
    {
        if (Time.timeScale != 0.0f)
        {
            float theta = Time.timeSinceLevelLoad / periodZ;
            float distance = amplitudeZ * Mathf.Sin(theta);
            transform.position = transform.position +(Vector3.up * distance);
            if (useRot)
            {
                transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0),Space.World);

            }

        }
    }
}
