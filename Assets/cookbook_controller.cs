
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cookbook_controller : MonoBehaviour {

	public List<string> activeRecipes;
	private string currentRecipe;
    private string currentOverlay;
    private string tempOverlay;

	// Use this for initialization
	void Start() {
		activeRecipes = new List<string>();
		activeRecipes.Add("none");
		activeRecipes.Add("none");
		currentRecipe = "none";
        currentOverlay = "overlay_prev";
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		 if (Input.GetButtonDown("Cleft")){
            switchLeft();
    	}else if (Input.GetButtonDown("Cright")){
    		switchRight();
    	}
	}

	void switchLeft(){
        transform.FindChild("overlay_prev").gameObject.SetActive(false);
        currentOverlay = "overlay_next";
		foreach (Transform child in transform){
			if (child.name == activeRecipes[1]){
				child.gameObject.SetActive(false);
			}
			if (child.name == activeRecipes[0]){
				child.gameObject.SetActive(true);
				if (activeRecipes[0] != "none"){
					currentRecipe = activeRecipes[0];
                }
            }
            if (child.name == currentOverlay)
            {
                child.gameObject.SetActive(true);
            }
        } 
    }

	void switchRight(){
        transform.FindChild("overlay_next").gameObject.SetActive(false);
        currentOverlay = "overlay_prev";
        foreach (Transform child in transform){
            if (child.name == activeRecipes[0]){
				child.gameObject.SetActive(false);
            }
			if (child.name == activeRecipes[1]){
				child.gameObject.SetActive(true);
				if (activeRecipes[1] != "none"){
					currentRecipe = activeRecipes[1];
                }
            }
            if (child.name == currentOverlay)
            {
                child.gameObject.SetActive(true);
            }
        }
	}

	public void setRecipe(string recipeName){
		activeRecipes.Remove("none");
		activeRecipes.Add(recipeName);
		currentRecipe = recipeName;
        Debug.Log(recipeName);
	}

	public void removeRecipe(string recipeName){
		activeRecipes.Remove(recipeName);
		activeRecipes.Add("none");
		if (activeRecipes[0] == "none"){
			currentRecipe = activeRecipes[1];
		}else{
			currentRecipe = activeRecipes[0];
		}
	}

    private void setSingleRecipe()
    {
        if (activeRecipes[0] == "none")
        {
            currentOverlay = "overlay_prev";
        }
        else if (activeRecipes[1] == "none")
        {
            currentOverlay = "overlay_next";
        }
    }

	public void openCookbook(){
        setSingleRecipe();
        transform.FindChild(currentOverlay).gameObject.SetActive(true);
        foreach (Transform child in transform){
			if (child.name != currentRecipe){
				child.gameObject.SetActive(false);
			}
			if (child.name == currentRecipe){
				child.gameObject.SetActive(true);
			}
		}
        transform.FindChild(currentOverlay).gameObject.SetActive(true);
    }

}
