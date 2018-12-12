using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardInput : MonoBehaviour {
	public InputField demoInputField;
	//int selectNumber;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//selectNumber = demoInputField.caretPosition;
	}

	public void ButtonAPushed() {
		Debug.Log("a押された");
		//Debug.Log(demoInputField.caretPosition);
		//demoInputField.text = demoInputField.text.Substring(0, demoInputField.caretPosition) + "a" + demoInputField.text.Substring(demoInputField.caretPosition);
		demoInputField.text = demoInputField.text + "a";
	}

	public void ButtonBPushed() {
		Debug.Log("b押された");
		//demoInputField.text = demoInputField.text.Substring(0, demoInputField.caretPosition) + "b" + demoInputField.text.Substring(demoInputField.caretPosition);
		demoInputField.text = demoInputField.text + "b";
	}

	public void ButtonDeletePushed() {
		if (demoInputField.text.Length > 0){
			demoInputField.text = demoInputField.text.Substring(0, demoInputField.text.Length - 1);
		}
	}
}
