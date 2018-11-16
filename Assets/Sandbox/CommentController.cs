using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CommentController : MonoBehaviour {
	[SerializeField]
	CommentManagerTest commentManager;

	[SerializeField]
	GridView gridView;

	[SerializeField]
	Button addButton;
	[SerializeField]
	CommentView commentView;

	// Use this for initialization
	void Start () {
		addButton.OnClickAsObservable().Subscribe(_ => {
			CommentDataTest data = new CommentDataTest(gridView.gridData);
			data.user_id = 1;
			data.comment = "from unity";
			data.id = 10;
			commentManager.Add(data);
			// commentManager.FilterById(10);
		});
		
	}
	
	// Update is called once per frame
	public void UpdateView(List<CommentDataTest> dataList) {
		commentView.SetData(dataList);
	}
}
