using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using UniRx;

public class AudioRecognizer : MonoBehaviour {
	private string apiKey = "AIzaSyBIyMOKzajLDTVMfzkchFoCa8_CLik0n84";
	SpeechData.Form form = null;
	SpeechData.Response response = null;

	public string responseText = "";

	// Use this for initialization
	public Text debugText;
	public Subject<string> transcriptSubject;
	void Start () {
		InitRequestData();
		transcriptSubject = new Subject<string>();
	}
	
	void InitRequestData(){
		form = new SpeechData.Form();
		form.config = new SpeechData.ConfigData();
		form.config.enableWordTimeOffsets = false;
		form.config.languageCode = "ja-JP";
		form.config.sampleRateHertz = 16000;
		form.config.encoding = "ENCODING_UNSPECIFIED";
		form.audio = new SpeechData.AudioData();
		response = new SpeechData.Response();
	}

	// public string GetText(string data) {
	// 	StartCoroutine(SpeechToText(data));
	// 	return responseText;
	// }
	public void SpeechToText(string data) {
		form.audio.content = data;
		string jsonString = JsonUtility.ToJson(form);
		StartCoroutine(SendSound(jsonString));
	}
	
	// public void HttpTest() {
	// 	form.audio.uri = "gs://cloud-samples-tests/speech/brooklyn.flac";
	// 	form.config.languageCode = "en-US";
	// 	form.config.encoding = "flac";
	// 	string jsonString = JsonUtility.ToJson(form);
	// 	StartCoroutine(SendSound(jsonString));
	// }

	IEnumerator SendSound(string json) {
		byte[] postData = System.Text.Encoding.UTF8.GetBytes (json);
		UnityWebRequest www = new UnityWebRequest("https://speech.googleapis.com/v1/speech:recognize" + "?key=" + apiKey, "POST");
		www.chunkedTransfer = false;
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
		www.SetRequestHeader("Content-Type", "application/json");
		yield return www.SendWebRequest();

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
			debugText.text = www.error;
			responseText = "Sorry";
        }
        else {
            Debug.Log(www.downloadHandler.text);
			string responseJson = www.downloadHandler.text;
			response = JsonUtility.FromJson<SpeechData.Response>(responseJson);
			if (response.results.Length != 0) {
				responseText = response.results[0].alternatives[0].transcript;
				transcriptSubject.OnNext(responseText);
			} else {
				transcriptSubject.OnNext("Please try again.");
			}
        }
	}

}