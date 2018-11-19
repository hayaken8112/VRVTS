using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentManagerOld : MonoBehaviour
{
	List<CommentDataOld> commentDataList;
    public GameObject textObj;
    Text commentText;
    

    // Use this for initialization
    void Start()
    {
		commentDataList = new List<CommentDataOld>();
        commentText = textObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddCommentData(CommentDataOld data) {
        commentDataList.Add(data);
    }
    public void UpdateBoard() {
        commentText.text = commentDataList[commentDataList.Count-1].data;

    }
}
