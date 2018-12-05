using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CommentTransition : MonoBehaviour {

	public Vector3 onPos;
	public Vector3 onRot;
	public Vector3 offPos;
	public Vector3 offRot;
	RectTransform rect;

	// Use this for initialization
	void Start () {
		rect = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TurnOn() {
		rect.DOMove(onPos,1);
		rect.DORotate(onRot,1);
	}
	public void TurnOff() {
		rect.DOMove(offPos,1);
		rect.DORotate(offRot,1);
	}
}
