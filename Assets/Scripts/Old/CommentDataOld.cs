using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CommentDataOld {
	public CommentDataOld(string data, Vector2 pivot, Vector2 local_position, float width, float height){
		this.data = data;
		this.pivot = pivot;
		this.local_position = local_position;
		this.width = width;
		this.height = height;
	}
	public int id;
	public string data;
	public Vector2 pivot;
	public Vector2 local_position;
	public float width;
	public float height;
}
