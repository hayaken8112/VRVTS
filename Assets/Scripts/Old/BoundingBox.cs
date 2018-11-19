using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BoundingBox : MonoBehaviour {
	RectTransform rect;
	int box_id;

	// Use this for initialization
	void Start () {
		rect = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UpdatePosition(Vector2 pointerPos) {
			float bb_x = transform.localPosition.x;
			float bb_y = transform.localPosition.y;
			int pivot_x = pointerPos.x > bb_x ? 0 : 1;
			int pivot_y = pointerPos.y > bb_y ? 0 : 1;
			rect.pivot = new Vector2(pivot_x, pivot_y);
			float width = Math.Abs(pointerPos.x - bb_x) * 0.01f;
			float height = Math.Abs(pointerPos.y - bb_y) * 0.01f;
			rect.sizeDelta = new Vector2(width, height);

	}
	public void StartBoundingBox(){
		var voiceManager = GameObject.FindGameObjectWithTag("VoiceManager");
		VoiceRecorder voiceRecorder = voiceManager.GetComponent<VoiceRecorder>();
		AudioRecognizer audioRecognizer = voiceManager.GetComponent<AudioRecognizer>();
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerEnterAsObservable().Subscribe(_ => {
			Image img = this.GetComponent<Image>();
			Color c = img.color;
			c.a = 0.5f;
			img.color = c;
		});
		eventTrigger.OnPointerExitAsObservable().Subscribe(_ => {
			Image img = this.GetComponent<Image>();
			Color c = img.color;
			c.a = 0.1f;
			img.color = c;
		});
		eventTrigger.OnPointerDownAsObservable().Subscribe(_ => {
			voiceRecorder.StartRecord();
		});
		eventTrigger.OnPointerUpAsObservable().Subscribe(_ => {
			var rec_data = voiceRecorder.FinishRecord();
			audioRecognizer.SpeechToText(rec_data);
			audioRecognizer.transcriptSubject.Subscribe(txt => {
				AddData(txt);
			});

		});
	}
	void AddData(string txt){
		GameObject commentManagerObj = GameObject.Find("CommentManager");
		CommentManagerOld commentManager = commentManagerObj.GetComponent<CommentManagerOld>();
		CommentDataOld data = new CommentDataOld(txt, rect.pivot, rect.localPosition, rect.sizeDelta.x, rect.sizeDelta.y);
		commentManager.AddCommentData(data);
		commentManager.UpdateBoard();
	}
}
