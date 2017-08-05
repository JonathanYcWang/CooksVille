
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
public class NetworkPlayer 
{
    
    private PhotonVoiceRecorder voiceChat;
    public Text speakingIndicator;

    public Text RecipeIndicator;
    hello
    private int playerteam = 1;

    private GameObject middle;
    private Transform cam;
    public Text instructionText;

    public GameObject recipePanel;
    public GameObject[] recipeIcons;
    private bool gotRecipe;

    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;
    public InventoryBehavior inventory;


    public GameObject craftCanvas;
    public InventoryBehavior craft;

    private string stage;
    private GameObject station;

    public Text craftMessage;

    public GameObject cookBook;
    public GameObject playerCanvas;

    public GameObject instructionPanel;
    public GameObject instructionCanvas;

    private int stationType;



    public List<string> returnIds;
    public List<string> enterIds = new List<string>();
    public Transform dumpSpawn;

    private AICharacterControl MoveNPC;

    public GameObject DumpCanvas;
    public InventoryBehavior dump;
    public bool onDumpIngredients = false;

    public Text pernamentInstruction;
    public Text StationErrorMess;

    public Text Won;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //Disable voice chat
        voiceChat = GetComponent<PhotonVoiceRecorder>();
        voiceChat.Transmit = false;

        cam = Camera.main.transform;
        middle = GameObject.FindGameObjectWithTag("middle");
        //Debug.Log(cam);
        //Debug.Log(middle);
        gotRecipe = false;

