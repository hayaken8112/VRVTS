using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommentView : MonoBehaviour {

	int cell_num = 6;
	public GameObject commentCellPrefab;
	List<CommentCellTest> cellList;

	// Use this for initialization
	void Start () {
		cellList = new List<CommentCellTest>();
		for (int i = 0; i < cell_num; i++) {
			GameObject cellObj = Instantiate(commentCellPrefab, this.transform);
			CommentCellTest cell  = cellObj.GetComponent<CommentCellTest>();
			cellList.Add(cell);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void UpdateView() {
		
	}

	public void SetData(IEnumerable<CommentDataTest> list) {
		var to_set_data = list.Take(cell_num).ToList();
		for (int i = 0; i < cell_num; i++) {
			if (i < to_set_data.Count) {
				cellList[i].SetCell(to_set_data[i].id);
			} else {
				cellList[i].DeleteCell();
			}
		}
	}
}
