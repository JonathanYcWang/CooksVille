using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class RecipeGeneratorClass
{

    static public RecipeClass GetRandomRecipe()
    {
        Debug.Log("Fetching Recipe...");
        return new RecipeClass(1);
    }
}
