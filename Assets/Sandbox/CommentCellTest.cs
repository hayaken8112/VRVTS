using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class CommentCellTest : MonoBehaviour {

	int id = -1;
	Image image;
	Text txt;
	// Use this for initialization
	void Start () {
		// CommentManagerTest commentManager = this.GetComponentInParent<CommentManagerTest>();
		// var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		// eventTrigger.OnPointerEnterAsObservable().Where(_ => id != -1).Subscribe(_ => commentManager.OnCellEnter(id));
		// eventTrigger.OnPointerExitAsObservable().Where(_ => id != -1).Subscribe(_ => commentManager.OnCellExit());
		// eventTrigger.OnPointerClickAsObservable().Where(_ => id != -1).Subscribe(_ => commentManager.SelectCells(id));
		image = this.GetComponent<Image>();
		txt = this.GetComponentInChildren<Text>();
		DeleteCell();
	}

	public void SetCell(CommentDataTest data) {
		id = data.id;
		image.color = Color.white;
		txt.text = data.id.ToString() + ":"+ data.comment;
	}

	public void DeleteCell() {
		id = -1;
		image.color = Color.gray;
		txt.text = "";
	}

	Color SetColor(int n) {
		n = n % 3;
		switch(n)
		{
			case 0:
				return Color.red;
			case 1:
				return Color.blue;
			case 2:
				return Color.green;
			default:
				return Color.white;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
