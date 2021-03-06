﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
// Klase, kas kontrolē ūdens objektus un izveido šķēršļus, kas atrodas uz šī ūdens objekta.
public class waterScript : MonoBehaviour {

	// Use this for initialization
	public Transform back;
	public Transform frontL;
	public Transform frontR;
	public Transform joinPoint;
    public Transform joinPointL;
	public Transform GatePoint;
    public Transform panPoint;
	Vector3[] vertices;
	private List<bool> directions = new List<bool>();
	private List<float> orgPos = new List<float>();
	private List<int> backList = new List<int>();
	private List<int> frontListL = new List<int>();
	private List<int> frontListR = new List<int>();
    public bool isSplit = false;
	public List<Vector4> backVectors = new List<Vector4>(), frontVectorsR = new List<Vector4>(), frontVectorsL = new List<Vector4>();
	[HideInInspector]
	public waterScript PrevWater;
    [HideInInspector]
    public bool spawnedL = false;
    //[HideInInspector]
    public waterScript nextWater1 = null;
    public waterScript nextWater2 = null;
	public bool FlipZ = false;
	public float waveSpeed = 1f;
	public float amplitude = 0.1f;
    [HideInInspector]
    public float lastTileRot = 0.0f;
	public Variables.dir sortDir = Variables.dir.z;
	public float rotation = 0;
	public Transform brokenBoatPos;
	GameObject barrel;
    GameObject wildfire;
    GameObject barrel2;
    GameObject turtle;
    GameObject amphora;
    GameObject wolf;
    GameObject pan;
	float wolfFreq = 0.4f;
	Mesh mesh;
	GameObject rock;
    GameObject quiver;
    public bool ToDie = false;
	void Start () {
        barrel = Variables.barrel;
        amphora = Variables.amphora;
        turtle = Variables.turtle;
        quiver = Variables.quiver;
        wildfire = Variables.wildfire;
        barrel2 = Variables.barrel2;
        pan = Variables.pan;





        mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		try
		{
            if (Time.timeSinceLevelLoad > 1.0f)
            {
                if (Variables.currentArea == 0)
                {
                    rock = Resources.Load("prefabs/rock") as GameObject;
                }
                if (Variables.currentArea == 1)
                {
                    rock = Resources.Load("prefabs/fireRock") as GameObject;
                }
                if (Variables.currentArea == 2)
                {
                    rock = Resources.Load("prefabs/desolateRock") as GameObject;
                }
                for (int i = 0; i < 1 + (int)Variables.distance / 10; i++)
                {
                    Vector3 rockPoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                    GameObject rockObj = Instantiate(rock, rockPoint, UnityEngine.Random.rotation);
                    rockObj.transform.SetParent(transform, true);
                    if (Variables.hasBarrel)
                    {
                        Vector3 barrelPoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                        GameObject barrelObj = Instantiate(barrel, new Vector3(barrelPoint.x, barrelPoint.y + 0.05f, barrelPoint.z), UnityEngine.Random.rotation);
                        barrelObj.transform.SetParent(transform, true);
                    }
                    if (Variables.hasWildfire)
                    {
                        Vector3 wildfirePoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                        GameObject wildfireObj = Instantiate(wildfire, new Vector3(wildfirePoint.x, wildfirePoint.y - 0.1f, wildfirePoint.z), Quaternion.identity);
                        wildfireObj.transform.SetParent(transform, true);
                    }
                    if (Variables.hasBarrel2)
                    {
                        Vector3 barrel2Point = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                        GameObject barrel2Obj = Instantiate(barrel2, new Vector3(barrel2Point.x, barrel2Point.y+0.05f, barrel2Point.z), UnityEngine.Random.rotation);
                        barrel2Obj.transform.SetParent(transform, true);
                    }
                }

                if (UnityEngine.Random.Range(0.0f, 1f) > 0.1f)
                {

                    Vector3 quiverPoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                    GameObject quiverObj = Instantiate(quiver, new Vector3(quiverPoint.x,quiverPoint.y+0.5f,quiverPoint.z), Quaternion.Euler(20,UnityEngine.Random.Range(0,180),0));
                    quiverObj.transform.SetParent(transform, true);
                }
                if (UnityEngine.Random.Range(0.0f, 1f) < 0.2f && UnityEngine.Random.Range(0.0f, 1f) > 0.1f)
                {

                    Vector3 turtlePoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                    GameObject turtleObj = Instantiate(turtle, new Vector3(turtlePoint.x, turtlePoint.y , turtlePoint.z), Quaternion.Euler(0, 0, 0));
                    turtleObj.transform.SetParent(transform, true);
                }
                if (UnityEngine.Random.Range(0.0f, 1f) < 0.3f && UnityEngine.Random.Range(0.0f, 1f) > 0.2f)
                {
                    Vector3 amphoraPoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                    GameObject amphoraObj = Instantiate(amphora, new Vector3(amphoraPoint.x, amphoraPoint.y+0.15f, amphoraPoint.z), Quaternion.Euler(0, UnityEngine.Random.Range(0,360), 25f));
                    amphoraObj.transform.SetParent(transform, true);
                }
                if (UnityEngine.Random.Range(0.0f, 1f) < 0.5f && UnityEngine.Random.Range(0.0f, 1f) > 0.3f)
                {
                    // print("doing pan");
                    GameObject panObj = Instantiate(pan, new Vector3(panPoint.position.x, panPoint.position.y + 1.45f, panPoint.position.z), Quaternion.Euler(0f, 50f, 0f));
                    panObj.transform.SetParent(transform, true);
                }
                if (Variables.hasWolves && UnityEngine.Random.Range(0.0f, 1f) > (1 - wolfFreq) && name.Contains("Straight"))
                {
                    wolf = Resources.Load("prefabs/wolf") as GameObject;
                    //Instantiate(wolf, brokenBoatPos, false);
                    Instantiate(wolf, brokenBoatPos, false);
                }
            }
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Asset import error: " + ex);
		}
		try { 
		for(int i =0;i<vertices.Length;i++)
		{
			//Add vertices and their original positions
			directions.Add(true);
			orgPos.Add(vertices[i].y);

			//Figure out if the vertice is in the first or last row

			//First row
			if (vertices[i].x < back.localPosition.x)
			{
				backVectors.Add(new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, i));               
			}

				//Last row, left side (or default)
				if (FlipZ)
				{
					if (vertices[i].x > frontL.localPosition.x && vertices[i].z < frontL.localPosition.z)
					{
						frontVectorsL.Add(new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, i));                        
					}
				}
				else
				{
					if (vertices[i].x > frontL.localPosition.x && vertices[i].z > frontL.localPosition.z)
					{
						frontVectorsL.Add(new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, i));                       
					}
				}
			
