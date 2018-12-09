using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class RegisterUser : MonoBehaviour {

	public InputField nameIF;

	public static string userName;
	public GameObject keyboardCanvas;

	void Start () {
		keyboardCanvas.SetActive(false);
		
		foreach(InputField vrKeyboard in Resources.FindObjectsOfTypeAll(typeof(InputField))){
			var trigger = vrKeyboard.gameObject.AddComponent<EventTrigger>();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((eventData) => { EditIF(vrKeyboard); });
			trigger.triggers.Add(entry);
		}
		
	}

	public void EditIF(InputField enterIF){
		GameObject sceneObj = GameObject.Find("SceneObj");
		sceneObj.GetComponent<MakeKeyboard>().CreateKeyboard(enterIF);
	}

	public void CanvasTapped(){
		//Debug.Log("ここは押してるんやで");
		PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, result);

		DisableKeyboard();

		/*

        foreach (RaycastResult raycastResult in result)
        {
            if(raycastResult.gameObject.GetComponent<InputField>()){
				Debug.Log("それはinputfieldや");
			}else{
				Debug.Log("それはちゃうなぁ");
			}
        }

		*/
	}

	public void DisableKeyboard() {
		//Debug.Log("キーボードを消します");
		keyboardCanvas.SetActive(false);
		
	}

	public void Regsiter(){

		//Debug.Log("押された！");

		userName = nameIF.text;

		if (userName == ""){
			Debug.Log("名前が入力されていません");
			return;
		}
		Add(userName);
	}

	const string serverUrl = "http://s-tsports.com:8082/";
	const string key = "ataroutoyama";

	public void Add(string name) {
		WWWForm form = new WWWForm();
		form.AddField("age", "20");
		form.AddField("name", name);
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
				string userId = "";
				string data = www.downloadHandler.text;
				string[] strList = data.Split(',');
				foreach(string str in strList){
					string[] strListSub = str.Split(':');
					if (strListSub[0] == "{\"userId\""){
						userId = strListSub[1];
						break;
					}
				}
				int userIdInt = int.Parse(userId);
				
				SqliteDatabase sqlDB = new SqliteDatabase("config.db");
				string query = "create table user(userId ingeger, name string)";
				sqlDB.ExecuteNonQuery(query);
				string querysecond = "insert into user values(" + userIdInt + ", '" + userName + "')";
        		sqlDB.ExecuteNonQuery(querysecond);
				
				//Debug.Log(data);
                Debug.Log("Form upload complete!");
            }
		}
	}
}

