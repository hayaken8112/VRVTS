using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeForward : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update () {
		GameObject camera = GameObject.Find("OVRCameraRig");
		Vector3 pos = camera.transform.position + 100 * camera.transform.forward;
		this.transform.position = pos;
	}
}
