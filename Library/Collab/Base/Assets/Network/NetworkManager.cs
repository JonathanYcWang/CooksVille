using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
    const string VERSION = "v0.0.1";
	private string objectSpawned = "Player";
	public int atLeastPlayers = 2;
    public Transform[] NPCSpawnedPoints;
    public Transform[] playerSpawnedPoints;
    public Transform[] stationSpawnedPoints;
	public GameObject[] ingredients;
	public GameObject[] pantryIngredients;
	public GameObject[] cheese;
    private int order;
    public GameObject c;
	private bool ready = false;
	private bool canEnter = true;
    public GameObject inputPanel;
	public Text errorM;
    private string playerPrefab;
	public InputField playerName;
	public InputField roomName;
	//private string pname;
	private string room;
	private int team;
	private Transform player;
	public Dropdown myDropdown; 
    public GameObject waiting;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject tieCanvas;
    public Sprite[] waitingImgs;
    // Use this for initialization


    void Start () {
        PhotonNetwork.ConnectUsingSettings(VERSION);
		myDropdown.onValueChanged.AddListener(delegate {
			myDropdownValueChangedHandler(myDropdown);
		});
		if (PhotonNetwork.room != null) {
			PhotonNetwork.LeaveRoom ();
		}
        c.SetActive(true);
        GameObject[] g = GameObject.FindGameObjectsWithTag("Waste");
        g[2].SetActive(false);
    }
	
    void OnJoinedLobby()
    {
    }

    void OnJoinedRoom()
    {
        inputPanel.SetActive(false);
		errorM.text =  "Waiting for your opponent.";
        order = PhotonNetwork.room.playerCount - 1;
        if (order == 0){
            team = 0;
            playerPrefab = "PlayerRed";
        }else{
            team = 1;
            playerPrefab = "PlayerBlue";
        }
        ready = true;
    }

	private void myDropdownValueChangedHandler(Dropdown target) {
		Debug.Log("selected: "+target.value);
		if (target.value == 0)
			target.captionText.color = Color.red;
		else
			target.captionText.color = new Color(0, 210, 255);
    }
	public void SetDropdownIndex(int index) {
		myDropdown.value = index;
	}

	public void gatherInfo(){
        
        //pname = playerName.text;
        room = roomName.text;
		//team = myDropdown.value;

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, MaxPlayers = 2 };
		if (PhotonNetwork.room == null) //&& room != "" && (team == 0 || team == 1))
        {
            //PhotonNetwork.playerName = pname;
			if (team == 0)
                playerPrefab = "PlayerRed";
            else
                playerPrefab = "PlayerBlue";
            PhotonNetwork.JoinOrCreateRoom("room", roomOptions, TypedLobby.Default);
            return;
        }
        errorM.text = "Please enter all needed information";
    }

	void faceBarToPlayer(){
		GameObject[] gs = GameObject.FindGameObjectsWithTag ("bar");
		foreach (GameObject g in gs) {
			g.GetComponent<BarTimer> ().setPlayer (player);
		}
	}

	void Update(){
		if (ready && PhotonNetwork.room.playerCount > atLeastPlayers - 1) {
            // Spaw the player and attach the camera
			GameObject g = PhotonNetwork.Instantiate (playerPrefab, playerSpawnedPoints[order].position, playerSpawnedPoints[order].rotation, 0);
			player = g.transform;
            Camera.main.GetComponent<ThirdPersonOrbitCam>().player = g.transform;
            Camera.main.GetComponent<ThirdPersonOrbitCam>().enabled = true;
            NetworkPlayer n = g.GetComponent<NetworkPlayer>();
            n.won = winCanvas;
            n.lost = loseCanvas;
            n.tied = tieCanvas;
            // IF this is the master client, set up the scene with network objects
            if (PhotonNetwork.player.isMasterClient)
            {
                // Spawn the NPCs
                PhotonNetwork.Instantiate("NPC_0", NPCSpawnedPoints[0].position, NPCSpawnedPoints[0].rotation, 0);
                PhotonNetwork.Instantiate("NPC_1", NPCSpawnedPoints[1].position, NPCSpawnedPoints[1].rotation, 0);
                PhotonNetwork.Instantiate("NPC_2", NPCSpawnedPoints[2].position, NPCSpawnedPoints[2].rotation, 0);
                PhotonNetwork.Instantiate("NPC_3", NPCSpawnedPoints[3].position, NPCSpawnedPoints[3].rotation, 0);
                PhotonNetwork.Instantiate("WashingStation", stationSpawnedPoints[0].position, stationSpawnedPoints[0].rotation, 0);
                PhotonNetwork.Instantiate("CookingStation", stationSpawnedPoints[1].position, stationSpawnedPoints[1].rotation, 0);
                PhotonNetwork.Instantiate("CuttingStation", stationSpawnedPoints[2].position, stationSpawnedPoints[2].rotation, 0);
                PhotonNetwork.Instantiate("GrindingStation", stationSpawnedPoints[3].position, stationSpawnedPoints[3].rotation, 0);
                PhotonNetwork.Instantiate("Stove", stationSpawnedPoints[4].position, stationSpawnedPoints[4].rotation, 0);
                PhotonNetwork.Instantiate("WashingStation", stationSpawnedPoints[5].position, stationSpawnedPoints[5].rotation, 0);
                PhotonNetwork.Instantiate("CuttingStation", stationSpawnedPoints[6].position, stationSpawnedPoints[6].rotation, 0);
                PhotonNetwork.Instantiate("GrindingStation", stationSpawnedPoints[7].position, stationSpawnedPoints[7].rotation, 0);

                foreach (GameObject i in ingredients) {
					i.GetComponent<spawnFruit> ().spawn ();
				}

				foreach (GameObject i in pantryIngredients) {
					i.GetComponent<pantrySpawn> ().spawn ();
				}

				foreach (GameObject i in cheese) {
					i.GetComponent<cheeseSpawn> ().spawn ();
				}
            }
            Cursor.visible = false;
            ready = false;
			c.SetActive(false);
            if (team == 0){
                waiting.GetComponent<Image>().sprite = waitingImgs[0];
            }else{
                waiting.GetComponent<Image>().sprite = waitingImgs[1];
            }
            waiting.SetActive(true);
			Invoke ("faceBarToPlayer", 3);
		}
		if (canEnter){
			if (Input.GetButtonDown("Submit")){
				gatherInfo ();
				canEnter = false;
			}
		}
	}
}
