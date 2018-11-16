﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class CommentController : MonoBehaviour {
	[SerializeField]
	CommentManagerTest commentManager;

	[SerializeField]
	GridView gridView;

	[SerializeField]
	Button addButton;
	[SerializeField]
	CommentView commentView;
	[SerializeField]
	ModeManager modeManager;
	public GameObject inputPanel;
	public InputField inputField;

	// Use this for initialization
	void Start () {
		addButton.OnClickAsObservable().Subscribe(_ => {
			if (modeManager.Mode.Value == 0) {
				CommentDataTest data = new CommentDataTest(gridView.gridData);
				data.user_id = 1;
				data.comment = inputField.text;
				data.id = 10;
				commentManager.Add(data);
				inputField.text = "";
				inputPanel.SetActive(false);
			} 
		});
		gridView.OnEndDragAsObservable.Subscribe(_ => {
				if (modeManager.Mode.Value == 0) {
					inputPanel.SetActive(true);
				} else {
					SearchComment(gridView.gridData);
				}
			});
		modeManager.Mode.Subscribe(mode => {
			inputPanel.SetActive(false);
		});
	}
	
	// Update is called once per frame
	public void UpdateView(IEnumerable<CommentDataTest> dataList) {
		commentView.SetData(dataList);
	}

	public void SearchComment(GridData data) {
		commentManager.Search(data).Subscribe(x => {
			var datalist = x.Select(i => i.ToCommentData());
			UpdateView(datalist);
		});
	}
}
