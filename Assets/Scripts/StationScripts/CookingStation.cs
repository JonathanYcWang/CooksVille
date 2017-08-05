using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CookingStation : MonoBehaviour {

    List<string> Ingredients;
    private List<string> temp_output;
    public float StationTime;
    public string StationName = "Mixing Station";

    private Dictionary<HashSet<CookItem>, string> PossibleRecipes = new Dictionary<HashSet<CookItem>, string>(new HashSetEqualityComparer<CookItem>());
    private List<string> AvailableIngredients = 
        new List<string>() { "onion_cut", "tomato_cut", "cucumber_cut", "cheese_cut", "lettuce_cut", "buns_cooked", "incomplete", "BBQ Sauce", "patty_cooked", "ketchup"};
    internal struct CookItem
    {
        public string name;
        public int amt;
        internal CookItem(string Name, int Amount)
        {
            name = Name;
            amt = Amount;
        }
    }

    void Start () {
		PossibleRecipes.Add(new HashSet<CookItem>() {
            new CookItem("onion_cut", 1),
            new CookItem("tomato_cut", 1),
            new CookItem("cucumber_cut",1),
            new CookItem("cheese_cut" ,1),
            new CookItem("lettuce_cut",1)
        }, "salad");

        PossibleRecipes.Add(new HashSet<CookItem>() {
            new CookItem("buns_cooked", 1),
            new CookItem("tomato_cut",1),
            new CookItem("cheese_cut" ,1),
            new CookItem("onion_cut" ,1),
            new CookItem("lettuce_cut" ,1),
        }, "incomplete");

        PossibleRecipes.Add(new HashSet<CookItem>() {
            new CookItem("incomplete", 1),
            new CookItem("BBQ Sauce", 1),
            new CookItem("patty_cooked", 1),
            new CookItem("ketchup",1)
        }, "hamburger");
    }
	
	public bool Input(List<string>  ingredient){
		Ingredients = ingredient;
		temp_output = new List<string>();
		Dictionary<string, int> station = new Dictionary<string, int>();
		foreach (string item in Ingredients)
		{
			if (station.ContainsKey(item))
				station[item]++;
			else
				station[item] = 1;
		}

		HashSet<CookItem> items = new HashSet<CookItem>();

		foreach (KeyValuePair<string, int> pair in station)
		{
			items.Add(new CookItem(pair.Key, pair.Value));
		}

		if (PossibleRecipes.ContainsKey (items)) {
			temp_output.Add (PossibleRecipes [items]);
            return true;
        }
		return false;


    }


	public List<string> getProcessedIds()
	{
		return temp_output;
	}

    public bool isValid(string id)
    {
        if (AvailableIngredients.Contains(id))
        {
            return true;
        }
        return false;
    }

}