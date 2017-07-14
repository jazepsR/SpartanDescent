using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase, kas rada ūdens objektus un ar tiem saistītos šķēršļus
public class waterGen : MonoBehaviour {
	public GameObject[] waterSplit;
    public GameObject[] waterStraight;
    public GameObject[] waterLeft;
    public GameObject[] waterRight;
    public GameObject[] waterSbend;
    public GameObject[] waterTreasure;
    public GameObject[] waterIsland;
    public GameObject[] waterFall;
    public GameObject[] waterRealSplit;
    public GameObject FireTransition;
    public GameObject DesolateTransition;
    GameObject Gate;
    enum tiles { straight, fall,left, right,split}
    List<tiles> bannedDoubles =new List<tiles> { tiles.left, tiles.right, tiles.fall, tiles.split };
    tiles lastTile = tiles.straight;   
    List<Vector2> usedPositions = new List<Vector2>();
    Vector2 currentPos = new Vector2(0, 0); 
    List<spawnData> spawnDataList= new List<spawnData>();
    List<spawnData> nextSpawnDataList;    
	// Use this for initialization
	void Start ()
    {
        Gate = Resources.Load("prefabs/gate") as GameObject;
        Variables.distance = 0;
       // Variables.deathPositions.Add(3);
        //Variables.rotationY = 0;
        Variables.WaterGen = this;
        Variables.LevelDone = false;

        GameObject water = Instantiate(waterStraight[Variables.currentArea],new Vector3(0,0,2.5f),Quaternion.identity);
        spawnDataList.Add(new spawnData(false, water.GetComponent<waterScript>()));
        GenRandomWater();
        GenRandomWater();
        GenRandomWater();
    }
    public void GenRandomWater()
    {
        nextSpawnDataList = new List<spawnData>();
        foreach(spawnData data in spawnDataList)
        {
            if(!data.prevWater.ToDie)
                GenOneRandomWater(data);
        }
        spawnDataList = nextSpawnDataList;
    }
    public void GenOneRandomWater(spawnData data ,int timeRan = 0)
    {
        if (Variables.LevelDone)
            return;

        if (timeRan > 10)
        {
            Debug.Log("Failed to find usable land piece");
            return;
        }
        int rand = Random.Range(0, 12);

        if(Variables.distance >= Variables.levelLength && Variables.currentLVL == Variables.levels.item)
        {
            Variables.LevelDone = true;
            GenWater(data,waterTreasure[Variables.currentArea]);
            return;
        }
        //TODO: figure out this stuff for split? Maybe spawns in both?
        if(Variables.distance == Variables.FireTreshold)
        {
            GenWater(data, FireTransition);
            Variables.currentArea = 1;
            return;
        }
        if (Variables.distance == Variables.LonelyTreshold)
        {
            GenWater(data, DesolateTransition);
            Variables.currentArea = 2;
            return;
        }



        switch (rand)
        {
            case 0:
                if (CheckMapPos(tiles.straight))
                {
                    GenOneRandomWater(data,++timeRan);
                }
                else
                {
                    GenWater(data, waterSbend[Variables.currentArea]);
                    lastTile = tiles.straight;                  
                }
                break;
            case 1:
                if (CheckMapPos(tiles.right))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                { 
                GenWater(data, waterRight[Variables.currentArea]);
                lastTile = tiles.right;
                }
                break;
            case 2:
                if(CheckMapPos(tiles.left))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                {
                    GenWater(data, waterLeft[Variables.currentArea]);
                    lastTile = tiles.left;
                }
                break;
            case 3:
                if (CheckMapPos(tiles.split))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                {
                    GenWater(data, waterSplit[Variables.currentArea]);
                    lastTile = tiles.split;                   
                }
                break;
            case 4:
                if (CheckMapPos(tiles.straight))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                {
                    GenWater(data, waterIsland[Variables.currentArea]);
                    lastTile = tiles.straight;                    
                }
                break;
            case 5:
                if (CheckMapPos(tiles.fall))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                {
                    GenWater(data, waterFall[Variables.currentArea]);
                    lastTile = tiles.fall;                   
                }
                break;
            case 6:
                if (CheckMapPos(tiles.split))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                {
                    GenWater(data, waterRealSplit[Variables.currentArea]);
                    lastTile = tiles.split;                   
                }
                break;
            default:
                if (CheckMapPos(tiles.straight))
                {
                    GenOneRandomWater(data, ++timeRan);
                }
                else
                {


                    GenWater(data, waterStraight[Variables.currentArea]);
                    lastTile = tiles.straight;                
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
    


    void GenWater(spawnData data,GameObject water)
    {
                    
        var rot = Quaternion.Euler(new Vector3(0, data.prevWater.lastTileRot, 0));
        Vector3 spawnloc;
        if (data.forLeft)
            spawnloc = data.prevWater.joinPointL.position;
        else
            spawnloc = data.prevWater.joinPoint.position;
        GameObject instantiatedWater = Instantiate(water, spawnloc,rot);
        waterScript script = instantiatedWater.GetComponent<waterScript>();      
        script.lastTileRot = data.prevWater.lastTileRot + script.rotation;
        script.spawnedL = data.forLeft;
        script.PrevWater = data.prevWater;
        if (data.forLeft)
            data.prevWater.nextWater2 = script;
        else
            data.prevWater.nextWater1 = script;
        nextSpawnDataList.Add(new spawnData(false,script));
        if (script.isSplit)
        {
            nextSpawnDataList.Add(new spawnData(true, script));
        }



        //prevWaterScript = instantiatedWater.GetComponent<waterScript>();

        //if(prevWaterScript.joinPointL != null)
        {
           // spawnpointL = prevWaterScript.joinPointL.position;
           // GenWater(waterLeft[Variables.currentArea], false);
           // GenWater(waterRight[Variables.currentArea], true);
            
        }




        if (Variables.deathPositions.Contains(Variables.distance) && Variables.hasGhosts)
        {
            script.SpawnBrokenBoat();
        }

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
    }
}



public class spawnData
{
    public bool forLeft;
    public waterScript prevWater;
    public spawnData(bool forLeft, waterScript prevWater)
    {
        this.forLeft = forLeft;
        this.prevWater = prevWater;
    }


}