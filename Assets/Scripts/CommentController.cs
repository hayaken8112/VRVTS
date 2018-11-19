using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class CommentController : MonoBehaviour {
	[SerializeField]
	CommentManager commentManager;
	[SerializeField]

	GridView gridView;

	[SerializeField]
	CommentView commentView;
	[SerializeField]
	ModeManager modeManager;
	[SerializeField]
	InputPanel inputPanel;
	InputField inputField;
	Button addButton;

	// Use this for initialization
	void Start () {
		inputField = inputPanel.inputField;
		addButton = inputPanel.addButton;
		addButton.OnClickAsObservable().Subscribe(_ => {
			if (modeManager.Mode.Value == 0) {
				CommentData data = new CommentData(gridView.gridData);
				data.user_id = 1;
				data.comment = inputField.text;
				data.id = 10;
				commentManager.Add(data);
				inputField.text = "";
				inputPanel.gameObject.SetActive(false);
				UpdateComment();
			} 
		});
		modeManager.Mode.Subscribe(mode => {
			inputPanel.gameObject.SetActive(false);
			UpdateComment();
		});
		gridView.OnEndDragAsObservable.Subscribe(_ => {
				if (modeManager.Mode.Value == 0) {
					inputPanel.gameObject.SetActive(true);
				} else {
					SearchComment(gridView.gridData);
				}
			});
	}
	
	// Update is called once per frame
	public void UpdateView(IEnumerable<CommentData> dataList) {
		commentView.SetData(dataList);
	}
	public void UpdateComment(){
		commentManager.GetLatest().Subscribe(x => {
			var datalist = x.Select(i => i.ToCommentData());
			UpdateView(datalist);
		});
	}

	public void SearchComment(GridData data) {
		commentManager.Search(data).Subscribe(x => {
			var datalist = x.Select(i => i.ToCommentData());
			UpdateView(datalist);
		});
	}
}
