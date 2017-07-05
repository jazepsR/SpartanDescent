using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Klase, kas generē objektus spēles vidē
public class spawnScript : MonoBehaviour
{
    public GameObject player;
    int iter = 0;
    public GameObject rock;
    float length = 21;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > length* (iter-1) - 5)
        {
            SpawnObjects();
            iter++;
        }

    }
    void SpawnObjects()
    {
        for(int i = 0; i < Mathf.Min(iter,8); i++)
        {
            Instantiate(rock, new Vector3(Random.Range(player.transform.position.x + 2+length, player.transform.position.x + length*2), 0.2f, Random.Range(0.5f, 5f)),Random.rotation);
        }

    }
}
