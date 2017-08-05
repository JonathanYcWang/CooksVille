using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerEnterHandler
{
    public Sprite slotEmpty;
    public Sprite slotHighLight;
    public Sprite slotDisabled;
	public Item thisSlotsItem;
    private bool empty = true;
    public InventoryBehavior parent;
    private bool transferON = true;
	
    // Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = slotEmpty;
        SpriteState st = new SpriteState();
        st.highlightedSprite = slotHighLight;
        st.pressedSprite = slotEmpty;
        st.disabledSprite = slotDisabled;
        Button butt = GetComponent<Button>();
        butt.spriteState = st;

        RectTransform slotRect = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    PointerEventData OnPointerClick(int clickCount)
    {
        throw new NotImplementedException();
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight, Sprite disabled){
        GetComponent<Image>().sprite = neutral;
        SpriteState st = new SpriteState();
        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;
        st.disabledSprite = disabled;
        Button butt = GetComponent<Button>();
        butt.spriteState = st;
        if (transferON == false)
        {
            butt.interactable = false;
        }
        else
        {
            butt.interactable = true;
        }
        

    }

    public void AddItem(Item thisItem){
        if (thisItem != null)
        {
            thisSlotsItem = thisItem;
            ChangeSprite(thisSlotsItem.spriteNeutral, thisSlotsItem.spriteHighlighted, thisSlotsItem.spriteDisabled);
            empty = false;
        }

    }

    public bool isEmpty()
    {
        return empty;
    }

    public Item RemoveItem()
    {
        Item i = thisSlotsItem;
        if (!empty && transferON)
        {
            thisSlotsItem = null;
            empty = true;
            ChangeSprite(slotEmpty, slotHighLight, slotDisabled);
            return i;

        }
        else
        {
            return null;
        }
    }

    public void hi()
    {
        if (!empty && parent.GetComponent<InventoryBehavior>().clickable)
        {
            if (transferON && parent.other.emptySlots > 0)
            {
                parent.transfer(thisSlotsItem);
                RemoveItem();
            }
            
        }
        Debug.Log("asdhfiuasfaseawergawefg");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(thisSlotsItem.id);
        /*
        if (!empty && parent.GetComponent<InventoryBehavior>().clickable)
        {
            if (transferON && parent.other.emptySlots > 0)
            {
                parent.transfer(thisSlotsItem);
                RemoveItem();
            }
            //Debug.Log("delete");
        }
        */
        Debug.Log("enter");
       
    }

    public Item getItem()
    {
        return thisSlotsItem;
    }

    public void OnPointerEnter(PointerEventData a)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("draged");
    }
    
    public void setTransferON(bool value)
    {
        transferON = value;
        GetComponent<Button>().interactable = value;
    }

}
