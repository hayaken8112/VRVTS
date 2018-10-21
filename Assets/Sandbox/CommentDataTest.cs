using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommentDataTest {
	public CommentDataTest(int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y){
		this.left_top_x = left_top_x;
		this.left_top_y = left_top_y;
		this.right_bottom_x = right_bottom_x;
		this.right_bottom_y = right_bottom_y;
	}
		public int id;
		public int left_top_x;
		public int left_top_y;
		public int right_bottom_x;
		public int right_bottom_y;
}