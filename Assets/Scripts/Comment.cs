using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CommentTest : MonoBehaviour {
	public int left_top_x;
	public int left_top_y;
	public int right_bottom_x;
	public int right_bottom_y;

	// Use this for initialization
	void Start () {
		GridView gridView = GameObject.Find("Grid").GetComponent<GridView>();
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerEnterAsObservable().Subscribe(_ => gridView.SelectCells(left_top_x, left_top_y, right_bottom_x,right_bottom_y));
		eventTrigger.OnPointerExitAsObservable().Subscribe(_ => gridView.ResetCells());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
