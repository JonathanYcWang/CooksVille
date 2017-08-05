using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    
	public GameObject Player;
    public string[] tags;
	private Vector3 playerPos;
	private Vector3 v;
	private Vector3 v1;
	public int i = 0;
	private Vector3 s;
	//transparent
	private ArrayList oldGS = new ArrayList();
	private GameObject[] newGS;
    


	public bool skip = false;
	// Audio Source
	public AudioSource unlockaudio;

	// Use this for initialization
	void Start () {
		//transform.LookAt (playerPos);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//Debug.Log ("----MAKE TRANS----");
		makeTransparent ();
        
	}

    

	void makeTransparent(){	
		//collect all blocked objects
		RaycastHit[] hits;
		Vector3 offset =  Player.transform.position - transform.position;
		hits = Physics.RaycastAll (transform.position, offset, 100f);
		Debug.DrawRay(transform.position, offset, Color.green);
		newGS = new GameObject[hits.Length];
		for (int i = 0; i < hits.Length; i++) {
			newGS [i] = hits[i].collider.gameObject;
		}
		ArrayList t = new ArrayList();
		//old object no longer blocks
		//Debug.Log ("aaaaaaa");
		foreach (GameObject oldG in oldGS){
			if (oldG != null && !exists (oldG, newGS) && oldG.tag == "remove"){
				//Debug.Log (oldG);
				oldG.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
				t.Add (oldG);
			}
		}
		foreach (GameObject g in t) {
			oldGS.Remove (g);
		}
		//new objects block
		foreach (GameObject newG in newGS) {
			if (newG != null && !exists (newG, oldGS) && newG.tag == "remove") {
				//Debug.Log (newG);
				oldGS.Add (newG);
				newG.GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
			}
		}
	}

	//check of GameObject g in array
	bool exists(GameObject g, ArrayList gs){
		int i = gs.Count;
		while (i > 0) {
			i -= 1;
			if (g == gs [i])
				return true;
		}
		return false;
	}

	bool exists(GameObject g, GameObject[] gs){
		int i = gs.Length;
		//Debug.Log (i);
		//Debug.Log (g);
		while (i > 0) {
			i -= 1;
			//Debug.Log (gs[i]);
			if (g == gs [i])
				return true;
		}
		return false;
	}
}