 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WashingStation : MonoBehaviour {

	List<string> Ingredients;
	private List<string> output;
	public float StationTime;
    Dictionary<string,string> AvailableIngredients;
	public string StationName = "Washing Station";
	void Start () {

        AvailableIngredients = new Dictionary<string, string>() 
		{{"onion","onion_washed"},{"tomato","tomato_washed"},{"cucumber","cucumber_washed"},
			{"lettuce","lettuce_washed"},{"potato","potato_washed"}, {"carrot","carrot_washed"}, };

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
