﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ModeManager : MonoBehaviour {
	private int mode = 0; // 0:comment, 1:view
	public ReactiveProperty<int> Mode;
	[SerializeField]
	Color CommentColor;
	[SerializeField]
	Color ViewColor;
	[SerializeField]
	GameObject modeButtonObj;

	// Use this for initialization
	void Start () {
		Button modeButton = modeButtonObj.GetComponent<Button>();
		Mode = new ReactiveProperty<int>(mode);
		modeButton.OnClickAsObservable().Subscribe(_ => {
			SwitchMode();
			Mode.Value= mode;
		});
		
	}

	void SetupComment(){
		
	}

	public void SwitchMode() {
		mode = (mode + 1) % 2;
		switch (mode) {
			case 0:
				modeButtonObj.GetComponent<Image>().color = CommentColor;
				modeButtonObj.GetComponentInChildren<Text>().text = "Comment";
				break;
			case 1:
				modeButtonObj.GetComponent<Image>().color = ViewColor;
				modeButtonObj.GetComponentInChildren<Text>().text = "View";
				break;
			default:
				modeButtonObj.GetComponent<Image>().color = CommentColor;
				modeButtonObj.GetComponentInChildren<Text>().text = "Comment";
				break;
		}
	}
}
