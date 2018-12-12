using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ModeManager : MonoBehaviour {
	private int mode = 0; // 0:comment, 1:view
	public ReactiveProperty<int> Mode;

	// Use this for initialization
	void Start () {
		Mode = new ReactiveProperty<int>(mode);
	}

}
