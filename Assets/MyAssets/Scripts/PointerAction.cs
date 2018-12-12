using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class PointerAction : MonoBehaviour {
	public GameObject debugText;
	public GameObject boundingboxPrefab;
	GameObject currentBB;

	// Use this for initialization
	void Start () {
		int cnt = 0;
		bool compFlag = true;
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();

		eventTrigger.OnPointerDownAsObservable().Subscribe(PointerEventData => {
			if (compFlag) {
				compFlag = false;
				Vector2 localPos = GetLocalPosition(PointerEventData.position);
				Vector3 bbPos = new Vector3(localPos.x, localPos.y, this.transform.position.z);
				currentBB = Instantiate(boundingboxPrefab);
				currentBB.transform.parent = transform;
				currentBB.transform.localPosition = localPos;
				cnt++;
			}
			});
		eventTrigger.OnDragAsObservable().Subscribe(PointerEventData => {
			Vector2 localPos = GetLocalPosition(PointerEventData.position);
			currentBB.GetComponent<BoundingBox>().UpdatePosition(localPos);
			debugText.GetComponent<Text>().text = localPos.ToString();
		});
		eventTrigger.OnEndDragAsObservable().Subscribe(_ => {
			currentBB.GetComponent<BoundingBox>().StartBoundingBox();
			compFlag = true;
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	Vector2 GetLocalPosition(Vector2 screenPos){
		Vector2 res = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), screenPos, Camera.main, out res);
		return res;
	}
}
