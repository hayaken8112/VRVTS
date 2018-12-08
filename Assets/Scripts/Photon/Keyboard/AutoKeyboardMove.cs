using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AutoKeyboardMove : MonoBehaviour {

	public void SenserOn(GameObject parentObj){
		GameObject btn = parentObj.transform.GetChild(0).gameObject;
		RectTransform rect = btn.GetComponent<RectTransform>();
		rect.DOScale(
			new Vector3(1.3f, 1.3f, 1.3f),
			0.05f
		);
	}

	public void LeaveImage(GameObject parentObj){
		GameObject btn = parentObj.transform.GetChild(0).gameObject;
		RectTransform rect = btn.GetComponent<RectTransform>();
		rect.DOScale(
			new Vector3(1, 1, 1),
			0
		);
	}
}
