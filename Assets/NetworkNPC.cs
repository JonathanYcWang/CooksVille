using System.Collections;
using UnityEngine;

public class NetworkNPC : MonoBehaviour {

    // Use this for initialization
    void Start () {
        // IF this is the object belong to the controlled clident, enable for the client to control
        if (GetComponent<PhotonView>().isMine)
        {
            GetComponent<AICharacterControl>().enabled = true;
        }
        else // IF not, disable 
        {
            StartCoroutine(wait(1));
        }
    }

    IEnumerator wait(float f)
    {
        yield return new WaitForSeconds(f);
        GetComponent<AICharacterControl>().enabled = false;
    }
}
