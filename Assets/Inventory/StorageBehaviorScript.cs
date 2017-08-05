using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBehaviorScript : MonoBehaviour {

    private InteractionState interactionState;
    public GameObject canvas;
    private InventoryBehavior storage;
    private string stage;
    private GameObject dumpSpawn;
    private InventoryBehavior inventory;

    // Use this for initialization
    void Start()
    {
        stage = "available";
        storage = canvas.GetComponentInChildren<InventoryBehavior>();
        dumpSpawn = GameObject.FindGameObjectWithTag("DumpSpawnPoint");
        canvas.SetActive(false);
    }

    // When user press e in front of the station
    public void gotInteracted()
    {
		if (stage == "available")
		{
			inventory = interactionState.startState(canvas);
			storage.setother(inventory);
			inventory.setother(storage);
			stage = "watingForPickedItems";
		}
    }

    // Called when LEAVE BUTTON clicked
    public void leaveClick()
    {
        storage.setother(null);
        inventory.setother(null);
        interactionState.finsihState(canvas);
        stage = "available";
    }
}
