using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Klase kas kontrolē spēles varoņa kustību, bastoties uz spēlētāja ievadiem
public class PlayerController : MonoBehaviour {
	public float speed = 30f;
	public Rigidbody rb;
	public int health = 5;	
	private float rotationSpeed = 2f;
	public float floatForce = 120f;
	public float currMaxSpeed = 5f;
   
    float changeSpeed = 1.0f;
    public float currentSpeed;
	public Transform boatFront;
	public Transform boatBack;
	private float mass;
    public bool inHub = false;
    [HideInInspector]
    private float localRot = 0;   
    public AudioClip sailFlapSound;
    float angleDif;
    float angleDifLerped;
    public AudioSource waveSource;
    public GameObject playerYoung;
    public GameObject playerMiddleAge;
    public GameObject playerOld;
    GameObject playerChar = null;
    void Awake()
    {
       
       
    }
	
	// Use this for initialization
	void Start () {
       // Variables.playerStats.currentStage = character.ageStage.middleAge;
        

        //Used for testing hub area
        if(Variables.playerStats == null && SceneManager.GetActiveScene().name == "hub")
        {
           Variables.playerStats = new character(Helpers.GetName(true));
        }
        //Variables.playerStats.currentStage = character.ageStage.oldAge;
        SetPlayerModel();
        
        Variables.player = transform;        
        rb.AddForce(transform.forward*35000);        
		mass = rb.mass;             
        InvokeRepeating("PlaySailSound", 2.0f, 1.5f);
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("hub"))
        {
            InvokeRepeating("AgeChar", 7f, 7f);
        }
    }


    public void AgeChar()
    {
       
        Variables.playerStats.age++;
        character.ageStage prevAge = Variables.playerStats.currentStage;
        Variables.playerStats.SetAging();
        if (prevAge != Variables.playerStats.currentStage && Variables.playerStats.currentStage != character.ageStage.adulthood)
        {
            Helpers.ShowGUIText("Character has reached " + Variables.playerStats.currentStage, 3.5f);
            SetPlayerModel();
        }
        //Debug.Log("Player age: " + Variables.playerStats.age);
    }


    public void SetPlayerModel()
    {
        if (playerChar != null)
        {
            Destroy(playerChar);
        }
            switch (Variables.playerStats.currentStage)
        {

            case character.ageStage.adulthood:
                playerChar = Instantiate(playerYoung, transform.Find("playerPoint"), false);
                break;
            case character.ageStage.middleAge:
                playerChar = Instantiate(playerMiddleAge, transform.Find("playerPoint"), false);
                break;
            case character.ageStage.oldAge:
                playerChar = Instantiate(playerOld, transform.Find("playerPoint"), false);
                break;
        }
        if (playerChar != null)
        {

            Variables.shotPoint = playerChar.transform.Find("Armature/Bone/spearHand/Spear/Spear_end");
            if (Variables.shotPoint == null)
                Variables.shotPoint = playerChar.transform.Find("Armature/body/spearHand/Spear/Spear_end");

        }

    }



    public void PlaySailSound()
    {
        
        if (angleDif > 15)
        {
            Variables.mainAudioSource.PlayOneShot(sailFlapSound);
                    }
        
    }
    public void ChangeLocalRot(float rotToAdd)
    {
        localRot += rotToAdd;
    }

  
		void FixedUpdate ()
	{

        angleDif = Quaternion.Angle(transform.rotation, Quaternion.Euler(2, 90 + localRot, 0));
        angleDifLerped = Mathf.MoveTowards(angleDifLerped, angleDif, Time.deltaTime*10);
        float volume = Mathf.Min(Mathf.Abs(angleDifLerped), 30.0f) / 50;
        
        waveSource.volume = volume;


#if UNITY_ANDROID
        AndroidControl();
#endif
#if UNITY_IPHONE
        AndroidControl();
#endif

#if UNITY_STANDALONE
        WinControl();
#endif








        if (rb.velocity.magnitude < currMaxSpeed)
		{
            if (inHub)
            {
                rb.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z) * speed*Time.deltaTime*50);
            }
            else
            {
                
                rb.AddForce(new Vector3( transform.forward.x,0,transform.forward.z)* Time.deltaTime * speed * 50);
                
            }
		}
        Variables.PlayerSpeed = rb.velocity.magnitude;
		Stablize(transform.position,Variables.waterLevel);

		
   


	}

    private void AndroidControl()
    {
        currentSpeed = rb.velocity.magnitude;
        guiControl.score = (int)transform.position.x / 2;
        float turnVal = 0.0f;
        float turnForce = Input.acceleration.x;
        float turnCutoff = 0.05f;
        if (Input.acceleration.x > turnCutoff)
        {
            turnVal = -Time.deltaTime * Variables.turnSpeed;
            var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, transform.eulerAngles.y + 65 * (turnForce - turnCutoff), -18), Time.deltaTime * Variables.turnSpeed*3 * (turnForce - turnCutoff));
            rb.MoveRotation(rot);
        }
        else
        {
            if (Input.acceleration.x < -turnCutoff)
            {
                turnVal = Time.deltaTime * Variables.turnSpeed;
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, transform.eulerAngles.y + 65 * (turnForce + turnCutoff), 18), Time.deltaTime * Variables.turnSpeed *3 * -(turnForce + turnCutoff));
                rb.MoveRotation(rot);
            }
            else
            {
                Quaternion rot;
                if (!inHub)
                {
                    rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, 90 + localRot, 0), Time.deltaTime * Variables.turnSpeed / 5);

                }
                else
                {
                    rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, transform.eulerAngles.y, 0), Time.deltaTime * Variables.turnSpeed / 4);
                }
                rb.MoveRotation(rot);
            }
        }



    }

    private void WinControl()
    {
        currentSpeed = rb.velocity.magnitude;
        guiControl.score = (int)transform.position.x / 2;
        float turnVal = 0.0f;
        if (Input.GetKey(KeyCode.W) && currMaxSpeed < Variables.maxSpeed)
        {
            currMaxSpeed += Time.deltaTime*changeSpeed;
            rb.AddForce(new Vector3(transform.forward.x, 0, transform.forward.z) * Time.deltaTime * speed * 50);
        }
        else
        {

            if (Input.GetKey(KeyCode.S) && currMaxSpeed > Variables.minSpeed)
            {
                currMaxSpeed -= Time.deltaTime*changeSpeed;
                rb.AddForce(new Vector3(-transform.forward.x, 0, -transform.forward.z) * Time.deltaTime * speed * 20);
            }
            else
            {
                currMaxSpeed = Mathf.MoveTowards(currMaxSpeed, Variables.normMaxSpeed, Time.deltaTime * changeSpeed);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            turnVal = -Time.deltaTime * Variables.turnSpeed;
            var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, transform.eulerAngles.y + 20, -18), Time.deltaTime * Variables.turnSpeed / 2);
            rb.MoveRotation(rot);
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                turnVal = Time.deltaTime * Variables.turnSpeed;
                var rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, transform.eulerAngles.y - 20, 18), Time.deltaTime * Variables.turnSpeed / 2);
                rb.MoveRotation(rot);
            }
            else
            {
                Quaternion rot;
                if (!inHub)
                {
                    rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, 90 + localRot, 0), Time.deltaTime * Variables.turnSpeed / 5);

                }
                else
                {
                    rot = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(2, transform.eulerAngles.y, 0), Time.deltaTime * Variables.turnSpeed / 4);
                }
                rb.MoveRotation(rot);
            }
        }

    }
    private void Stablize(Vector3 pos,float waterLevel)
	{
		float floatVal=0;
		if (pos.y < waterLevel)
		{
			float diff = Mathf.Abs(pos.y - waterLevel)+0.1f;
			floatVal = Time.deltaTime * floatForce * 100 * diff*mass;
			
		}
		if (pos.y > waterLevel+0.2)
		{
			float diff = Mathf.Abs(boatFront.position.y - waterLevel+0.2f)+0.1f;
			floatVal = -Time.deltaTime * floatForce * 150 * diff*mass;
			
		}
		if (floatVal != 0)
		{
			rb.AddForce(new Vector3(0, floatVal, 0));
		}
	}
}
