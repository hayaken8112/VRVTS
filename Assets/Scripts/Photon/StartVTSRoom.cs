using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVTSRoom : MonoBehaviour {

	public GameObject photonObject;

	// Use this for initialization
	void Start () {
		PhotonNetwork.Instantiate(
			photonObject.name,
			new Vector3(9.71f, 0.5f, 12.55f),
			Quaternion.identity, 0
		);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
