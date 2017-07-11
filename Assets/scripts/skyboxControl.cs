using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxControl : MonoBehaviour {
    Material firstSkybox;
    Material secondSkybox;
    Color fogColor;
    public Color FireFogColor;
    public Color LonelyFogColor;
    public static skyboxControl Instance { get; private set; }
    // Use this for initialization
    void Start()
    {
        Instance = this;
        fogColor = RenderSettings.fogColor;
        firstSkybox = (Material)Resources.Load("NormalToFireSkybox", typeof(Material));
        secondSkybox = (Material)Resources.Load("FireToDesolateSkybox", typeof(Material));
        firstSkybox.SetFloat("_Blend", 0.0f);
        secondSkybox.SetFloat("_Blend", 0.0f);
       
    }
    public void ChangeSkybox(int area)
    {
        if(area == 1)
            StartCoroutine("ChangeFistSkybox");
        else
            StartCoroutine("ChangeSecondSkybox");
        

    }

    IEnumerator ChangeFistSkybox()
    {

        for (int i = 0; i <= 100; i++)
        {
            float val = (float)i / 100.0f;
            firstSkybox.SetFloat("_Blend",val);
            yield return new WaitForSeconds(0.04f);
            RenderSettings.fogColor = fogColor * (1.0f - val) + FireFogColor * val;
        }
        
        RenderSettings.skybox = secondSkybox;
        yield return null;

    }

    IEnumerator ChangeSecondSkybox()
    {

        for (int i = 0; i <= 100; i++)
        {
            float val = (float)i / 100.0f;
            secondSkybox.SetFloat("_Blend", val);
            RenderSettings.fogColor = FireFogColor * (1.0f - val) + LonelyFogColor* val;
            yield return new WaitForSeconds(0.04f);
        }        
        yield return null;

    }

}
