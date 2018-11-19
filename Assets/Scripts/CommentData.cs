using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommentData{
	public CommentData(GridData gridData){
		this.left_top_x = gridData.left_top_x;
		this.left_top_y = gridData.left_top_y;
		this.right_bottom_x = gridData.right_bottom_x;
		this.right_bottom_y = gridData.right_bottom_y;
	}
	public CommentData(int id, int user_id, int work_id, int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y, string comment){
		this.id = id;
		this.user_id = user_id;
		this.work_id = work_id;
		this.left_top_x = left_top_x;
		this.left_top_y = left_top_y;
		this.right_bottom_x = right_bottom_x;
		this.right_bottom_y = right_bottom_y;
		this.comment = comment;
	}
		public int id;
		public int user_id = 0;
		public int work_id;
		public int left_top_x;
		public int left_top_y;
		public int right_bottom_x;
		public int right_bottom_y;
		public string comment;
}

public class GridData {
	public GridData(int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y){
		this.left_top_x = left_top_x;
		this.left_top_y = left_top_y;
		this.right_bottom_x = right_bottom_x;
		this.right_bottom_y = right_bottom_y;
	}
	public int left_top_x;
	public int left_top_y;
	public int right_bottom_x;
	public int right_bottom_y;
}