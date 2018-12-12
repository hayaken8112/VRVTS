using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRKetboardField : InputField {

	public override void OnSelect(BaseEventData eventData){
		Debug.Log("EventSystem作動");
		GameObject sceneObj = GameObject.Find("SceneObj");
		sceneObj.GetComponent<MakeKeyboard>().CreateKeyboard(this);
	}

}
