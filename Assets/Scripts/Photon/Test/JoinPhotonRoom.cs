using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPhotonRoom : MonoBehaviour {

	public GameObject photonObject;

	// Use this for initialization
	void Start () {
		Debug.Log("room名は、" + CreateRoomPushed.roomName + "です");
		
		PhotonNetwork.Instantiate(
			 photonObject.name,
            new Vector3(-7.09f, 1.62f, -2.7f),
            Quaternion.identity, 0
		);
		GameObject mainCamera = 
            GameObject.FindWithTag("MainCamera");
        mainCamera.GetComponent<ThirdPersonCamera>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
