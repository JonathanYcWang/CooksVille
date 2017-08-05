using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour {

    public RecipeClass recipe;
    public int team;
	// Use this for initialization
	void Start () {
        team = 0;
	}
    
    int Deliver(List<string> l)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (recipe.CheckRequirement(l[i]))
                return i;
        }
        return -1;
    }

    // The player signals NPC
    public void gotInteracted()
    {
        
    }
}
