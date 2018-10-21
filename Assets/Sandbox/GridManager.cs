using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GridManager : MonoBehaviour {

	public GameObject cellObjectPrefab;
	List<List<GridCell>> cellList;
	const float cellrate_x = 0.1f;
	const float cellrate_y = 0.1f;
	int cell_num_x;
	int cell_num_y;
	int start_cell_x;
	int start_cell_y;
	int end_cell_x;
	int end_cell_y;
	ReactiveProperty<int> current_cell_x;
	ReactiveProperty<int> current_cell_y;
	public ReactiveProperty<bool> isDragging;
	Button button;
	CommentManagerTest commentManager;
	// Use this for initialization
	void Start () {
		commentManager = GameObject.Find("CommentBoard").GetComponent<CommentManagerTest>();
		button = GameObject.Find("AddButton").GetComponent<Button>();
		GridLayoutGroup gridLayout = this.GetComponent<GridLayoutGroup>();
		RectTransform rectTransform = this.GetComponent<RectTransform>();
		gridLayout.cellSize = new Vector2(rectTransform.rect.width*cellrate_x, rectTransform.rect.height*cellrate_y);
		cell_num_x = (int)Mathf.Round(1.0f / cellrate_x);
		cell_num_y = (int)Mathf.Round(1.0f / cellrate_x);
		cellList = new List<List<GridCell>>(cell_num_y);
		for (int i = 0; i < cell_num_y; i++) {
			List<GridCell> cellRow = new List<GridCell>(cell_num_x);
			for (int j = 0; j < cell_num_x; j++) {
				GameObject cellObj = Instantiate(cellObjectPrefab, this.transform);
				GridCell cell = cellObj.GetComponent<GridCell>();
				cell.SetCellID(j,i);
				cellRow.Add(cell);
			}
			cellList.Add(cellRow);
		}
		
		isDragging = new ReactiveProperty<bool>(false);
		isDragging.Skip(1).Subscribe(flag => {
			if(flag){
				start_cell_x = current_cell_x.Value;
				start_cell_y = current_cell_y.Value;
			} else {
				end_cell_x = current_cell_x.Value;
				end_cell_y = current_cell_y.Value;
				SelectCells(start_cell_x, start_cell_y, end_cell_x, end_cell_y);
				commentManager.SelectCellsByArea(start_cell_x, start_cell_y, end_cell_x, end_cell_y);
			}
		});
		current_cell_x = new ReactiveProperty<int>(0);
		current_cell_y = new ReactiveProperty<int>(0);
		current_cell_x.Subscribe(_ => {
			if (isDragging.Value) {
				SelectCells(start_cell_x, start_cell_y, current_cell_x.Value, current_cell_y.Value);
			}
			Debug.Log(new Vector2Int(start_cell_x, start_cell_y).ToString() + new Vector2Int(current_cell_x.Value, current_cell_y.Value).ToString());
		});
		current_cell_y.Subscribe(_ => {
			if (isDragging.Value) {
				SelectCells(start_cell_x, start_cell_y, current_cell_x.Value, current_cell_y.Value);
			}
			Debug.Log(new Vector2Int(start_cell_x, start_cell_y).ToString() + new Vector2Int(current_cell_x.Value, current_cell_y.Value).ToString());
		});
		button.OnClickAsObservable().Subscribe(_ => {
			CommentDataTest data = new CommentDataTest(start_cell_x, start_cell_y, end_cell_x, end_cell_y);
			commentManager.AddData(data);
		});
	}

	public void SetCurrentCell(int id_x, int id_y) {
		current_cell_x.Value = id_x;
		current_cell_y.Value = id_y;
	}
	
	public void SelectCells(int start_x, int start_y, int end_x, int end_y) {
		for (int i = 0; i < cell_num_y; i++) {
			for (int j = 0; j < cell_num_x; j++) {
				if (start_x <= j && j <= end_x && start_y <= i && i <= end_y){
					cellList[i][j].SelectCell();
				} else {
					cellList[i][j].DeSelectCell();
				}
			}
		}
	}
	public void ResetCells(){
		for (int i = 0; i < cell_num_y; i++) {
			for (int j = 0; j < cell_num_x; j++) {
					cellList[i][j].DeSelectCell();
			}
		}
	}


	public void DebugCell(int id) {
		Debug.Log(id);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
