using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Klase, kas kontrolē no ūdens iznākošo ienaidnieku roku uzvedību
public class zombieHandsScript : MonoBehaviour {
	Vector3 startPos;
	float moveSpeed = 0.4f;
	bool timerRunning = false;
	float reactionTime = 3.0f;
	float waitTime = 0.05f;
	float downDist;    
	public KeyCode AttackKey;
	public Transform boatPos;
    public AudioClip handsAppear1;
    public AudioClip handsAttack1;
    public AudioClip handsAttack2;
    public AudioClip handsAppear2;
    public AudioClip handsHurt;
    bool hasInitialized = false;

	// Use this for initialization
	void Start () {
       

	}
    public void HitWaterFall()
    {
        Variables.mainAudioSource.PlayOneShot(handsHurt);
        downDist = Random.Range(7f, 10f);
        transform.Translate(Vector3.up * -downDist);
        StartCoroutine("MoveUp");
    }
    void Reset()
    {
        transform.position = new Vector3(0, 0, 0);
    }
	void Update()
	{
        if (Variables.hasHands)
        {
            if (!hasInitialized)
            {
                transform.position = new Vector3(boatPos.position.x, 0.35f, boatPos.position.z);
                downDist = Random.Range(4f, 10f);
                transform.Translate(Vector3.up * -downDist);
                StartCoroutine("MoveUp");
                hasInitialized = true;
            }
            startPos = boatPos.position;
            transform.position = new Vector3(boatPos.position.x, transform.position.y, boatPos.position.z);
            transform.rotation = boatPos.rotation;
            if (AttackKey == KeyCode.E)
            {
                transform.rotation *= Quaternion.Euler(0, -90, 0);
            }
            if (AttackKey == KeyCode.Q)
            {
                transform.rotation *= Quaternion.Euler(0, 90, 0);
            }
            if (timerRunning)
            {
                reactionTime -= Time.deltaTime;
                if (reactionTime <= 0)
                {
                    Variables.mainAudioSource.PlayOneShot(handsAttack1);
                    Debug.Log("Hit by zombie hands");
                    Variables.health--;
                    StartCoroutine("MoveDown");
                }
                if (Input.GetKey(AttackKey))
                {
                    Variables.mainAudioSource.PlayOneShot(handsHurt);
                    Variables.mainAudioSource.PlayOneShot(handsAppear1);
                    StartCoroutine("MoveDown");
                }

            }


        }
        else
        {
            if (hasInitialized)
            {                // LeanTween.move(gameObject, transform.position - new Vector3(0, -5, 0), 2.0f).setOnComplete(Reset);
                transform.position = new Vector3(0, 0, 0);
                hasInitialized = false;
            }
           
        }


		
	}
    public void Descend()
    {
        StartCoroutine("MoveDown");
    }
	
	IEnumerator MoveUp()
	{
		while (transform.position.y <= startPos.y)
		{
            if (Time.deltaTime != 0.0f)
            {
                transform.Translate(Vector3.up * waitTime * moveSpeed);
                yield return new WaitForSeconds(waitTime);
            }
		}
		timerRunning = true;
        Variables.mainAudioSource.PlayOneShot(handsAppear1);
        Variables.mainAudioSource.PlayOneShot(handsAppear2);     
	}
	IEnumerator MoveDown()
	{
		downDist = Random.Range(2f, 4f);
		reactionTime = 3.0f;
		timerRunning = false;
		while (transform.position.y >= startPos.y-downDist)
		{
			transform.Translate(Vector3.up * -waitTime * moveSpeed);
			yield return new WaitForSeconds(waitTime);
		}
		StartCoroutine("MoveUp");
	}
	
}
