using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraScript : MonoBehaviour {

    VRNode mainNode;

    Transform cameraTransform = null;

    Vector3 initialPosition;

    // Use this for initialization
    void Start () {
        VRSettings.enabled = true;
        //GameObject.FindWithTag("MainCamera").transform.position = new Vector3(50, 0, 0); ;
        mainNode = VRNode.CenterEye;
        //gameObject.transform.position = new Vector3(50, 0, 0);
        initialPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        

        gameObject.transform.position = initialPosition + 50.0f * InputTracking.GetLocalPosition(mainNode);
	}
}
