using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorCollisionDetection : MonoBehaviour {

    public Text hudText;
    public GameObject exitDoor;

    // Use this for initialization
    void Start() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Equals("FPSController"))
        {
            DisplayDoorHUD();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("FPSController"))
        {
            HideDoorHUD();
        }
    }

    void DisplayDoorHUD()
    {
        hudText.text = "Press A to open the door";
    }

    void HideDoorHUD()
    {
        hudText.text = "";
    }

    // Update is called once per frame
    void Update () {
		
	}
}
