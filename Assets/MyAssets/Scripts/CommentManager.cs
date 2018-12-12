using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;
using UniRx.Triggers;
using UniRx.WebRequest;

public class CommentManager : MonoBehaviour {
	const string serverUrl = "http://s-tsports.com:8082/";
	const string key = "ataroutoyama";
	public int workId = 9;
	List<CommentData> dataList;
	GridView gridView;

	public Subject<Unit> requestSubject;
	string requestText;

	// Use this for initialization
	void Start () {
		gridView = GameObject.Find("Grid").GetComponent<GridView>();
		dataList = new List<CommentData>();
	}

	public void Add(CommentData data) {
		WWWForm form = new WWWForm();
		form.AddField("userId", data.user_id);
		form.AddField("id", this.workId);
		form.AddField("leftTopX", data.left_top_x);
		form.AddField("leftTopY", data.left_top_y);
		form.AddField("rightBottomX", data.right_bottom_x);
		form.AddField("rightBottomY", data.right_bottom_y);
		form.AddField("opinion", data.comment);
		StartCoroutine(PostData(form, "tweet"));

	}

	 public IObservable<List<ReceiveData>> GetLatest(){
		Dictionary<string,string> query = new Dictionary<string, string>();
		query.Add("id", this.workId.ToString());
		return GetData("tweet", query).Select((x,ex) => {
			List<ReceiveData> datalist;
			Debug.Log(x);
			datalist = JsonHelper.ListFromJson<ReceiveData>(x);
			return datalist;
		});

	 }

	public IObservable<List<ReceiveData>> Search(GridData data){
		Dictionary<string,string> query = new Dictionary<string, string>();
		query.Add("id", this.workId.ToString());
		query.Add("leftTopX", data.left_top_x.ToString());
		query.Add("leftTopY", data.left_top_y.ToString());
		query.Add("rightBottomX", data.right_bottom_x.ToString());
		query.Add("rightBottomY", data.right_bottom_y.ToString());
		return GetData("search", query).Select((x,ex) => {
			List<ReceiveData> datalist;
			Debug.Log(workId);
			datalist = JsonHelper.ListFromJson<ReceiveData>(x);
			return datalist;
		});
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
                Debug.Log("Form upload complete!");
            }
		}
	}
	
	IObservable<string> GetData(string root, Dictionary<string,string> query) {
		string url = serverUrl + root + "/" + "?key=" + key;
		foreach(KeyValuePair<string,string> item in query) {
			url += "&" + item.Key + "=" + item.Value;
		}
		Debug.Log(url);
		return ObservableWebRequest.Get(url);
	}
}
