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
	List<CommentData> dataList;
	GridView gridView;

	public Subject<Unit> requestSubject;
	string requestText;

	// Use this for initialization
	void Start () {
		gridView = GameObject.Find("Grid").GetComponent<GridView>();
		dataList = new List<CommentData>();
	}

	public void AddData(CommentData data) {
		data.id = dataList.Count;
		dataList.Add(data);
		// SetData(dataList);
	}
	public void SelectCells(int id) {
		var data = dataList.Where(x => x.id % 3 == id % 3);
		// SetData(data);
	}
	public void SelectCellsByArea(int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y) {
		var data = dataList.Where(x => IsInArea(x, left_top_x, left_top_y, right_bottom_x, right_bottom_y));
		// SetData(data);

	}

	public void OnCellEnter(int id) {
		CommentData data = dataList[id];
		gridView.SelectCells(data.left_top_x, data.left_top_y, data.right_bottom_x, data.right_bottom_y);

	}
	bool IsInArea(CommentData data, int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y) {

		return data.left_top_x >= left_top_x && data.left_top_y >= left_top_y && data.right_bottom_x <= right_bottom_x && data.right_bottom_y <= right_bottom_y;
	}
	public void OnCellExit() {
		gridView.ResetCells();
	}
	public void Add(CommentData data) {
		WWWForm form = new WWWForm();
		form.AddField("userId", data.user_id);
		form.AddField("id", data.id);
		form.AddField("leftTopX", data.left_top_x);
		form.AddField("leftTopY", data.left_top_y);
		form.AddField("rightBottomX", data.right_bottom_x);
		form.AddField("rightBottomY", data.right_bottom_y);
		form.AddField("opinion", data.comment);
		StartCoroutine(PostData(form, "tweet"));

	}

	 public void GetLatest(){

	 }
	 public void FilterById(int id) {
		Dictionary<string,string> query = new Dictionary<string, string>();
		query.Add("id", id.ToString());
		// StartCoroutine(GetData("search", query));
	 }
	public IObservable<List<ReceiveData>> Search(GridData data){
		Dictionary<string,string> query = new Dictionary<string, string>();
		query.Add("id", 10.ToString());
		query.Add("leftTopX", data.left_top_x.ToString());
		query.Add("leftTopY", data.left_top_y.ToString());
		query.Add("rightBottomX", data.right_bottom_x.ToString());
		query.Add("rightBottomY", data.right_bottom_y.ToString());
		return GetData("search", query).Select((x,ex) => {
			List<ReceiveData> datalist;
			Debug.Log(x);
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
