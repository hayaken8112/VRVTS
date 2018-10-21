using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;


public class GridCell : MonoBehaviour {
	RawImage img;
	int cell_id_x;
	int cell_id_y;
	const float alphaOn = 0.8f;
	const float alphaSelect = 0.6f;
	const float alphaOff = 0.1f;
	bool isSelected = false;
	// Use this for initialization
	void Start () {
		img = this.GetComponent<RawImage>();
		GridManager gridManager = this.GetComponentInParent<GridManager>();
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerEnterAsObservable().Subscribe(_ => {
			TurnCellOn();
			gridManager.SetCurrentCell(cell_id_x, cell_id_y);
		});
		eventTrigger.OnPointerExitAsObservable().Subscribe(_ => {
			if(isSelected){
				SelectCell();
			} else {
				TurnCellOff();
			}
		});
		eventTrigger.OnPointerDownAsObservable().Subscribe(_ => {gridManager.isDragging.Value = true;});
		eventTrigger.OnPointerUpAsObservable().Subscribe(_ => {gridManager.isDragging.Value = false;});
		

	}
	public void SetCellID(int id_x, int id_y){
		cell_id_x = id_x;
		cell_id_y = id_y;
	}
	public void SelectCell() {
		Color c = img.color;
		c.a = alphaSelect;
		img.color = c;
		isSelected = true;
	}
	public void DeSelectCell() {
		Color c = img.color;
		c.a = alphaOff;
		img.color = c;
		isSelected = false;
	}
	
	public void TurnCellOn() {
		Color c = img.color;
		c.a = alphaOn;
		img.color = c;
	}
	public void TurnCellOff() {
		Color c = img.color;
		c.a = alphaOff;
		img.color = c;
	}
}
