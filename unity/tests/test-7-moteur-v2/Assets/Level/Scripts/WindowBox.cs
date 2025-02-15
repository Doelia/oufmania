﻿using UnityEngine;
using System.Collections;

public class WindowBox : MonoBehaviour, TempoReceiver {

	public WindowOpen windowOpen;
	public WindowClose windowClose;

	public void Start () {
		this.windowOpen.connect(this);
		this.windowClose.connect(this);
	}
	
	void Update () {
		
	}

	public void onStep () {
	}

	public void onSuccessWindowExit () {
		this.renderer.material.color = new Color (1, 0, 0);
	}

	public void onSuccessWindowEnter () {
		this.renderer.material.color = new Color (0, 1, 0);
	}
}
