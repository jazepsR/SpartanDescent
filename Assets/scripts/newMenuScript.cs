using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newMenuScript : MonoBehaviour {
    public Text spearText;
    int spearCount = 12;
    int age = 20;
    public Image ageDisplay;
    public Image SpeedDial;
    public Text AgeText;
    public GameObject[] portraits;
    public Color[] ageColors;
    // Use this for initialization
    void Start() {
        ageDisplay.fillAmount = getAgeBarPercent();
        spearText.text = Variables.spearCount.ToString();
        Debug.Log(Variables.playerStats.middleAge+" , "+ Variables.playerStats.oldAge+", "+ Variables.playerStats.deathAge);
    }

    // Update is called once per frame
    void Update() {


        // SpeedDial.rectTransform.localRotation =Quaternion.RotateTowards(SpeedDial.rectTransform.localRotation, Quaternion.Euler(0, 0, GetSpeedAngle()),Time.deltaTime*13f);
        SpeedDial.rectTransform.localRotation = Quaternion.RotateTowards(SpeedDial.rectTransform.localRotation,Quaternion.Euler(0, 0, GetSpeedAngle()),Time.deltaTime*20.0f);
        if (spearCount != Variables.spearCount)
        {
            spearText.text = Variables.spearCount.ToString();
        }
        spearCount = Variables.spearCount;

        if (age != Variables.playerStats.age)
        {
            AgeText.text = Variables.playerStats.age.ToString();
            StartCoroutine(BlinkText(0.5f,AgeText));
            ageDisplay.fillAmount = getAgeBarPercent();
            int colIndex = (int)Variables.playerStats.currentStage;
            ageDisplay.color = Color.Lerp(ageColors[colIndex], ageColors[colIndex + 1], 4*(getAgeBarPercent()%0.25f));
            foreach (GameObject img in portraits)
            {

                if (img.name == Variables.playerStats.currentStage.ToString())
                    img.SetActive(true);
                else
                    img.SetActive(false);
            }
        }


        age = Variables.playerStats.age;
    }
    public IEnumerator BlinkText(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

  


    float getAgeBarPercent()
    {
        switch (Variables.playerStats.currentStage)
        {
            case character.ageStage.adulthood:
                return 0.25f+0.25f*(float)(age -20) / (Variables.playerStats.middleAge-20);
            case character.ageStage.middleAge:
                return 0.5f + 0.25f * (float)(age - Variables.playerStats.middleAge) / (Variables.playerStats.oldAge - Variables.playerStats.middleAge);
            case character.ageStage.oldAge:
                return 0.75f + 0.25f * (float)(age - Variables.playerStats.oldAge) / (Variables.playerStats.deathAge - Variables.playerStats.oldAge);
            case character.ageStage.dead:
                return 1.0f;
            default:
                return 1.0f;
               
        }
    }
    float GetSpeedAngle()
    {
        float SpeedAngle = (Variables.PlayerSpeed - Variables.normMaxSpeed);
        if (SpeedAngle > 0)
        {
            SpeedAngle = Mathf.Min(1f,SpeedAngle / (Variables.maxSpeed - Variables.normMaxSpeed));
        }
        else
        {
            SpeedAngle = Mathf.Max(-1f,SpeedAngle / (Variables.normMaxSpeed - Variables.minSpeed));
        }
       // Debug.Log(SpeedAngle);
        SpeedAngle *= -35f;
        return SpeedAngle;
    }
 }
