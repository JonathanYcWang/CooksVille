using UnityEngine;
using UnityEngine.UI;

public class respawn : MonoBehaviour {

	private int rtime;
    public bool destroyed = false;
    private Text mess;
	private PhotonView photonView;
	public BarTimer bar;
    private Image bgMess;
	void Start(){
		photonView = GetComponent<PhotonView> ();
		rtime = 10;
		bar.setTimer (rtime);
	}

    void Respawn()
    {
        SphereCollider s = this.GetComponent<SphereCollider>();
        BoxCollider b = this.GetComponent<BoxCollider>();
        CapsuleCollider c = this.GetComponent<CapsuleCollider>();       
        if (s != null)
            s.enabled = true;
        else if (b != null)
            b.enabled = true;
        else if (c != null)
            c.enabled = true;
		bar.stopTimer ();
		//bar.setTimer (rtime += 3);
        MeshRenderer[] mehes = this.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mehes)
        {
            m.enabled = true;
        }
    }

    void ClearText()
    {
        mess.text = "";
        bgMess.enabled = false;
    }

    public void gotInteracted(InventoryBehavior inventory, Text message, Image bg)
    {
        mess = message;
        bgMess = bg;
        Item i = GetComponent<Item>();
        if (inventory.AddItem(i))
        {
			photonView.RPC ("hideAndShowIngre", PhotonTargets.AllBuffered);

            //string ingredientName = this.gameObject.GetComponent<Item>().id;
            //mess.text = string.Format("Added {0} into inventory", ingredientName);
        }
        else
        {
            mess.text = string.Format("Need more room in inventory");

            bgMess.enabled = true;
            Invoke("ClearText", 3);
        }
        //bgMess.enabled = true;
        //Invoke("ClearText", 2);
    }

	[PunRPC]
	void hideAndShowIngre(){
		
		SphereCollider s = this.GetComponent<SphereCollider>();
		BoxCollider b = this.GetComponent<BoxCollider>();
		CapsuleCollider c = this.GetComponent<CapsuleCollider>();

		if (s != null)
			s.enabled = false;
		else if (b != null)
			b.enabled = false;
		else if (c != null)
			c.enabled = false;
		bar.startTimer ();

		MeshRenderer[] mehes = this.transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer m in mehes)
		{
			m.enabled = false;
		}

		Invoke("Respawn", rtime);
	}
}