        instructionPanel.SetActive(false);
        cookBook.SetActive(false);
        DumpCanvas.SetActive(false);
        craftCanvas.SetActive(false);
        stage = "";
        // IF this is the object belong to the controlled clident, enable for the client to control
        if (photonView.isMine)
        {
            GetComponent<BasicBehaviour>().enabled = true;
            GetComponent<MoveBehaviour>().enabled = true;
            GetComponent<AudioListener>().enabled = true;
        }
        else //IF not, disable
        {
            GetComponent<BasicBehaviour>().enabled = false;
            GetComponent<MoveBehaviour>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
            //StartCoroutine(wait(2));
        }


    }

    IEnumerator wait(float f)
    {
        yield return new WaitForSeconds(f);

    }

    void Update()
    {
        if (photonView.isMine)
        {
            switch (stage)
            {
                case "":
                    showInstruction();
                    dumpIngredients();
                    break;

                case "beforeCraft":
                    controlTransision(true);
                    craftCanvas.SetActive(true);
                    StationErrorMess.text = "";
                    stage = "enterCraft";

                    break;
                case "enterCraft":
                    break;

                case "existedCraft":
                    controlTransision(false);
                    craftCanvas.SetActive(false);
                    stage = "";
                    break;

                case "remove":
                    controlTransision(true);
                    DumpCanvas.SetActive(true);
                    break;

                case "exitRemove":
                    controlTransision(false);
                    DumpCanvas.SetActive(false);
                    stage = "";
                    break;

            }

            // Press Q to talk feature
            pushToTalk();
            showcookbook();
            showInstructionPanel();

        }
    }



    // Show instructions when close to an object
    // Dectect key pressed and handle the interations
    void showInstruction()
    {
        // Cast a ray
        RaycastHit hit;
        Vector3 offset = middle.transform.position - cam.position;
        //Debug.DrawRay(cam.position, offset, Color.green, 1);
        // IF hit something
        if (Physics.Raycast(cam.position, offset, out hit, 8))
        {
            GameObject hitObject = hit.collider.gameObject;
            // IF it is a NPC and have not collect the recipe, collect
            switch (hitObject.tag)
            {
                // Meet NPC
                case "NPC":
                    MoveNPC = hitObject.GetComponent<AICharacterControl>();
                    MoveNPC.move = false;
                    // Not have recipe yet
                    if (!gotRecipe && hitObject.GetComponent<Recipe>().team == 0)
                    {
                        instructionText.text = "Press E to get the recipe";
                        if (Input.GetKeyDown("e"))
                        {
                            string order = hitObject.GetComponent<Recipe>().recipe.GetRecipeText();
                            photonView.RPC("addRecipe", PhotonTargets.AllBufferedViaServer, order);
                            hitObject.GetComponent<TextMesh>().text = string.Format("Team {0}", playerteam);
                            hitObject.GetComponent<Recipe>().team = playerteam;
                        }
                    }
                    // Already have recipe
                    else if (gotRecipe && hitObject.GetComponent<Recipe>().team == playerteam)
                    {
                        instructionText.text = "Press E to deliver the meal.";
                        if (Input.GetKeyDown("e"))
                        {
                            deliverFood(hitObject);
                        }
                    }
                    else
                    {
                        instructionText.text = "Hello!";
                    }
                    break;
                // IF it is an ingrident
                case "ingredient":

                    // IF there is still space in the inventory
                    if (inventory.hasSpace())
                    {
                        instructionText.text = "Press E to collect the item";
                        if (Input.GetKeyDown("e"))
                        {
                            collectItem(hitObject);
                        }
                    }
                    else
                    {
                        instructionText.text = "No more space in the inventory to store";
                    }
                    break;

                // IF hit a station on the kitchen
                case "kitchenStation":
                    // Station is the station object for a specific crafting station, needed for sending Item to Station script when press CRAFT button
                    station = hitObject;

                    if (station.name == "WashingStation(Clone)")
                    {
                        stationType = 0;
                        int stationstage = station.GetComponent<WashingStation>().stage;
                        if (stationstage == 0)
                        {
                            instructionText.text = "Press E to Wash ingridients";
                            if (Input.GetKeyDown("e"))
                            {
                                stage = "beforeCraft";
                            }
                        }
                        else if (stationstage == 1) // pickup
                        {

                            instructionText.text = "Press E to pick up the item";
                            if (Input.GetKeyDown("e"))
                            {

                                returnIds = station.GetComponent<WashingStation>().output;
                                if (inventory.emptySlots > returnIds.Count - 1)
                                {
                                    station.GetComponent<WashingStation>().timeOut();
                                    foreach (string s in returnIds)
                                    {

                                        string a = "Vegetables/" + s;
                                        Object o = Resources.Load(a);
                                        GameObject g = Instantiate((GameObject)o, dumpSpawn);
                                        inventory.AddItem(g.GetComponent<Item>());
                                    }
                                }
                                else
                                {
                                    instructionText.text = "Not enough space for all processed ingredients";
                                }

                            }

                        }
                        else if (stationstage == 2)
                        {
                            instructionText.text = "This Washing station is in use";
                        }
                    }
                    else if (station.name == "CuttingStation(Clone)")
                    {
                        int stationstage = station.GetComponent<CuttingStation>().stage;
                        stationType = 1;
                        if (stationstage == 0)
                        {
                            instructionText.text = "Press E to Cut ingridients";
                            if (Input.GetKeyDown("e"))
                            {
                                stage = "beforeCraft";
                            }

                        }
                        else if (stationstage == 1) // pickup
                        {
                            instructionText.text = "Press E to pick up the item";
                            if (Input.GetKeyDown("e"))
                            {

                                returnIds = station.GetComponent<CuttingStation>().output;
                                if (inventory.emptySlots > returnIds.Count - 1)
                                {
                                    station.GetComponent<CuttingStation>().timeOut();
                                    foreach (string s in returnIds)
                                    {

                                        string a = "Vegetables/" + s;
                                        Object o = Resources.Load(a);
                                        GameObject g = Instantiate((GameObject)o, dumpSpawn);
                                        inventory.AddItem(g.GetComponent<Item>());
                                    }
                                }
                                else
                                {
                                    instructionText.text = "Not enough space for all processed ingredients";
                                }
                            }

                        }
                        else if (stationstage == 2)
                        {
                            instructionText.text = "This Cutting station is in use";
                        }
                    }
                    else if (station.name == "CookingStation(Clone)")
                    {
                        stationType = 2;
                        int stationstage = station.GetComponent<CookStation>().stage;
                        if (stationstage == 0)
                        {
                            instructionText.text = "Press E to cook the ingredients";
                            if (Input.GetKeyDown("e"))
                            {
                                stage = "beforeCraft";
                            }

                        }
                        else if (stationstage == 1) // pickup
                        {
                            instructionText.text = "Press E to pick up the item";
                            if (Input.GetKeyDown("e"))
                            {
                                returnIds = station.GetComponent<CookStation>().output;
                                if (inventory.emptySlots > returnIds.Count - 1)
                                {
                                    station.GetComponent<CookStation>().timeOut();
                                    foreach (string s in returnIds)
                                    {

                                        string a = "Vegetables/" + s;
                                        Object o = Resources.Load(a);
                                        GameObject g = Instantiate((GameObject)o, dumpSpawn);
                                        inventory.AddItem(g.GetComponent<Item>());
                                    }
                                }
                                else
                                {
                                    instructionText.text = "Not enough space for all processed ingredients";
                                }
                            }

                        }
                        else if (stationstage == 2)
                        {
                            instructionText.text = "This Cook station is in use";
                        }
                    }
                    break;
                // Open the door
                case "door":
                    hitObject.GetComponent<Animator>().SetBool("doorOpen", true);
                    break;

                default:
                    instructionText.text = "";

                    break;

            }
        }
        // When hit nothing
        else
        {
            if (MoveNPC != null)
            {
                MoveNPC.move = true;
            }
            instructionText.text = "";

        }


    }

    void controlTransision(bool isConverted)
    {

        if (isConverted)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = isConverted;
        GetComponent<BasicBehaviour>().onPlayerMovement = !isConverted;
        Camera.main.GetComponent<ThirdPersonOrbitCam>().enabled = !isConverted;
        instructionText.text = "";
        pernamentInstruction.enabled = !isConverted;
        inventory.clickable = isConverted;
        craft.clickable = isConverted;
    }

    void showcookbook()
    {
        if (Input.GetKey("c"))
        {
            cookBook.SetActive(true);
            playerCanvas.SetActive(false);

        }
        else
        {
            cookBook.SetActive(false);
            playerCanvas.SetActive(true);
        }
    }


    void showInstructionPanel()
    {
        if (Input.GetKey("tab"))
        {
            instructionPanel.SetActive(true);
            playerCanvas.SetActive(false);

        }
        else
        {
            instructionPanel.SetActive(false);
            playerCanvas.SetActive(true);
        }
    }

    // o is the hit object, need to destroy
    // item is the order of the item in inventoryIcons list, need it to add to the panel
    void collectItem(GameObject o)
    {
        o.GetComponent<respawn>().destroyed = true;

        Item obj = o.GetComponent<Item>();
        inventory.AddItem(obj);
    }


    void dumpIngredients()
    {
        if (Input.GetKeyDown("r"))
        {
            stage = "remove";
            onDumpIngredients = true;
        }
    }

    public void removeDumpIngredients()
    {
        dump.deleteAllSlots();
        onDumpIngredients = false;
        stage = "exitRemove";
    }

    // Called when click CRAFT
    public void sendItemToStation()
    {
        List<Item> allCraftingItems = craft.getAllItem();
        // allCraftingItems contains a list of all items from crating panel, the station need to get it and check valid or not

        foreach (Item i in allCraftingItems)
        {

            if (i != null)
            {
                enterIds.Add(i.id);
            }
        }

        if (stationType == 0)
        {
            //washing
            if (station.GetComponent<WashingStation>().Input(enterIds))
            {
                craft.deleteAllSlots();
                stage = "existedCraft";
            }
            else
            {
                StationErrorMess.text = "All ingredients must be valid to be processed";
            }
        }
        else if (stationType == 1)
        {
            //cutting
            if (station.GetComponent<CuttingStation>().Input(enterIds))
            {
                craft.deleteAllSlots();
                stage = "existedCraft";
            }
            else
            {
                StationErrorMess.text = "All ingredients must be valid to be processed";
            }
        }
        else
        {
            //cooking
            if (station.GetComponent<CookStation>().Input(enterIds))
            {
                stage = "existedCraft";
                craft.deleteAllSlots();
            }
            else
            {
                StationErrorMess.text = "All ingredients must be valid to be processed";
            }
        }
        enterIds.Clear();
    }

    // Called when click LEAVE
    public void leaveCraft()
    {
        List<Item> allDeletedItems = craft.deleteAllSlots();
        foreach (Item i in allDeletedItems)
        {
            if (i != null)
                inventory.AddItem(i);
        }

        stage = "existedCraft";
    }

    [PunRPC]
    void addRecipe(string RecipeText)
    {
        Debug.Log("order");
        // Add recipe to the panel
        RecipeIndicator.text = "ORDER:\n" + RecipeText + "\n\nPress C to view cookbook";
        // Change to got recipe
        gotRecipe = true;
    }

    void deliverFood(GameObject NPCObject)
    {
        //test if food can be delivered, if result is -1, then no items match the npc's desired item.
        //If result >= 0, than that is the index that the npc's desired item is in.
        List<Item> allIventoryItems = inventory.getAllItem();
        List<string> allIventoryIDS = new List<string>();
        foreach (Item i in allIventoryItems)
        {
            if (i != null)
            {
                allIventoryIDS.Add(i.id);
            }
        }


        int result = NPCObject.GetComponent<Recipe>().Deliver(allIventoryIDS);

        if (result >= 0)
        {

            NPCObject.GetComponent<Recipe>().SetNewRecipe();
            NPCObject.GetComponent<Recipe>().team = 0;
            inventory.deleteAllSlots();
            RecipeIndicator.text = "Get the next recipe from some customer";
            Won.text = "CONGRATULATION!!!\nYOU SUCCESSFULLY DELIVERED THE FOOD TO THE CUSTOMER";
            gotRecipe = false;
            Time.timeScale = 0;
        }
        else
        {
            instructionText.text = "None of your food is what I want";
        }


    }


    void pushToTalk()
    {
        if (Input.GetKey("q"))
        {
            voiceChat.Transmit = true;
            speakingIndicator.text = "Speaking";
        }
        else
        {
            voiceChat.Transmit = false;
            speakingIndicator.text = "";
        }
    }

    // This method is called from script "slot"
    // id == 0 means: a slot from inventory clicked -> delete and add it to craft 
    // id == 1 means: versa
    public void moveItem(int id, Item i)
    {
        switch (id)
        {
            // Slot in inventory is clicked
            case 0:
                if (onDumpIngredients)
                    dump.AddItem(i);
                else
                    craft.AddItem(i);
                break;
            // Slot in craft is clicked
            case 1:
                inventory.AddItem(i);
                break;
            // Slot in dump is clicked
            case 2:
                inventory.AddItem(i);
                break;
        }
    }
    
}

*/
