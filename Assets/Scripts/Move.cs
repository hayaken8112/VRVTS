using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	Transform transform = null;
	// Use this for initialization
	void Start () {
		Debug.Log("hello");	
		transform = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0.1f, 0, 0);	
	}
}
