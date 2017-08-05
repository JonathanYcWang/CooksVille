using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

public class GiveOrder : MonoBehaviour {

    private PhotonView photonView;
	private RecipeClass recipe;
	private string team;
    private string item_requirement;
    private bool isAssignedToTeam;
    private double score;
    public GameObject redFlag;
    public GameObject blueFlag;
    public GameObject[] dishSprites;
    internal struct RecipeStruct
    {
        internal RecipeStruct(string Name, string Instrs, string Recipe, double RecipeScoreTime, string RecipeIngredients)
        {
            name = Name;
            instructions = Instrs;
            recipeManual = Recipe;
            time = RecipeScoreTime;
            recipeIngredients = RecipeIngredients;
        }
        internal string name;
        internal string instructions;
        internal string recipeManual;
        internal double time;
        internal string recipeIngredients;
    }

    static Dictionary<string, RecipeStruct> recipies = new Dictionary<string, RecipeStruct>()
    {
        {"salad",
            new RecipeStruct("Salad",
            @"---- Cheese Salad ----",
            "---- How to make: SALAD!!! ----\n\n -- 1). Wash these Ingredients: --\n        1 Cucumber\n        1 Lettuce\n        1 Onion\n        1 Tomato \n\n -- 2). Cut these Ingredients: --\n        1 Cheese\n        1 Washed Cucumber\n        1 Washed Lettuce\n        1 Washed Onion\n        1 Washed Tomato \n\n -- 3). Combine these Ingredients at the Mixing Station: --\n        1 Cut Cucumber\n        1 Cut Lettuce\n        1 Cut Onion\n        1 Cut Tomato\n        1 Cut Cheese",
            240,
            "1 Cheese\n1 Cucumber\n1 Lettuce\n1 Onion\n1 Tomato")
        },
        { "hamburger",
            new RecipeStruct("Burger",
            @"---- Burger ----",
            "---- How to make: BURGER!!! ----\n\n -- 1). Wash these Ingredients: --\n        1 Lettuce\n        1 Onion\n        1 Tomato \n\n -- 2). Cut these Ingredients: --\n        1 Cheese\n        1 Washed Lettuce\n        1 Washed Onion\n        1 Washed Tomato\n\n -- 3). Grill these Ingredients on the stove:. --\n        1 Hamburger Buns\n        1 Meat Patty\n\n -- 4). Combine these Ingredients at the Mixing station for Incomplete burger:. --\n        1 Cut Cheese\n        1 Grilled Burger\n        1 Cut Lettuce\n        1 Cut Onion\n        1 Cut Tomato \n\n -- 5). Combine the Ingredients at the Mixing Station for complete Burger: --\n        1 Incomplete Burger\n        1 Grilled Patty\n        1 BBQ Sauce\n        1 Ketchup",
            600,
             "1 BBQ Sauce\n1 Cheese\n1 Hamburger Buns\n1 Ketchup\n1 Lettuce\n1 Meat Patty\n1 Onion\n1 Tomato")
        },
        {"macncheese",
            new RecipeStruct("Macroni N' Cheese",
                @"---- Macroni N' Cheese ----",
                "---- How to make: Macroni N' Cheese!!!----\n\n -- 1). Wash these Ingredients: --\n        1 Onion\n        1 Potato\n\n -- 2). Grind these Ingredients at Grinding Station: --\n        2 Cheese\n\n -- 3). Cut these Ingredients: --\n        1 Washed Potato\n        1 Washed Onion\n\n -- 4). Combine these ingredients on the stove: --\n        2 Grinded Cheese\n        1 Macroni\n        1 Cut Onion\n        1 Cut Potato",
                240,
                 "2 Cheese\n1 Macroni\n1 Potato\n1 Onion")
        },
        {"beefstew",
            new RecipeStruct("Beef Stew",
                @"---- Beef Stew ----",
                "---- How to make: Beef Stew!!! ----\n -- 1). Wash these Ingredients: --\n        1 Carrot\n        1 Onion\n        1 Potato\n\n -- 2). Grind these Ingredients at Grinding Station: --\n        2 Meat Patty\n\n -- 3). Cut these Ingredients: --\n        1 Washed Carrot\n        1 Washed Onion\n        1 Washed Potato\n\n -- 4). Combine these ingredients on the stove: --\n        2 Beefstock\n        1 Cut Onion\n        1 Cut Potato\n        1 Cut Carrot\n        2 Grinded Patty",
                480,
                "2 Beefstock\n1 Carrots\n2 Meat Patty\n1 Onion\n1 Potato")
        }
    };

    // Use this for initialization
    void Start () {
        photonView = GetComponent<PhotonView>();
		isAssignedToTeam = false;
        team = "";
        setTeamFlag();
    }


	// Called when the player not got a recipe
	public string gotInteracted(string thisteam, int stage){
        string order = "";
		if (!isAssignedToTeam) {
            //create recipe
            string[] keys = recipies.Keys.ToArray<string>();
            int rand;
            if (thisteam == "red")
                rand = (stage) % 4;
            else
                rand = (stage + 2) % 4;
            item_requirement = keys[rand];
            score = recipies[item_requirement].time;
			order = string.Format("{0}_{1}_{2}", recipies[item_requirement].name, score, item_requirement);
            team = thisteam;
            setTeamFlag();
            isAssignedToTeam = true;
		}

        photonView.RPC("updateNetwork", PhotonTargets.OthersBuffered, team, item_requirement, isAssignedToTeam);
        return order;
	}

    // Called when the player already got a recipe
    public string gotInteracted(InventoryBehavior inventory, string thisteam)
    {
        if (team == thisteam)
        {
            List<string> ids = inventory.getAllIds();
            int i = 0;
            foreach (string id in ids)
            {
                if (item_requirement == id)
                {
                    inventory.deleteSlot(i);
                    isAssignedToTeam = false;
                    team = "";
                    setTeamFlag();
                    photonView.RPC("updateNetwork", PhotonTargets.OthersBuffered, team, item_requirement, isAssignedToTeam);
                    return item_requirement;
                }
                i++;
            }
        }
        return "";
    }

    public void showMessage(Text mess, bool gotRecipe, string thisteam)
    {
        if (gotRecipe)
            if (thisteam == team)
                mess.text = "Is my order ready?\nI'm getting impatient!!!";
            else
                mess.text = "Sigh.. Looks like you already\nhave someone to take care of.";
        else
            if (isAssignedToTeam)
                mess.text = "Oops! Somebody was already\nhere before you.";
            else
                mess.text = "I am so hungry.\nDO you have something to offer me?";
    }

    [PunRPC]
    void updateNetwork(string thisteam, string thisItemRequirement, bool thisisAssisnedToTeam)
    {
        team = thisteam;
        setTeamFlag();
        item_requirement = thisItemRequirement;
        isAssignedToTeam = thisisAssisnedToTeam;
    }

    void setTeamFlag()
    {
        if (team == "")
        {
            redFlag.SetActive(false);
            blueFlag.SetActive(false);
            disableSprite();
        }
        else
        {
            if (team == "red")
                redFlag.SetActive(true);
            else
                blueFlag.SetActive(true);
            setSprite();
        }
    }

    public bool ordered()
    {
        return team != "";
    }

    void disableSprite(){
        foreach (var sprite in dishSprites){
            sprite.SetActive(false);
        }
    }

    void setSprite(){
        Debug.Log(item_requirement);
        foreach (var sprite in dishSprites){
            if (sprite.name == item_requirement){
                sprite.SetActive(true);
            }
            else
            {
                sprite.SetActive(false);
            }
        }
    }
}
