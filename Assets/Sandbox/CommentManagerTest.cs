using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class CommentManagerTest : MonoBehaviour {
	List<CommentDataTest> dataList;
	List<CommentCellTest> cellList;
	public GameObject commentCellPrefab;
	GridManager gridManager;
	public int cell_num = 6;

	// Use this for initialization
	void Start () {
		gridManager = GameObject.Find("Grid").GetComponent<GridManager>();
		cellList = new List<CommentCellTest>();
		dataList = new List<CommentDataTest>();
		for (int i = 0; i < cell_num; i++) {
			GameObject cellObj = Instantiate(commentCellPrefab, this.transform);
			CommentCellTest cell  = cellObj.GetComponent<CommentCellTest>();
			cellList.Add(cell);
		}
		
	}

	public void AddData(CommentDataTest data) {
		data.id = dataList.Count;
		dataList.Add(data);
		SetData(dataList);
	}
	void SetData(IEnumerable<CommentDataTest> list) {
		var to_set_data = list.Take(cell_num).ToList();
		for (int i = 0; i < cell_num; i++) {
			if (i < to_set_data.Count) {
				cellList[i].SetCell(to_set_data[i].id);
			} else {
				cellList[i].DeleteCell();
			}
		}
	}
	public void SelectCells(int id) {
		var data = dataList.Where(x => x.id % 3 == id % 3);
		SetData(data);
	}
	public void SelectCellsByArea(int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y) {
		var data = dataList.Where(x => IsInArea(x, left_top_x, left_top_y, right_bottom_x, right_bottom_y));
		SetData(data);

	}

	public void OnCellEnter(int id) {
		CommentDataTest data = dataList[id];
		gridManager.SelectCells(data.left_top_x, data.left_top_y, data.right_bottom_x, data.right_bottom_y);

	}
	bool IsInArea(CommentDataTest data, int left_top_x, int left_top_y, int right_bottom_x, int right_bottom_y) {

		return data.left_top_x >= left_top_x && data.left_top_y >= left_top_y && data.right_bottom_x <= right_bottom_x && data.right_bottom_y <= right_bottom_y;
	}
	public void OnCellExit() {
		gridManager.ResetCells();
	}
	
}
