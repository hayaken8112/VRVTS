using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MakeKeyboard : MonoBehaviour {

	public GameObject keyButton;
	InputField demoField;
	private static char[] c;

	public GameObject canvas;

	public void CreateKeyboard(InputField selectedIF) {
		Debug.Log("押された！！！！！！！");

		demoField = selectedIF;
		Vector3 pos = selectedIF.transform.position;
		pos.y = pos.y - 350f;

		c = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '←', 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M'};
		canvas.transform.position = pos;
		canvas.SetActive(true);
		RectTransform content = canvas.GetComponent<RectTransform>();

		for (int i = 0; i < c.Length; i++) {
			char cs = c[i];
			GameObject btn = (GameObject)Instantiate(keyButton);
			btn.transform.GetComponentInChildren<Text>().text = c[i].ToString();
			if (0 <= i && i < 10) {
				//content.sizeDelta = new Vector2(-300 + i * 55, 100);
				btn.transform.position = new Vector2(-275 + i * 55, 85);
				btn.transform.SetParent(content, false);
				btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(cs));
			}else if (i == 10) {
				btn.transform.position = new Vector2(-275 + i * 55, 85);
				btn.transform.SetParent(content, false);
				btn.transform.GetComponent<Button>().onClick.AddListener(() => DeleteChar());
			} else if (11 <= i && i < 21) {
				//content.sizeDelta = new Vector2(-275 + i * 55, 40);
				btn.transform.position = new Vector2(-855 + i * 55, 25);
				btn.transform.SetParent(content, false);
				btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(cs));
			} else if (21 <= i && i < 30) {
				//ontent.sizeDelta = new Vector2(-250 + i * 55, -20);
				btn.transform.position = new Vector2(-1380 + i * 55, -35);
				btn.transform.SetParent(content, false);
				btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(cs));
			} else {
				//content.sizeDelta = new Vector2(-225 + i * 55, -80);
				btn.transform.position = new Vector2(-1825 + i * 55, -115);
				btn.transform.SetParent(content, false);
				btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(cs));
			}
		}
	}

	public void OnClick(char cs) {
		demoField.text = demoField.text + cs;
		demoField.ActivateInputField();
		demoField.selectionColor = Color.clear;
	}

	public void DeleteChar() {
		if (demoField.text.Length > 0){
			demoField.text = demoField.text.Substring(0, demoField.text.Length - 1);
			demoField.ActivateInputField();
		}
	}
}
