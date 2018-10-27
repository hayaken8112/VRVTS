using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Facilitater : MonoBehaviour {
	int Mode = 0; // 0:comment, 1:view
	[SerializeField]
	Color CommentColor;
	[SerializeField]
	Color ViewColor;
	GameObject modeButtonObj;

	// Use this for initialization
	void Start () {
		modeButtonObj = GameObject.Find("ModeButton");
		Button modeButton = modeButtonObj.GetComponent<Button>();
		modeButton.OnClickAsObservable().Subscribe(_ => {
			SwitchMode();
		});
		
	}

	void SetupComment(){
		
	}
	public void SwitchMode() {
		switch (Mode) {
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
		Mode = (Mode + 1) % 2;
	}
}
