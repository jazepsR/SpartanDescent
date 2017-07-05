using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase kas veido ūdens viļņošanās efektu un savieno viena ūdens objekta malas punktus ar nākamā ūdens objekta punktiem
public class meshGen : MonoBehaviour {

	private List<Vector3> vertices = new List<Vector3>();
	private List<int> triangles = new List<int>();
	private List<bool> directions = new List<bool>();
    public GameObject brokenBoat;
    public GameObject terrainTurn;
    public GameObject player;
    public GameObject terrain;
	private int index = 0;
    private int iter = 0;
	public int length;
	public int width;
    public int turnLength = 30;
	public float waveSpeed = 0.15f;
	public float amplitude =0.2f;
    private int terrainCount = 0;
    MeshRenderer renderer;
    Mesh mesh;

    
	void Awake()

	{
        Variables.player = player.transform;     

        }
	




	
    void AddTurn()
    {
        
    }
    void AddPoints(int iteration)
    {
        int iterVal = length * width * iter;
        //Add vertices
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {               
                vertices.Add(new Vector3(i+length*iter, Random.Range(-amplitude, amplitude), j));                
            }

        }
        //Add Wave direction booleans to array
        for (int i = 0; i < length * width; i++)
            directions.Add(true);
        //Add triangles to start
        for (int i = 0; i < width - 1; i++)
        {
            triangles.Add(i+iterVal);
            triangles.Add(i + 1 + iterVal);
            triangles.Add(width + i + iterVal);

            triangles.Add(width * (length - 2) + i + 1 + iterVal);
            triangles.Add(width * (length - 1) + i + 1 + iterVal);
            triangles.Add(width * (length - 1) + i + iterVal);
        }
        
       if (iteration!= 0)
        {
            for(int i = 0; i < width-1; i++)
            {
                triangles.Add(i + iterVal);
                triangles.Add(i + iterVal - width);                
                triangles.Add(i + 1 + iterVal);
                
                triangles.Add(i + iterVal - width + 1);
                triangles.Add(i + iterVal + 1);
                triangles.Add(i + iterVal - width);
                
                
            }

        }

        //Add main triangles
        for (int i = 1; i < length - 1; i++)
        {
            for (int j = 0; j < width - 1; j++)
            {
                triangles.Add(width * i + j + iterVal);
                triangles.Add(width * (i - 1) + j + 1 + iterVal);
                triangles.Add(width * i + j + 1 + iterVal);


                triangles.Add(width * i + j + iterVal);
                triangles.Add(width * i + j + 1 + iterVal);
                triangles.Add(width * (i + 1) + j + iterVal);
            }
        }
        
        try
        {
            mesh.Clear();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();            
        }
        catch { };
        iter++;
    }

    void Update () {
        if (player.transform.position.x > length * (iter-1) - 5)
        {
            CreateTerrain();
            AddPoints(iter);
        }
        
		index = 0;
		float y;
		foreach ( Vector3 pos in vertices)
		{
		   y = pos.y;
			if (directions[index])
			{
				y = Mathf.Min(Time.deltaTime * Random.Range(0.0f, waveSpeed) + y, amplitude);
                
				if (y >= amplitude)
					directions[index] = false;
			}
            else
			{
                y = Mathf.Max(-Time.deltaTime * Random.Range(0.0f,waveSpeed) + y ,  -amplitude);
                if (y <= -amplitude)
                    directions[index] = true;
            }
            vertices[index] = new Vector3(pos.x, y, pos.z);
            index++;

        }
        mesh.vertices = vertices.ToArray();  
	}
    void CreateTerrain()
    {
        Vector3 pos = terrain.transform.position;
        terrainCount++;
        if (terrainCount >= 3)
        {
            Instantiate(terrainTurn, new Vector3(pos.x + (length) * iter, pos.y, pos.z), Quaternion.identity);
            terrainCount = 0;
        }
        else
        {
            Instantiate(terrain, new Vector3(pos.x + length * iter, pos.y, pos.z), Quaternion.identity);
            foreach (float position in Variables.deathPositions)
            {
                if (position > pos.x + length * iter && position < pos.x + length * (iter + 1))
                {
                  //  Instantiate(brokenBoat, new Vector3(position, 1.37f, 0.34f), brokenBoat.transform.rotation);
                }
            }
        }
    }
	Mesh CreateMesh(float width, float height)
	{
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = new Vector3[] {
		 new Vector3(-width, -height, 0.01f),
		 new Vector3(width, -height, 0.01f),
		 new Vector3(width, height, 0.01f),
		 new Vector3(-width, height, 0.01f)
	 };
		m.uv = new Vector2[] {
		 new Vector2 (0, 0),
		 new Vector2 (0, 1),
		 new Vector2(1, 1),
		 new Vector2 (1, 0)
	 };
		m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
		m.RecalculateNormals();

		return m;
	}
}
