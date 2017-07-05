using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Klase, kas kontrolē spēlētājam parādīto informāciju, kad tas saņem jaunu objektu un metodes pielietošanai
public class recievedItemScript : MonoBehaviour {
	public GameObject GotItemMenu;
	public GameObject[] itemGUIs;
    public Image newItemPic;
    public Text newItemTitle;
    public Text newItemDesc; 
    List<Item> newItemList = new List<Item>();
    Sprite[] orgSprites = new Sprite[3];
    Item newItem;
	// Use this for initialization
	void Start () {
        newItem = new Item();
        newItemTitle.text = newItem.itemName;
        newItemDesc.text = newItem.createDescription();
        newItemPic.overrideSprite = newItem.itemPicture;
		Variables.GotItemMenu = GotItemMenu;
		for (int i = 0; i < Variables.playerStats.inventory.Count; i++)
		{
            orgSprites[i] = Variables.playerStats.inventory[i].itemPicture;
			itemGUIs[i].GetComponent<Image>().overrideSprite = Variables.playerStats.inventory[i].itemPicture;            
			itemGUIs[i].GetComponent<Image>().color = new Color(1, 1, 1);
            itemGUIs[i].GetComponent<ItemGUI>().itemStats = Variables.playerStats.inventory[i];
            newItemList.Add(Variables.playerStats.inventory[i]);
            
		}
        if (Variables.playerStats.inventory.Count > 2)
        {
            
        }else
        {
            itemGUIs[Variables.playerStats.inventory.Count].GetComponent<Image>().overrideSprite = newItem.itemPicture;
            itemGUIs[Variables.playerStats.inventory.Count].GetComponent<Image>().color = new Color(1, 1, 1);
        }

	}

    public void ItemClicked(int name)
    {
        if (Variables.playerStats.inventory.Count > 2)
        {
            if (itemGUIs[name].GetComponent<ItemGUI>().itemStats == newItem)
        {
            Revert();
            itemGUIs[name].GetComponent<ItemGUI>().HideTooltip();
        }
            else
            {
            Revert();
           

                itemGUIs[name].GetComponent<Image>().overrideSprite = newItem.itemPicture;
                itemGUIs[name].GetComponent<ItemGUI>().itemStats = newItem;
                itemGUIs[name].GetComponent<ItemGUI>().HideTooltip();
                newItemList.Remove(Variables.playerStats.inventory[name]);
                newItemList.Add(newItem);
            }

        }
    
    }
    public void Continue()
    {
        Variables.currentLVL = Variables.levels.normal;
        if (Variables.playerStats.inventory.Count > 2)
        {
            for (int i = 0; i < newItemList.Count; i++)
            {

                Variables.playerStats.inventory[i].unapplyItem();
                newItemList[i].applyItem();
                Variables.playerStats.inventory = newItemList;
            }
        }else
        {
            Variables.playerStats.inventory.Add(newItem);
            newItem.applyItem();
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("hub");
    }


    public void Revert()
    {
        if (Variables.playerStats.inventory.Count > 2)
        {
            newItemList.Clear();
            for (int i = 0; i < Variables.playerStats.inventory.Count; i++)
            {
                newItemList.Add(Variables.playerStats.inventory[i]);
                itemGUIs[i].GetComponent<Image>().overrideSprite = orgSprites[i];
                itemGUIs[i].GetComponent<ItemGUI>().itemStats = Variables.playerStats.inventory[i];
            }
        }
    }
}
