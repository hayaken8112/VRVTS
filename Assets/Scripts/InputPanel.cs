using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class InputPanel : MonoBehaviour {
	public GameObject recordButton;
	public VoiceRecorder voiceRecorder;
	public AudioRecognizer audioRecognizer;
	public InputField inputField;

	// Use this for initialization
	void Start () {
		var recordTrigger = recordButton.AddComponent<ObservableEventTrigger>();
		recordTrigger.OnPointerDownAsObservable().Subscribe(_ => {
			voiceRecorder.StartRecord();
		});
		recordTrigger.OnPointerUpAsObservable().Subscribe(_ => {
			var rec_data = voiceRecorder.FinishRecord();
			audioRecognizer.SpeechToText(rec_data);
			audioRecognizer.transcriptSubject.Subscribe(txt => {
				inputField.text = txt;
			});
		});
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
