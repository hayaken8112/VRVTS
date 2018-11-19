using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GridController : MonoBehaviour {

	public CommentView commentView;
	public GridView gridView;
	// Use this for initialization
	void Start () {
		commentView.OnCommentOver.Subscribe(cd => {
			gridView.HighLightCells(cd.left_top_x,cd.left_top_y,cd.right_bottom_x, cd.right_bottom_y);
		});
		commentView.OnCommentOut.Subscribe(_ => {
			gridView.ResetAllCells();
		});
		
	}
	
}
