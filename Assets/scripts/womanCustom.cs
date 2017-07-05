using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Klase, kas modificē sieviešu tēlu izskatu "village" ainā
public class womanCustom : MonoBehaviour
{
    public GameObject[] hairList;
    public Color[] hairColor;
    public Color[] eyeColor;
    public Color[] dressColor;
    public Color[] skinColor;
    public Transform hairPos;
    public Renderer renderer;
    // Use this for initialization
    void Start()
    {
        foreach (Material mat in renderer.materials)
        {
            if (mat.name == "Skirt (Instance)")
            {
                mat.color = dressColor[Random.Range(0, dressColor.Length - 1)];
            }
            if (mat.name == "EyeColor (Instance)")
            {
                mat.color = eyeColor[Random.Range(0, eyeColor.Length - 1)];
            }
            if (mat.name == "Skin (Instance)")
            {
                mat.color = skinColor[Random.Range(0, skinColor.Length - 1)];
            }
        }
        GameObject hair = hairList[Random.Range(0, hairList.Length - 1)];
        hair  = Instantiate(hair, hairPos, false);
        Renderer hairRenderer = hair.GetComponent<Renderer>();

        foreach (Material hairMat in hairRenderer.materials)
        {
            if(hairMat.name == "Material.002 (Instance)")
            {
                hairMat.color = hairColor[Random.Range(0, hairColor.Length - 1)];
            }
        }
        
    }
}