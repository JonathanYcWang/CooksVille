using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBehaviour : MonoBehaviour
{

    private InteractionState interactionState;
    public GameObject canvas;
    public GameObject mainCanvas;
    private InventoryBehavior storage;
    private string stage;
    private GameObject dumpSpawn;
    private InventoryBehavior inventory;
    public Text binMess;
    // Use this for initialization
    void Start()
    {
        stage = "available";
        storage = canvas.GetComponentInChildren<InventoryBehavior>();
        dumpSpawn = GameObject.FindGameObjectWithTag("DumpSpawnPoint");
        mainCanvas.SetActive(false);
    }

    // When user press e in front of the station
    public void gotInteracted()
    {
        mainCanvas.SetActive(true);
        inventory = interactionState.startState(canvas);
        storage.setother(inventory);
        inventory.setother(storage);
        inventory.makeAvailableToTransfer(true);
        storage.makeAvailableToTransfer(true);
        stage = "watingForPickedItems";
    }

    // Called when PROCESS BUTTON clicked

    // Called when LEAVE BUTTON clicked
    public void leaveClick()
    {
        mainCanvas.SetActive(false);
        // Collect all ids
        inventory.makeAvailableToTransfer(true);
        storage.makeAvailableToTransfer(true);
        storage.setother(null);
        inventory.setother(null);
        interactionState.finsihState(canvas);
        stage = "available";
        //mainCanvas.SetActive(false);
        //storage.transferAll();
        //inventory.makeAvailableToTransfer(false);
        //storage.makeAvailableToTransfer(false);
        //storage.setother(null);
        //inventory.setother(null);
        //interactionState.finsihState(canvas);
        //stage = "available";
    }

    //public void processClick()
    //{
        //mainCanvas.SetActive(false);
        // Collect all ids
        //inventory.makeAvailableToTransfer(false);
        //storage.makeAvailableToTransfer(false);
        //storage.setother(null);
        //inventory.setother(null);
        //interactionState.finsihState(canvas);
        //stage = "available";
    //}

    public void setInteractacState(InteractionState iState)
    {
        interactionState = iState;
        inventory = iState.inventory;
    }

}
