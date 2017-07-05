using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase, kas rada ūdens objektus un ar tiem saistītos šķēršļus
public class waterGen : MonoBehaviour {
	public GameObject waterSplit;
    public GameObject waterStraight;
    public GameObject waterLeft;
    public GameObject waterRight;
    public GameObject waterSbend;
    public GameObject waterTreasure;
    public GameObject waterIsland;
    public GameObject waterFall;
    GameObject Gate;
    enum tiles { straight, fall,left, right,split}
    List<tiles> bannedDoubles =new List<tiles> { tiles.left, tiles.right, tiles.fall, tiles.split };
    tiles lastTile = tiles.straight;
    private waterScript prevWaterScript;
    List<Vector2> usedPositions = new List<Vector2>();
    Vector2 currentPos = new Vector2(0, 0);
    private Vector3 spawnpoint= new Vector3(0,0,2.5f);
	// Use this for initialization
	void Start ()
    {
        Gate = Resources.Load("prefabs/gate") as GameObject;
        Variables.distance = 0;
       // Variables.deathPositions.Add(3);
        Variables.rotationY = 0;
        Variables.WaterGen = this;
        Variables.LevelDone = false;

        //Variables.levelLength = 3;

        GenWater(waterStraight);
        GenWater(waterStraight);
        GenWater(waterStraight);
        GenRandomWater();
        GenRandomWater();

        /*for (int i = 0; i < 50; i++)
        {
            GenRandomWater();
        }

        //Test all bits
        
        GenWater(waterLeft);
        GenWater(waterLeft);
        GenWater(waterRight);
        GenWater(waterLeft);
        GenWater(waterSbend);
        GenWater(waterLeft);
        GenWater(waterStraight);
        GenWater(waterLeft);
        GenWater(waterRight);
        GenWater(waterRight);
        GenWater(waterStraight);
        GenWater(waterRight);
        GenWater(waterSbend);
        GenWater(waterRight);
        GenWater(waterSbend);
        GenWater(waterSbend);
        GenWater(waterStraight);
        GenWater(waterSbend);
        GenWater(waterStraight);
        GenWater(waterStraight);
        */
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GenRandomWater(int timeRan=0)
    {
        if (Variables.LevelDone)
            return;

        if (timeRan > 10)
        {
            Debug.Log("Failed to find usable land piece");
            return;
        }
        int rand = Random.Range(0, 10);

        if(Variables.distance >= Variables.levelLength && Variables.currentLVL == Variables.levels.item)
        {
            Variables.LevelDone = true;
            GenWater(waterTreasure);
            return;
        }
       



        switch (rand)
        {
            case 0:
                if (CheckMapPos(tiles.straight))
                {
                    GenRandomWater(++timeRan);
                }
                else
                {
                    GenWater(waterSbend);
                    lastTile = tiles.straight;
                   // UpdatePosMap();
                }
                break;
            case 1:
                if (CheckMapPos(tiles.right))
                {
                    GenRandomWater(++timeRan);
                }
                else
                { 
                GenWater(waterRight);
                lastTile = tiles.right;
                }
                break;
            case 2:
                if(CheckMapPos(tiles.left))
                {
                    GenRandomWater(++timeRan);
                }
                else
                {
                    GenWater(waterLeft);
                    lastTile = tiles.left;
                }
                break;
            case 3:
                if (CheckMapPos(tiles.split))
                {
                    GenRandomWater(++timeRan);
                }
                else
                {
                    GenWater(waterSplit);
                    lastTile = tiles.split;                   
                }
                break;
            case 4:
                if (CheckMapPos(tiles.straight))
                {
                    GenRandomWater(++timeRan);
                }
                else
                {
                    GenWater(waterIsland);
                    lastTile = tiles.straight;                    
                }
                break;
            case 5:
                if (CheckMapPos(tiles.fall))
                {
                    GenRandomWater(++timeRan);
                }
                else
                {
                    GenWater(waterFall);
                    lastTile = tiles.fall;
                    // UpdatePosMap();
                }
                break;
            default:
                if (CheckMapPos(tiles.straight))
                {
                    GenRandomWater(++timeRan);
                }
                else
                {


                    GenWater(waterStraight);
                    lastTile = tiles.straight;
                    //  UpdatePosMap();
                }
                break;
        }


    }
    bool CheckMapPos(tiles tileType)
    {
        if (bannedDoubles.Contains(tileType) && tileType == lastTile)
            return true;
        else
            return false;
    }


    /*void UpdatePosMap()
    {
        usedPositions.Add(currentPos);
        currentPos = Helpers.CalculatePos(currentPos, Variables.rotationY);
    }*/


    void GenWater(GameObject water)
    {
        Variables.distance++;

       
        
        

        var rot = Quaternion.Euler(new Vector3(0, Variables.rotationY, 0));
        GameObject instantiatedWater = Instantiate(water, spawnpoint,rot);
        waterScript script = instantiatedWater.GetComponent<waterScript>();
        Variables.rotationY += script.rotation;

        if (Variables.distance >= Variables.levelLength && water.Equals(waterStraight) && Variables.currentLVL != Variables.levels.item)
        {
            try
            {
                Instantiate(Gate, script.GatePoint.position, rot);
                Variables.LevelDone = true;
            }
            catch
            {
                Debug.Log("Failed gate");
            }
            Debug.Log("Level done");
        }

        if (Variables.deathPositions.Contains(Variables.distance) && Variables.hasGhosts)
        {
            script.SpawnBrokenBoat();
        }


        if (prevWaterScript != null)
        {
            instantiatedWater.GetComponent<waterScript>().PrevWater = prevWaterScript;
        }
        prevWaterScript = instantiatedWater.GetComponent<waterScript>();
        spawnpoint = prevWaterScript.joinPoint.position;
    }
}
