using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingStation : MonoBehaviour {

    List<string> Ingredients;
    public List<string> output;
	public float StationTime;
	Dictionary<string,string> AvailableIngredients;

	public ArrayList NewFood = new ArrayList();

	public string StationName = "Cutting Station";

	void Start () {
        AvailableIngredients = new Dictionary<string, string>() 
		{{"onion_washed","onion_cut"},{"tomato_washed","tomato_cut"},{"cucumber_washed","cucumber_cut"},{"cheese","cheese_cut"},
			{"lettuce_washed","lettuce_cut"},{"potato_washed","potato_cut"}, {"carrot_washed","carrot_cut"} };
		
	}

	public bool Input(List<string> ingredients){
		
		Ingredients = ingredients;

		output = new List<string>();

		foreach (string i in Ingredients) {
			if (AvailableIngredients.ContainsKey (i)) {
				output.Add (AvailableIngredients [i]);
			}
            else
            {
                output.Clear();
                return false;
            }
        }
        return (output.Count > 0);
    }

    public List<string> getProcessedIds()
    {
        return output;
    }

    public bool isValid(string id)
    {
        if (AvailableIngredients.ContainsKey(id))
        {
            return true;
        }
        return false;
    }
}
