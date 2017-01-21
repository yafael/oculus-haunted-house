using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SimonController : MonoBehaviour {

	private bool analogWasReset = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");

		Vector2 input = new Vector2 (horizontal, vertical);

		// we don't want to register the command if analog stick isn't tilted enough
		if (input.magnitude < 0.5f) {
			// have we recently entered a command? If so, make us able to send another command now.
			if (!analogWasReset) {
				analogWasReset = true;
			}
		} else if (analogWasReset) {
			// find what direction the tilt most represents
			float upProduct = Vector2.Dot (Vector2.up, input);
			float downProduct = Vector2.Dot (Vector2.down, input);
			float leftProduct = Vector2.Dot (Vector2.left, input);
			float rightProduct = Vector2.Dot (Vector2.right, input);

			// compare up and down
			float maxHorizontal = Mathf.Max(leftProduct, rightProduct);
			if (upProduct >= downProduct) {
				if (upProduct > maxHorizontal) {
					// up is the direction
					Debug.Log ("up");
				} else {
					if (leftProduct > rightProduct) {
						// left is the direction
						Debug.Log ("left");
					} else {
						// right is the direction
						Debug.Log ("right");
					}
				}
			} else {
				// down is the direction
				Debug.Log ("down");
			}

			analogWasReset = false;
		}
	}

}
