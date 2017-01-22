using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class DoorCollisionDetection : MonoBehaviour {

    public Text hudText;
    public GameObject exitDoor;

    bool canOpenDoor = false;
    bool doorOpen = false;

    // Use this for initialization
    void Start() {

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Equals("FPSController") && !doorOpen)
        {
            DisplayDoorHUD();
            canOpenDoor = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("FPSController") && !doorOpen)
        {
            HideDoorHUD();
            canOpenDoor = false;
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
        if (CrossPlatformInputManager.GetButtonDown("Fire1") && canOpenDoor && !doorOpen)
        {
            iTween.RotateTo(exitDoor, new Vector3(0, -120, 0), 2.0f);
            doorOpen = true;
            HideDoorHUD();
        }
	}
}
