using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour {

	private bool isGlowing = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (isGlowing) {
			// fade out over time
		}
	}

	public void Press(bool rightButton) {
		// play noise and effect
		if (!rightButton)
		{
			Debug.Log("WRONG!");
		}
		else
		{
			Debug.Log("Good!");
		}
	}
}
