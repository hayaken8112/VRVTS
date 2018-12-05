using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageUpdater : MonoBehaviour {
	public ModeManager modeManager;
	public MaterialUI.TabView tabView;
	int previousPage;

	// Use this for initialization
	void Start () {
		previousPage = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (previousPage != tabView.currentPage) {
			modeManager.Mode.Value = tabView.currentPage;
			previousPage = tabView.currentPage;
		}
		
	}
}
