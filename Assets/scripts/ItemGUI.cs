using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Klase, kas kontorlē papildinformācijas parādīšanu, kad spēlētājs apskata viņa savāktu objektu
public class ItemGUI : MonoBehaviour {
	public Image image;
	public GameObject tooltip;
	public Text description;
	public Text title;
	public Item itemStats;
	Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = new Vector3(Screen.width/8, Screen.height/4, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ShowTooltip()
	{
		if (itemStats != null )
		{
            
			tooltip.transform.position = Input.mousePosition + offset;
			image.overrideSprite = itemStats.itemPicture;
			title.text = itemStats.itemName;
			description.text = itemStats.createDescription();
		}
	}
	public void HideTooltip()
	{
		tooltip.transform.position = new Vector3(5000, 5000, 0);
	}
}
