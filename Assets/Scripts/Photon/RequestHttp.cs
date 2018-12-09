using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestHttp : MonoBehaviour {

	const string serverUrl = "http://s-tsports.com:8082/";
	const string key = "ataroutoyama";

	public void Add() {
		WWWForm form = new WWWForm();
		form.AddField("age", "20");
		form.AddField("name", "soya");
		StartCoroutine(PostData(form, "getId"));

	}

	IEnumerator PostData (WWWForm form, string root) {
		string url = serverUrl + root + "/" + "?key=" + key;
		using(UnityWebRequest www = UnityWebRequest.Post(url, form))
		{
			www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
			yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
				string data = www.downloadHandler.text;
            	Debug.Log(data);
                Debug.Log("Form upload complete!");
            }
		}
	}
}
