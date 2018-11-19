using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class VtsController : MonoBehaviour {
	[SerializeField]
	GameObject artworkObj;
	GridView gridView;
	[SerializeField]
	Button nextButton;
	int imgIndex = 0;
	public int ImgId = 10;
	public List<Sprite> images;
	public List<Dictionary<string,int>> sizes;
	[SerializeField]
	CommentManager commentManager;
	Dictionary<string,int> size0 = new Dictionary<string,int>(){
		{"width", 180},
		{"height", 250},
		{"Id", 10}
	};
	Dictionary<string,int> size1 = new Dictionary<string,int>(){
		{"width", 600},
		{"height", 400},
		{"Id", 9}
	};
	Dictionary<string,int> size2 = new Dictionary<string,int>(){
		{"width", 200},
		{"height", 300},
		{"Id", 8}
	};


	// Use this for initialization
	void Start () {
		RectTransform artworkRect = artworkObj.GetComponent<RectTransform>();		
		Image artworkImg = artworkObj.GetComponent<Image>();
		sizes = new List<Dictionary<string,int>>(){
			size0,size1,size2
		};
		gridView = artworkObj.GetComponentInChildren<GridView>();
		nextButton.OnClickAsObservable().Subscribe(_ => {
			imgIndex = (imgIndex + 1) % 3;
			artworkImg.sprite = images[imgIndex];
			artworkRect.sizeDelta = new Vector2(sizes[imgIndex]["width"],sizes[imgIndex]["height"]);
			gridView.InitGrid();
			commentManager.workId = sizes[imgIndex]["Id"];
		});
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
