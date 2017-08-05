using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehavior : MonoBehaviour {

	private RectTransform inventoryRect;
	
	private float inventoryWidth, inventoryHeight;
	
	public int slots;
	
	public int rows;
	
	public float paddingLeft, paddingTop;
	
	public float slotSize;
	
	public GameObject slotPrefab;

	public List<GameObject> allSlots;

    public int emptySlots;
    
    public bool clickable;

    public int id;

    public InventoryBehavior other = null;
	

	// Use this for initialization
	void Awake () {
        CreateLayout();
        clickable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void CreateLayout() {
		allSlots = new List<GameObject>();
        emptySlots = slots;
		inventoryWidth = (slots / rows) * (slotSize + paddingLeft) + paddingLeft;
		inventoryHeight = rows * (slotSize + paddingTop) + paddingTop;
		inventoryRect = GetComponent<RectTransform>();
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
		inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

		int columns = slots / rows;
        
		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < columns; x++) {
				GameObject newSlot = (GameObject)Instantiate (slotPrefab);
                
				RectTransform slotRect = newSlot.GetComponent<RectTransform>();

				newSlot.name = "Slot";
                newSlot.GetComponent<Slot>().parent = this;

				newSlot.transform.SetParent(this.transform.parent);

				slotRect.localPosition = inventoryRect.localPosition + new Vector3((paddingLeft + 9) * (x+1) + (slotSize) * x, -(paddingTop+2) * (y+2) - (slotSize * y));

				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Horizontal, slotSize);
				slotRect.SetSizeWithCurrentAnchors (RectTransform.Axis.Vertical, slotSize);

				allSlots.Add(newSlot);
			} 
		}
	}


 
	public bool AddItem(Item item)
    {
        if (emptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if (tmp.isEmpty()) {
                    tmp.AddItem(item);
                    emptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    void addItems(List<Item> items)
    {
        foreach (Item i in items)
        {
            if (i != null)
                AddItem(i);
        }
    }

    public List<Item> getAllItem()
    {
        List<Item> allItems = new List<Item>();
        foreach (GameObject g in allSlots)
        {
            allItems.Add(g.GetComponent<Slot>().getItem());
        }
        return allItems;
    }

	public List<string> getAllIds()
	{
		List<string> allIds = new List<string>();
		foreach (GameObject g in allSlots)
		{
			Item i = g.GetComponent<Slot> ().getItem ();
			if (i != null)
				allIds.Add (i.id);
		}
		return allIds;
	}

    public void deleteSlot(int number)
    {
        Slot s = allSlots[number].GetComponent<Slot>();
        s.RemoveItem();
        emptySlots++;
    }

    public List<Item> deleteAllSlots()
    {
        List<Item> allItems = new List<Item>();
        foreach (GameObject g in allSlots)
        {

            allItems.Add(g.GetComponent<Slot>().RemoveItem());
        }
        emptySlots = slots;
        return allItems;
    }

    public void transfer(Item i)
    {
        if (other != null)
        {
            other.AddItem(i);
        }
        emptySlots++;

    }

    public void transferAll()
    {
        if (other != null)
        {
            other.addItems(deleteAllSlots());
        }
        
    }

    public bool hasSpace()
    {
        return emptySlots != 0;
    }

    public void setother(InventoryBehavior ib)
    {
        other = ib;
    }

    public void makeAvailableToTransfer(bool value)
    {
        foreach (GameObject g in allSlots)
        {
            g.GetComponent<Slot>().setTransferON(value);
        }
    }
}
