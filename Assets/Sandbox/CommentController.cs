using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	[SerializeField]
	ModeManager modeManager;

	// Use this for initialization
	void Start () {
		addButton.OnClickAsObservable().Subscribe(_ => {
			CommentDataTest data = new CommentDataTest(gridView.gridData);
			data.user_id = 1;
			data.comment = "from unity";
			data.id = 10;
			if (modeManager.Mode == 0) {
				commentManager.Add(data);
			} 
		});
		gridView.OnEndDragAsObservable.Subscribe(_ => {
				if (modeManager.Mode == 1) {
					SearchComment(gridView.gridData);
				}
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
