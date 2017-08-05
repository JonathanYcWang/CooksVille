using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindingStation : MonoBehaviour {
	List<string> Ingredients;
	public List<string> output;
	public float StationTime;
	Dictionary<string,string> AvailableIngredients;

	public ArrayList NewFood = new ArrayList();
	public string StationName = "Grinding Station";
	void Start () {
		AvailableIngredients = new Dictionary<string, string>() 
		{{"cheese","cheese_grinded"},{"patty","patty_grinded"}};
		
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


