using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Stove : MonoBehaviour {
	List<string> Ingredients;
	private List<string> temp_output;
	public float StationTime;

	public ArrayList NewFood = new ArrayList();
	private Dictionary<HashSet<CookItem>, string> PossibleRecipes = 
		new Dictionary<HashSet<CookItem>, string>(new HashSetEqualityComparer<CookItem>());

	private Dictionary<string,string> OtherAvailableIngredients = new Dictionary<string, string> ();

    private List<string> AvailableIngredients =
    new List<string>() { "cheese_grinded", "potato_cut", "onion_cut", "macaroni", "patty_grinded", "carrot_cut", "beefstock", "patty", "buns"};



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

	public string StationName = "Stove";
	void Start () {
		 
		PossibleRecipes.Add(new HashSet<CookItem>() {
			new CookItem("cheese_grinded", 2),
			new CookItem("potato_cut", 1),
			new CookItem("onion_cut",1),
			new CookItem("macaroni" ,1)

		}, "macncheese");


		PossibleRecipes.Add(new HashSet<CookItem>() {
			new CookItem("patty_grinded", 2),
			new CookItem("onion_cut", 1),
			new CookItem("potato_cut",1),
			new CookItem("carrot_cut" ,1),
			new CookItem("beefstock" ,2)
		}, "beefstew");
			
			
		OtherAvailableIngredients = new Dictionary<string, string>() 
		{{"patty","patty_cooked"},{"buns","buns_cooked"} };
		
}
		

	public bool Input(List<string>  ingredient){

		Ingredients = ingredient;
		temp_output = new List<string>();
		foreach (string i in Ingredients) {
		
			if (OtherAvailableIngredients.ContainsKey (i)) {
				temp_output.Add (OtherAvailableIngredients [i]);
			}
		}
		if (temp_output.Count == ingredient.Count &&temp_output.Count != 0) {
			return true;
		}
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

