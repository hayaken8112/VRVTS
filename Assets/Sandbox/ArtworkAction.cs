using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using UniRx.Triggers;

public class ArtworkAction : MonoBehaviour {
	public GameObject boundingboxPrefab;
	public GameObject canvas;
	public GameObject currentBB;

	// Use this for initialization
	void Start () {
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerDownAsObservable().Subscribe(PointerEventData => {
				Debug.Log(PointerEventData.position);
				Vector3 bbPos = new Vector3(PointerEventData.position.x, PointerEventData.position.y, 0);
				currentBB = Instantiate(boundingboxPrefab, bbPos, transform.rotation, canvas.transform);
				Debug.Log(transform.position);
			});
		eventTrigger.OnDragAsObservable().Subscribe(PointerEventData => {
			currentBB.GetComponent<BoundingBox>().UpdatePosition(PointerEventData.position);
		});
		eventTrigger.OnPointerUpAsObservable().Subscribe(_ => {
			currentBB.GetComponent<BoundingBox>().StartBoundingBox();
		});
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
