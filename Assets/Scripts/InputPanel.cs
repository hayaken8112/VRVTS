using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class InputPanel : MonoBehaviour {
	public GameObject recordButton;
	public Button addButton;
	[SerializeField]
	GameObject voiceManager;
	public InputField inputField;

	// Use this for initialization
	void Start () {
		VoiceRecorder voiceRecorder = voiceManager.GetComponent<VoiceRecorder>();
		AudioRecognizer audioRecognizer = voiceManager.GetComponent<AudioRecognizer>();
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
}
