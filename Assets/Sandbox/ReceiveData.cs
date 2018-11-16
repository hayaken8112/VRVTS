using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReceiveData {
		public int tweetId;
		public int userId;
		public string userName;
		public int id;
		public int userAge;
		public int leftTopX;
		public int leftTopY;
		public int rightBottomX;
		public int rightBottomY;
		public string opinion;
		public CommentDataTest ToCommentData() {
			CommentDataTest data = new CommentDataTest(tweetId, userId, id, leftTopX, leftTopY, rightBottomX, rightBottomY,opinion);
			return data;
		}
}
