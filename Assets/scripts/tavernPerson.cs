using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Klase, kas glabā sieviešu tēlu datus un kontrolē to uzvedību ainā "village"
public class tavernPerson : MonoBehaviour {
    [HideInInspector]
    public static tavernPerson selectedPerson;
    [HideInInspector]
    public character ch;
    [HideInInspector]
    public string charName;
    public AudioClip interested;
    public AudioClip interested2;
    
   // public GameObject infoPanel;
    Animator anim;
    public Text marrigeChanceText;
    int hoverBool = Animator.StringToHash("Hover");
    int YesMarTrigger = Animator.StringToHash("SelectedYes");
    int NoMarTrigger = Animator.StringToHash("SelectedNo");
    int IdleSpeed1 = Animator.StringToHash("IdleSpeed1");
    int IdleSpeed2 = Animator.StringToHash("IdleSpeed2");
    public Text infoText;
    public float marrigeChance;
	// Use this for initialization
	void Start () {
        InvokeRepeating("ChangeIdleSpeeds", 0.01f, 1.5f);
        charName = Helpers.GetName(false);
        ch = new character(charName);
        SetTexts();
        anim = GetComponent<Animator>();
        
	}
    void ChangeIdleSpeeds()
    {
        anim.SetFloat(IdleSpeed1, Random.Range(0.4f, 1.6f));
        anim.SetFloat(IdleSpeed2, Random.Range(0.4f, 1.6f));
    }
    public void SetTexts()
    {
        marrigeChance = Mathf.InverseLerp(0f, 20f, Mathf.Max(10 + Variables.playerStats.beauty - ch.beauty, 0.0f));
        infoText.text = charName + "\n";
        marrigeChanceText.text= (marrigeChance * 100).ToString("F0") + "%";
        if(marrigeChance>= 0.7f)
        {
            marrigeChanceText.color = new Color(0, 1, 0);

        }
        if(marrigeChance<= 0.3f)
        {
            marrigeChanceText.color = new Color(1, 0, 0);
        }
        if(marrigeChance>0.3f && marrigeChance < 0.7f)
        {
            marrigeChanceText.color = new Color(1, 1, 0);
        }


        infoText.text += "Beauty: " + ch.beauty.ToString() + "\n";
        foreach (string activeTrait in ch.activeTraits)
        {            
            infoText.text += '\u2022' + " " + activeTrait + "\n";
        }
    }

	void OnMouseEnter()
    {
        SetTexts();
       // infoPanel.SetActive(true);
        Debug.Log("Mouse on " + charName);
    }
    void OnMouseExit()
    {

        //infoPanel.SetActive(false);
        //outline.SetActive(false);
        
        Debug.Log("Mouse off " + charName);
        
    }
    public void YesMarrige()
    {
        anim.SetTrigger(YesMarTrigger);
    }
    public void NoMarrige()
    {
        anim.SetTrigger(NoMarTrigger);
        Destroy(gameObject, 1.417f);
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (GameObject outlineObj in GameObject.FindGameObjectsWithTag("outline"))
            {
                outlineObj.SetActive(false);
                

            }
            selectedPerson = this;
            if(Helpers.RandomBool())
                Variables.mainAudioSource.PlayOneShot(interested);
            else
                Variables.mainAudioSource.PlayOneShot(interested2);
            anim.SetBool(hoverBool,true);
        }
    }
	// Update is called once per frame
	void Update () {
		if(tavernPerson.selectedPerson!= this)
        {
            anim.SetBool(hoverBool, false);
        }
	}
}
