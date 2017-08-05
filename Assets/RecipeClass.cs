using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RecipeClass
{
    internal struct RecipeStruct
    {
        internal RecipeStruct(string Name, string Instrs, string Recipe, double RecipeScoreTime)
        {
            name = Name;
            instructions = Instrs;
            recipeManual = Recipe;
            time = RecipeScoreTime;
        }
        internal string name;
        internal string instructions;
        internal string recipeManual;
        internal double time;
    }

    static Dictionary<string, RecipeStruct> recipies = new Dictionary<string, RecipeStruct>()
    {
        {"salad",
            new RecipeStruct("Salad",
            @"---- Cheese Salad ----",
            "---- Cheese Salad Manual ----\n -- 1). Collect these Ingredients: --\n         1 Cheese, 1 Cucumber, 1 Lettuce, 1 Onion, 1 Tomato\n -- 2). Wash these Ingredients: --\n        1 Cucumber, 1 Lettuce, 1 Onion, 1 Tomato \n -- 3). Cut these Ingredients: --\n        1 Cheese, 1 Washed Cucumber, 1 Washed Lettuce, 1 Washed Onion, 1 Washed Tomato \n-- 4). Combine these Ingredients at the Mixing Station: --\n         1 Cut Cucumber, 1 Cut Lettuce, 1 Cut Onion, 1 Cut Tomato & 1 Cut Cheese \n",
            120)
        },//Salad
        {"burger",
            new RecipeStruct("Burger",
            @"---- Burger ----",
            "---- Burger Manual ----\n -- 1). Collect the Ingredients: --\n         1 BBQ Sauce, 1 Cheese, 1 Hamcburger Bun, 1 Ketchup, 1 Lettuce, 1 Meat Patty, 1 Onion, 1 Tomato\n -- 2). Wash the Ingredients: --\n         Lettuce, Onion, Tomato \n -- 3). Cut the Ingredients --\n         Cheese, Lettuce, Onion, Tomato\n-- 4). Grill the Ingredients on the stove:. --\n         Burger, Meat Patty\n-- 5). Combine the Ingredients at the Mixing station for incomplete burger:. --\n         Cheese, Grilled Burger, Grilled Meat Patty, Lettuce, Onion, Tomato, Cheese \n-- 6). Combine the Ingredients at the Mixing Station for complete burger: --\n         Incomplete Burger, BBQ Sauce, Ketchup",
            180)
        },//Burger
        {"macroni",
            new RecipeStruct("Macroni N' Cheese",
                @"---- Macroni N' Cheese ----", "---- Macroni N' Cheese Manual ----\n -- 1). Collect the Ingredients: --\n         2 Cheese, 1 Macroni, 1 Potato, 1 Onion\n -- 2). Wash the Ingredients: --\n         Onion, Potato\n -- 3). Grind the Ingredients at Grinding Station:--\n         Cheese\n-- 4). Cut the Ingredients: --\n    Potato, Onion\n -- 5). Combine the ingredients on the stove: --\n    cheese, macroni, onion, potato",
                120)
        },
        {"beefstew",
            new RecipeStruct("Beef Stew",
                @"---- Beef Stew ----", "---- Beef Stevw Manual ----\n -- 1). Collect the Ingredients: --\n    1 Beef stock, 2 Carrots, 2 Meat Patty, 1 Onion, 2 Potato\n -- 2). Wash the Ingredients: --\n   Carrot, Onion, Potato\n -- 3). Grind the Ingredients at Grinding Station:--\n    Meat Patty\n-- 4). Cut the Ingredients: --\n    Carrot, Onion, Potato\n -- 5). Combine the ingredients on the stove: --\n    beef stock, , onion, potato",
                180)
        }

    };

    private string item_requirement;
    private string recipe_text;
    private string recipe_name;
    private string recipe_manual;
    private double recipe_timer;

    private DateTime end_time;

    public RecipeClass(int stage)
    {
        string[] keys = recipies.Keys.ToArray<string>();
        System.Random r = new System.Random();
        int rand;
        if (stage == 0)
		    rand = r.Next (0, keys.Length/2 - 1);
        else
            rand = r.Next(keys.Length / 2, keys.Length - 1);
        item_requirement = keys[rand];
        recipe_text = recipies[item_requirement].instructions;
        recipe_name = recipies[item_requirement].name;
        recipe_manual = recipies[item_requirement].recipeManual;
        recipe_timer = recipies[item_requirement].time;
    }



    public bool CheckRequirement(string ItemID)
    {
        return ItemID == item_requirement;
    } 

    public string GetItemID()
    {
        return item_requirement;
    }

    public string GetRecipeText()
    {
        return recipe_text;
    }

    public string GetRecipeName()
    {
        return recipe_name;
    }

    public string GetRecipeManual()
    {
        return recipe_manual;
    }

    public void startRecipe()
    {
        end_time = DateTime.Now.AddSeconds(recipe_timer);
    }

    public double getCash()
    {
        return GetScore()/100d;
    }

    public int GetScore()
    {
        double score = recipe_timer + (end_time - DateTime.Now).TotalSeconds;
        if (score < 0)
            score = 0;
        return Convert.ToInt32(score);
    }
}

