using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenshot : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ScreenCapture.CaptureScreenshot("images/screenshot.png");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
