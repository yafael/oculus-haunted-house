using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ExitCollision : MonoBehaviour
{

    public Text hudText;
    public GameObject exitDoor;

    bool canOpenDoor = false;
    bool doorShut = false;

    // Use this for initialization
    void Start()
    {
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("FPSController") && !doorShut)
        {
            hudText.text = "Now turn completely around to shut the door.";
        }
    }

    float tolerance = 25;

    Transform cameraTransform = null;

    Vector3 idealVector = new Vector3(-1.0f, 0.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        if (!doorShut)
        { 
            Vector3 worldDirection = cameraTransform.transform.TransformDirection(Vector3.forward);

            float ang = Vector3.Angle(worldDirection, idealVector);

            if (Mathf.Abs(ang) < tolerance)
            {
                iTween.RotateTo(exitDoor, new Vector3(0, 0, 0), 0.5f);
                hudText.text = "";
                doorShut = false;
            }
        }

    }
}
