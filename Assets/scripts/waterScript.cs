using System.Collections;
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
	public Transform GatePoint;

	Vector3[] vertices;
	private List<bool> directions = new List<bool>();
	private List<float> orgPos = new List<float>();
	private List<int> backList = new List<int>();
	private List<int> frontListL = new List<int>();
	private List<int> frontListR = new List<int>();
	
	public List<Vector4> backVectors = new List<Vector4>(), frontVectorsR = new List<Vector4>(), frontVectorsL = new List<Vector4>();
	[HideInInspector]
	public waterScript PrevWater;
	public bool FlipZ = false;
	public float waveSpeed = 1f;
	public float amplitude = 0.1f;
	public Variables.dir sortDir = Variables.dir.z;
	public float rotation = 0;
	public Transform brokenBoatPos;
	public GameObject barrel;
   GameObject wolf;
	float wolfFreq = 0.4f;
	Mesh mesh;
	GameObject rock;
    GameObject quiver;
	
	void Start () {
        quiver = Resources.Load("prefabs/quiver") as GameObject;
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
                        GameObject barrelObj =Instantiate(barrel, new Vector3(barrelPoint.x, barrelPoint.y + 0.1f, barrelPoint.z), UnityEngine.Random.rotation);
                        barrelObj.transform.SetParent(transform, true);
                    }

                }

                if (UnityEngine.Random.Range(0.0f, 1f) > 0.1f)
                {

                    Vector3 quiverPoint = transform.TransformPoint(vertices[UnityEngine.Random.Range(0, vertices.Length - 1)]);
                    GameObject quiverObj = Instantiate(quiver, new Vector3(quiverPoint.x,quiverPoint.y+0.5f,quiverPoint.z), Quaternion.Euler(20,UnityEngine.Random.Range(0,180),0));
                    quiverObj.transform.SetParent(transform, true);
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
			if ( frontR!= null && vertices[i].x < frontR.localPosition.x && vertices[i].z > frontR.localPosition.z)
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
			}
			else
			{
				if (PrevWater != null)
				{
					vertices[(int)backVectors[prevWaterIndex].w] = transform.InverseTransformPoint( PrevWater.frontVectorsL[prevWaterIndex]);
					prevWaterIndex++;
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
