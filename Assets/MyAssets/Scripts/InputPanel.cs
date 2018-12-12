using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class InputPanel : MonoBehaviour {
	public Button recordButton;
	public MaterialUI.MaterialButton materialButton;
	public Button addButton;
	[SerializeField]
	GameObject voiceManager;
	public InputField inputField;
	bool isRecording = false;
	public MaterialUI.VectorImageData startIcon;
	public MaterialUI.VectorImageData stopIcon;
	public CommentController commentController;

	// Use this for initialization
	void Start () {
		VoiceRecorder voiceRecorder = voiceManager.GetComponent<VoiceRecorder>();
		AudioRecognizer audioRecognizer = voiceManager.GetComponent<AudioRecognizer>();
		recordButton.OnClickAsObservable().Subscribe(_ => {
			if (!isRecording) {
				voiceRecorder.StartRecord();
				materialButton.iconVectorImageData = stopIcon;
			} else {
				materialButton.iconVectorImageData = startIcon;
				var rec_data = voiceRecorder.FinishRecord();
				audioRecognizer.SpeechToText(rec_data);
				audioRecognizer.transcriptSubject.Subscribe(txt => {
					inputField.text = txt;
				});
			}
			isRecording = !isRecording;
		});
		addButton.OnClickAsObservable().Subscribe(_ => {
			if (inputField.text != "") {
				commentController.AddComment(inputField.text);
				inputField.text = "";
			}
		});
	}
}