			//Last row, right side (optional)
			if ( frontR!= null && vertices[i].x > frontR.localPosition.x && vertices[i].z < frontR.localPosition.z)
			{
				frontVectorsR.Add(new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, i));                
			}

		}

			//Sorting the first and last vector lists
			backVectors = SortList(backVectors, Variables.dir.z);
			frontVectorsL = SortList(frontVectorsL, sortDir);
			frontVectorsR = SortList(frontVectorsR, sortDir);

			//Adding the sorted indexes to the index lists.
			foreach(Vector4 vec in backVectors)            
				backList.Add((int)vec.w);

			foreach (Vector4 vec in frontVectorsL)
				frontListL.Add((int)vec.w);

			foreach (Vector4 vec in frontVectorsR)
				frontListR.Add((int)vec.w);



		}
		catch(System.Exception ex)
		{
			Debug.Log("fail in waterScript line 88: " +ex.Message);
		}

	}


    public void Destroy(float timeToLive)
    {
        ToDie = true;
        if (nextWater1 != null)
            nextWater1.Destroy(2f);
        if (nextWater2 != null)
            nextWater2.Destroy(2f);
        Destroy(gameObject, timeToLive);
    }
	public void SpawnBrokenBoat()
	{
        if (Time.timeSinceLevelLoad > 1.0f)
        {
            GameObject brokenBoat = Resources.Load("prefabs/brokenBoat") as GameObject;
            Instantiate(brokenBoat, brokenBoatPos, false);
        }
	}
	// Update is called once per frame
	void Update ()
	{
		int index = 0;
		int prevWaterIndex = 0;
		foreach (Vector3 pos in vertices)
		{
			
			//TODO: fix orgpos bug-> some vertexes become stationary.
			//Add frontListL for testing
			if (!backList.Contains(index))// && !frontListL.Contains(index)) //&& !frontListR.Contains(index))
		   {



				float y = pos.y;
                float org = 0.0f;
                try
                {
                    org = (float)orgPos[index];
                }
                catch
                {
                    Debug.Log("cant assign orgPos");
                }
				if(UnityEngine.Random.Range(0, 1) > 0.2f)
				{
					directions[index] = !directions[index];
				}
				if (directions[index])
				{
                    y = Time.deltaTime * UnityEngine.Random.Range(0.0f, waveSpeed) + y;                
					if (y >=org+ amplitude)
						directions[index] = false;
				}
				else
				{
                    y = -Time.deltaTime * UnityEngine.Random.Range(0.0f, waveSpeed) + y;
					if (y <= org-amplitude)
						directions[index] = true;
				}
				vertices[index] = new Vector3(pos.x, y, pos.z);


				if (frontListL.Contains(index))
				{
					int valIndex = frontListL.IndexOf(index);
					Vector3 vec = transform.TransformPoint(vertices[index]);
					frontVectorsL[valIndex] = new Vector4(vec.x,vec.y,vec.z,frontVectorsL[valIndex].w);
				}
                if (frontListR.Contains(index))
                {
                    int valIndex = frontListR.IndexOf(index);
                    Vector3 vec = transform.TransformPoint(vertices[index]);
                    frontVectorsR[valIndex] = new Vector4(vec.x, vec.y, vec.z, frontVectorsR[valIndex].w);
                }
            }
			else
			{
                if (PrevWater != null)
                {
                    if (spawnedL)
                    {
                        vertices[(int)backVectors[prevWaterIndex].w] = transform.InverseTransformPoint(PrevWater.frontVectorsR[prevWaterIndex]);
                        prevWaterIndex++;
                    }
                    else
                    {
                        vertices[(int)backVectors[prevWaterIndex].w] = transform.InverseTransformPoint(PrevWater.frontVectorsL[prevWaterIndex]);
                        prevWaterIndex++;
                    }
                }


			}
			index++;
		}

			   
		mesh.vertices = vertices;
	}
	List<Vector4> SortList(List<Vector4> list, Variables.dir sortBy)
	{
		if (sortBy == Variables.dir.z)       
			list.Sort((a, b) => a.z.CompareTo(b.z));        
		if (sortBy == Variables.dir.zNeg)
			list.Sort((a, b) => b.z.CompareTo(a.z));         
		if (sortBy == Variables.dir.x)
			list.Sort((a, b) => a.x.CompareTo(b.x));      
		if (sortBy == Variables.dir.xNeg)
			list.Sort((a, b) => b.x.CompareTo(a.x));
		return list;       
	}
}
