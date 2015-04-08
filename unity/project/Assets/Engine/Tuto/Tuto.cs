﻿using UnityEngine;
using System.Collections;

public class Tuto : HelikoObject {

	public StepTuto[] steps;
	int nStep = 0;
	public int idGame;

	public new void Start() {
		base.Start ();
		initTuto();
		startTuto();
	}

	public void initTuto() {
		foreach (StepTuto t in steps) {
			t.init(this);
		}
	}

	public void startTuto() {
		next();
	}

	public void next() {
		if (nStep >= steps.Length) {
			startLevel();
		}
		steps[nStep].play();
		nStep++;
	}

	private void startLevel() {
		this.GetComponent<LoadOnClick>().LoadScene(constantes.getNumSceneFromIdMiniGame(idGame));
	}

	public void skipTuto() {
		this.startLevel();
	} 

}
