using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashCanBehavior : MonoBehaviour {

    private InteractionState interactionState;
    public GameObject canvas;
    public GameObject mainCanvas;
    private InventoryBehavior trash;
    private string stage;
    private GameObject dumpSpawn;
    private InventoryBehavior inventory;
	public Text binMess;

    private GameObject perm_icons;
    // Use this for initialization
    void Start()
    {
        stage = "available";
        trash = canvas.GetComponentInChildren<InventoryBehavior>();
        dumpSpawn = GameObject.FindGameObjectWithTag("DumpSpawnPoint");
        mainCanvas.SetActive(false);
    }

    private void Update()
    {
        if (stage == "watingForPickedItems")
        {
            if (Input.GetButtonDown("StationLeave"))
            {
                leaveClick();
            }
            else if (Input.GetButtonDown("StationProcess"))
            {
                processClick();
            }
        }
        
    }

    // When user press e in front of the station
    public void gotInteracted()
    {
        mainCanvas.SetActive(true);
        inventory = interactionState.startState(canvas);
        trash.setother(inventory);
        inventory.setother(trash);
        inventory.makeAvailableToTransfer(true);
        trash.makeAvailableToTransfer(true);
        GameObject s = trash.allSlots[0];
        Event e = Event.current;
        EventSystem.current.SetSelectedGameObject(s);
        stage = "watingForPickedItems";
    }

    // Called when PROCESS BUTTON clicked
    public void processClick()
    {
        perm_icons.SetActive(true);
        mainCanvas.SetActive(false);
        // Collect all ids
        trash.deleteAllSlots();
        inventory.makeAvailableToTransfer(true);
        trash.makeAvailableToTransfer(true);
        trash.setother(null);
        inventory.setother(null);
        interactionState.finsihState(canvas);
        stage = "available";
        //disselect all slots
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Called when LEAVE BUTTON clicked
    public void leaveClick()
    {
        perm_icons.SetActive(true);
        mainCanvas.SetActive(false);
        trash.transferAll();
        inventory.makeAvailableToTransfer(true);
        trash.makeAvailableToTransfer(true);
        trash.setother(null);
        inventory.setother(null);
        interactionState.finsihState(canvas);
        stage = "available";
        //disselect all slots
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void setInteractacState(InteractionState iState, GameObject instr_icons)
    {
        interactionState = iState;
        inventory = iState.inventory;
        perm_icons = instr_icons;
    }

}
