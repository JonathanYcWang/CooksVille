using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipePickup : MonoBehaviour {

    public GameObject inventoryPanel;
    public GameObject[] inventoryIcons;
    private bool gotRecipe;

    void Start()
    {
        gotRecipe = false;
    }

    void OnCollisionEnter(Collision collision)
	{
        // IF already got the recipe, skip
        if (gotRecipe) return;
        // Else, add the recipe
        if (collision.gameObject.tag == "NPC")
        {
            addRecipe();
        }
	}

    void addRecipe()
    {
        // Add recipe to the panel
        GameObject i = Instantiate(inventoryIcons[0]);
        i.transform.SetParent(inventoryPanel.transform);
        // Change to got recipe
        gotRecipe = true;
    }
}
