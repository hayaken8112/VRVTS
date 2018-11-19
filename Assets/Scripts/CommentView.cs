using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

public class CommentView : MonoBehaviour {

	int cell_num = 6;
	public GameObject commentCellPrefab;
	List<CommentCell> cellList;
	public Subject<CommentData> PointerEnterSubject = new Subject<CommentData>();

	public IObservable<CommentData> OnCommentOver
	{
		get {return PointerEnterSubject;}
	}
	public Subject<Unit> PointerOutSubject = new Subject<Unit>();
	public IObservable<Unit> OnCommentOut
	{
		get {return PointerOutSubject;}
	}

	// Use this for initialization
	void Start () {
		cellList = new List<CommentCell>();
		for (int i = 0; i < cell_num; i++) {
			GameObject cellObj = Instantiate(commentCellPrefab, this.transform);
			CommentCell cell  = cellObj.GetComponent<CommentCell>();
			cellList.Add(cell);
		}

	}

	
	void UpdateView() {
		
	}

	public void SetData(IEnumerable<CommentData> list) {
		var to_set_data = list.Take(cell_num).ToList();
		for (int i = 0; i < cell_num; i++) {
			if (i < to_set_data.Count) {
				cellList[i].SetCell(to_set_data[i]);
			} else {
				cellList[i].DeleteCell();
			}
		}
	}
}
