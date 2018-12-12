using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuTransition : MonoBehaviour {

	public Vector3 toPos;
	public Vector3 toRot;
	public Vector3 defaultPos;
	public Vector3 defaultRot;

	// Use this for initialization
	void Start () {
		RectTransform rect = this.GetComponent<RectTransform>();
		Sequence seq = DOTween.Sequence();
		seq.Append(rect.DOJump(defaultPos,1,3,2));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
