using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// klase kontrolē vārtu pozīcijas "hub" ainā
public class GateController : MonoBehaviour {
	public GameObject[] gates;
	
	void Awake () {
		/*Variables.gatePos[0] = false;
		Variables.gatePos[1] = false;
		Variables.gatePos[2] = false;
		for(int i =0; i < gates.Length;i++)
		{
			gates[i].SetActive(false);
		}*/
		
	}
	
	public void CloseGates()
	{
		
		for (int i = 0; i < gates.Length; i++)
		{
			gates[i].SetActive(true);
		}
	}
}
