using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class kitchenStation : MonoBehaviour {

    private InteractionState interactionState;
    public GameObject canvas;
    private InventoryBehavior table;
    
    private InventoryBehavior inventory;
    public WashingStation WashingStation;
    public CuttingStation CuttingStation;
	public CookingStation CookingStation;
	public GrindingStation GrindingStation;
	public Stove Stove;
	public Text stationMess;
    private string origMess;
    private GameObject dumpSpawn;
	public BarTimer bar1;
	public BarTimer bar2;

	public int stationIngredientCount;

    public string stage;
    public List<string> newIDs;
    public bool clostT = false;
    public bool doneFood = false;

	public string teamBelong;

    private bool isReadyForSameTeam;
    private bool isInProcess;
    private float timeLeft;
    private PhotonView photonView;

    private Text message;
    private Image background;
    private List<PhotonView> playersP;
    private List<PhotonView> enemiesP;
    private int totalTeamOrders;
    private GameObject perm_icons;
    // Use this for initialization
    void Start () {
        stage = "available";
        table = canvas.GetComponentInChildren<InventoryBehavior>();
        newIDs = new List<string>();
        dumpSpawn = GameObject.FindGameObjectWithTag("DumpSpawnPoint");
        canvas.SetActive(false);
        photonView = GetComponent<PhotonView>();
        teamBelong = "";
        isReadyForSameTeam = false;
        isInProcess = false;
        origMess = stationMess.text;
        timeLeft = -1;
        stationIngredientCount = 0;

    }


    void Update()
    {
        if (stage == "Taken")
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
        if (isInProcess)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                stage = "Ready for same team to pick up";
                isReadyForSameTeam = true;
                isInProcess = false;
                timeLeft = 2;
                prepareAndCallNetworkUpdate();
            }
        }
        else if (isReadyForSameTeam)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0){
                stage = "Ready for everyone to pick up";
                isReadyForSameTeam = false;
                prepareAndCallNetworkUpdate();
            }
        }
    }

	void closeTimeBar(){
		bar1.stopTimer ();
		bar2.stopTimer ();
	}

    [PunRPC]
    void openTimeBar(){
		bar1.startTimer ();
		bar2.startTimer ();
	}

    void setTimerbar(float blue, float red){
		bar1.setTimer (blue);
		bar2.setTimer (red);
	}

    void setBarColor(string b1, string b2){
		bar1.setImg (b1);
		bar2.setImg (b2);
	}

    [PunRPC]
    void setBar()
    {
        if (teamBelong == "blue")
        {
            setBarColor("blue", "red");
        }
        else if (teamBelong == "red")
        {
            setBarColor("red", "blue");
        }
        setTimerbar(timeLeft, timeLeft + 2);
    }


    // When user press e in front of the station
    public int gotInteracted(string fromTeam)
    {
        int i = 0;
        switch (stage){
            // Enter the system
			case "available":
				teamBelong = fromTeam;
                interactionState.startState(canvas);
                table.setother(inventory);
                inventory.setother(table);
                setValidItemsToTransfer();

                GameObject s = table.allSlots[0];
                EventSystem.current.SetSelectedGameObject(s);

                stage = "Taken";
                break;
            case "In Use":
                if (isInProcess)
                {
                    showMessage("The station is processing");
                }
                else
                {
                    showMessage("The station is being used by the other");
                }
                break;
            // Ready to pick up
            case "Ready for same team to pick up":
                if (teamBelong == fromTeam)
                {
                    addNewItemsToInventory();       
                }
                else
                {
                    showMessage("Not ready to steal");
                }
                break;
            case "Ready for everyone to pick up":
                addNewItemsToInventory();
                if (teamBelong != fromTeam)
                {
                    noticeGotStolen();
                    i = 1;
                }
                break;
        }
        // For network
        prepareAndCallNetworkUpdate();
        return i;
    }

	public string checkStation(){
		if (WashingStation != null) {
			return "Washing Station";
		} else if (CuttingStation != null) {
			return "Cutting Station";
		} else if (CookingStation != null) {
			return "Mixing Station";
		} else if (GrindingStation != null) {
			return "Grinding Station";
		} else if (Stove != null) {
			return "Stove";
		}else {
			return "";
		}
	}

    [PunRPC]
    void clear()
    {
        stage = "available";
        isInProcess = false;
        isReadyForSameTeam = false;
		closeTimeBar ();
        stationIngredientCount = 0;
    }

    void prepareClear()
    {
        photonView.RPC("clear", PhotonTargets.AllBuffered);
    }

    // Called when PROCESS BUTTON clicked
    public void processClick()
    {
        // Collect all ids
        List<string> ids = new List<string>();
        List<Item> items = table.getAllItem();
        foreach (Item i in items)
            if (i != null)
            {
                ids.Add(i.id);
            }

        string m = "Combination must be valid\nCheck cookbook!";
        // Pick the right station to send ids and get approved
		if (WashingStation != null) {
			//playWashing sound effect
			GameObject.FindGameObjectWithTag ("Player").GetComponent<seController> ().playWash ();

			if (WashingStation.Input (ids)) {
				newIDs = WashingStation.getProcessedIds ();
                stationIngredientCount = newIDs.Count;

                timeLeft = 7 + ids.Count * 15;
                prepareAndCallNetworkUpdate();
                photonView.RPC("setBar", PhotonTargets.AllBuffered);
                photonView.RPC("openTimeBar", PhotonTargets.AllBuffered);

                isInProcess = true;
				clostT = true;
				closeTable ();

                noticeProcessingToAllPlayers();
            } else {
				showMessage(m);
			}
		} else if (CuttingStation != null) {
			//playCutting sound effect
			GameObject.FindGameObjectWithTag ("Player").GetComponent<seController> ().playCut ();

			if (CuttingStation.Input (ids)) {
				newIDs = CuttingStation.getProcessedIds ();
                stationIngredientCount = newIDs.Count;

                timeLeft = 7 + ids.Count * 15;
                prepareAndCallNetworkUpdate();
                photonView.RPC("setBar", PhotonTargets.AllBuffered);
                photonView.RPC("openTimeBar", PhotonTargets.AllBuffered);

                isInProcess = true;
				clostT = true;
				closeTable ();

                noticeProcessingToAllPlayers();

            }
            else {
				showMessage(m);
			}
		} else if (CookingStation != null) {
			//playCooking sound effect -- salad station; playShake
			GameObject.FindGameObjectWithTag ("Player").GetComponent<seController> ().playShake ();
            
			if (CookingStation.Input (ids)) {
				newIDs = CookingStation.getProcessedIds ();
                stationIngredientCount = newIDs.Count;

                timeLeft = 7 + ids.Count * 15;
                prepareAndCallNetworkUpdate();
                photonView.RPC("setBar", PhotonTargets.AllBuffered);
                photonView.RPC("openTimeBar", PhotonTargets.AllBuffered);

                isInProcess = true;
				clostT = true;
				closeTable ();

                noticeProcessingToAllPlayers();

            }
            else {
				showMessage(m);
			}

		} else if (GrindingStation != null) {
            //playGrinding sound effect
            GameObject.FindGameObjectWithTag("Player").GetComponent<seController>().playGrind();
            
			if (GrindingStation.Input (ids)) {
				newIDs = GrindingStation.getProcessedIds ();
                stationIngredientCount = newIDs.Count;

                timeLeft = 7 + ids.Count * 15;
                prepareAndCallNetworkUpdate();
                photonView.RPC("setBar", PhotonTargets.AllBuffered);
                photonView.RPC("openTimeBar", PhotonTargets.AllBuffered);

                isInProcess = true;
				clostT = true;
				closeTable ();

                noticeProcessingToAllPlayers();

            }
            else {
				showMessage(m);
			}

		}
		else if (Stove != null) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<seController> ().playCook ();
            if (Stove.Input (ids)) {
				newIDs = Stove.getProcessedIds ();
                stationIngredientCount = newIDs.Count;

                timeLeft = 7 + ids.Count * 15;
                prepareAndCallNetworkUpdate();
                photonView.RPC("setBar", PhotonTargets.AllBuffered);
                photonView.RPC("openTimeBar", PhotonTargets.AllBuffered);

                isInProcess = true;
				clostT = true;
				closeTable ();

                noticeProcessingToAllPlayers();

            }
            else {
				showMessage(m);
			}

		}
        // For network
        prepareAndCallNetworkUpdate();
    }

	
    // Called when LEAVE BUTTON clicked
    public void leaveClick()
    {	
		stationMess.text = origMess;
        table.transferAll();
        inventory.setother(null);
        table.setother(null);
        inventory.makeAvailableToTransfer(true);
        interactionState.finsihState(canvas);
        stage = "available";
        teamBelong = "";
        // For network
        prepareAndCallNetworkUpdate();
        //icon
        perm_icons.gameObject.SetActive(true);
        //disselect all slots
        EventSystem.current.SetSelectedGameObject(null);

    }


    // Start the process: enter food ready state in 3s, clear the table, notice the leaving
    void closeTable()
    {
        table.deleteAllSlots();
        inventory.makeAvailableToTransfer(true);
        interactionState.finsihState(canvas);
        stage = "In Use";
        perm_icons.gameObject.SetActive(true);
        //disselect all slots
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void setInteractacState(InteractionState iState, GameObject instr_icons)
    {
        interactionState = iState;
        inventory = iState.inventory;
        perm_icons = instr_icons;
    }

    bool addNewItemsToInventory()
    {
        
        if (inventory.emptySlots >= newIDs.ToArray().Length)
        {
            prepareClear();
            List<string> n = newIDs;
            // For each new id, create a new gameobject and push its item into the inventory table
            foreach (string s in newIDs)
            {
                string a = "Vegetables/" + s;
                UnityEngine.Object o = Resources.Load(a);
                GameObject g = Instantiate((GameObject)o, dumpSpawn.transform);
                inventory.AddItem(g.GetComponent<Item>());
            }
            newIDs.Clear();
            perm_icons.gameObject.SetActive(true);
            return true;
        }
        return false;
    }

    void prepareAndCallNetworkUpdate()
    {
        //Convert newIDs list to a string
        string i = string.Join(",", newIDs.ToArray());
        photonView.RPC("networkupdate", PhotonTargets.AllBuffered, timeLeft, stage, i, clostT, doneFood, teamBelong, isInProcess, isReadyForSameTeam, stationIngredientCount);
    }

    [PunRPC]
    void networkupdate(float t, string s, string i, bool c, bool d, string team, bool startProcess, bool sameTeamPickUp, int itemsCount)
    {
        timeLeft = t;
        stage = s;
        clostT = c;
        doneFood = d;
        //break string i into list
        newIDs = new List<string>(i.Split(','));
        teamBelong = team;
        isInProcess = startProcess;
        isReadyForSameTeam = sameTeamPickUp;
        stationIngredientCount = itemsCount;
    }

    public void showMessgae(Text mess)
    {
        if (isInProcess || isReadyForSameTeam || stage == "Ready for everyone to pick up")
        {
            mess.text = string.Format("{0}: {1} ingredient(s)", checkStation(), stationIngredientCount);
        }
        else
        {
            mess.text = string.Format("{0}: {1}", checkStation(), stage);
        }
    }

    public void setValidItemsToTransfer()
    {
        List<string> ids = new List<string>();
        foreach (GameObject g in inventory.allSlots)
        {
            Item item = (g.GetComponent<Slot>().getItem());
            if (item != null)
            {
                String id = item.id;
                if (WashingStation != null)
                {
                    if (WashingStation.isValid(id))
                    {
                        g.GetComponent<Slot>().setTransferON(true);
                    }
                    else
                    {
                        g.GetComponent<Slot>().setTransferON(false);
                    }
                }
                else if (CuttingStation != null)
                {
                    if (CuttingStation.isValid(id))
                    {
                        g.GetComponent<Slot>().setTransferON(true);
                    }
                    else
                    {
                        g.GetComponent<Slot>().setTransferON(false);
                    }
                }
                else if (CookingStation != null)
                {
                    if (CookingStation.isValid(id))
                    {
                        g.GetComponent<Slot>().setTransferON(true);
                    }
                    else
                    {
                        g.GetComponent<Slot>().setTransferON(false);
                    }
                }
                else if (GrindingStation != null)
                {
                    if (GrindingStation.isValid(id))
                    {
                        g.GetComponent<Slot>().setTransferON(true);
                    }
                    else
                    {
                        g.GetComponent<Slot>().setTransferON(false);
                    }
                }
                else if (Stove != null)
                {
                    if (Stove.isValid(id))
                    {
                        g.GetComponent<Slot>().setTransferON(true);
                    }
                    else
                    {
                        g.GetComponent<Slot>().setTransferON(false);
                    }
                }
            }
        }
    }

    public void setup(Text mess, Image bg, List<PhotonView> players, List<PhotonView> enemies, int currentOrderTotal)
    {
        message = mess;
        background = bg;
        playersP = players;
        enemiesP = enemies;
        totalTeamOrders = currentOrderTotal;
    }

    void showMessage(string m)
    {
        message.text = m;
        background.enabled = true;
        Invoke("clearMessage", 4);
    }

    void clearMessage()
    {
        message.text = "";
        background.enabled = false;
    }

    void noticeGotStolen()
    {
        string enemyM = string.Format("Your ingredients in {0} got stolen", checkStation());
        string playerM = "Nice steal!\n You got $15 into your countdown bonus of your order";
        foreach (PhotonView p in playersP)
        {
            p.RPC("showMessage", PhotonTargets.AllBuffered, playerM);
        }
        foreach (PhotonView p in enemiesP)
        {
            p.RPC("showMessage", PhotonTargets.AllBuffered, enemyM);
        }
    }

    void noticeProcessingToAllPlayers()
    {
        if (totalTeamOrders == 1)
        {
            showMessage("Nice job!\nYou know you can have 2 orders from 2 customers at a time, right?");
        }
        //string playerM = string.Format("Ready in {0}s\n May get stolen in {1}s", timeLeft, timeLeft + 2);
        string enemyM = string.Format("{0} processing {1} ingredient(s)\n You can steal in {2}s", checkStation(), stationIngredientCount, timeLeft + 2);
        foreach (PhotonView p in playersP)
        {
            //p.RPC("showMessage", PhotonTargets.AllBuffered, playerM);
        }
        foreach (PhotonView p in enemiesP)
        {
            p.RPC("showMessage", PhotonTargets.AllBuffered, enemyM);
        }
    }
}
