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
	const float alphaMark = 0.3f;
	const float alphaOff = 0.1f;
	int state = 0;
	// Use this for initialization
	void Start () {
		img = this.GetComponent<RawImage>();
		GridView gridView = this.GetComponentInParent<GridView>();
		var eventTrigger = this.gameObject.AddComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerEnterAsObservable().Subscribe(_ => {
			HighLightCell();
			gridView.SetCurrentCell(cell_id_x, cell_id_y);
		});
		eventTrigger.OnPointerExitAsObservable().Subscribe(_ => {
			SetCellState(state);
		});
		eventTrigger.OnPointerDownAsObservable().Subscribe(_ => {gridView.isDragging.Value = true;});
		eventTrigger.OnPointerUpAsObservable().Subscribe(_ => {gridView.isDragging.Value = false;});
		

	}
	public void SetCellID(int id_x, int id_y){
		cell_id_x = id_x;
		cell_id_y = id_y;
	}
	public void SetCellState(int n_state) {
		img.color = GetColor(n_state);
		state = n_state;
	}
	public void ResetCellState() {
		img.color = GetColor(0);
		state = 0;
	}
	public void ResetCell() {
		img.color = GetColor(state);
	}
	public void SelectCell() {
		img.color = GetColor(2);
	}
	public void DeSelectCell() {
		SetCellState(0);
	}
	public void HighLightCell() {
		img.color = GetColor(3);
	}
	
	Color GetColor(int state) {
		Color c = img.color;
		switch(state){
			case 0:
				c.a = alphaOff;
				return c;
			case 1:
				c.a = alphaMark;
				return c;
			case 2:
				c.a = alphaSelect;
				return c;
			case 3:
				c.a = alphaOn;
				return c;
			default:
				c.a = alphaOff;
				return c;
		}
	}
}
