using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class CommentCell: MonoBehaviour {

	Image image;
	[SerializeField]
	Text mainText;
	[SerializeField]
	Text subText;
	CommentData commentData;
	CommentView commentView;
	// Use this for initialization
	void Start () {
		commentView = this.GetComponentInParent<CommentView>();
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		// eventTrigger.OnPointerClickAsObservable().Where(_ => id != -1).Subscribe(_ => commentManager.SelectCells(id));
		image = this.GetComponent<Image>();
		eventTrigger.OnPointerEnterAsObservable().Subscribe(_ => {
			if(commentData != null){
				commentView.PointerEnterSubject.OnNext(commentData);
			}
			});
		eventTrigger.OnPointerExitAsObservable().Subscribe(_ => {
			if(commentData != null){
				commentView.PointerOutSubject.OnNext(Unit.Default);
			}
			});
		DeleteCell();
	}

	public void SetCell(CommentData data) {
		commentData = data;
		image.color = Color.white;
		mainText.text = data.comment;
		subText.text = data.id.ToString();
	}

	public void DeleteCell() {
		image.color = Color.gray;
		commentData = null;
		mainText.text = "";
		subText.text = "";
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
