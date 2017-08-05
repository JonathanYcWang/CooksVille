using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodOrder : MonoBehaviour {
    private RecipeClass order;
	// Use this for initialization
	void Start () {
        //selecte a random Recipe order.
        order = RecipeGeneratorClass.GetRandomRecipe();
	}
	
    public RecipeClass GetOrder()
    {
        return order;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
