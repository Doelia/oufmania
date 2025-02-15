﻿using UnityEngine;
using System.Collections;

public class MetronomePlayer : HelikoObject, LevelScriptedReceiver {

	private LevelScripted levelMetronome;
	public AudioSource snd;

	public new void Start() {
		if (isStart) return;
		base.Start ();
		levelMetronome = this.GetComponent<LevelScripted>();
		levelMetronome.connect(this);
	}

	public void OnAction(int type) {
		if (type == 1) {
			snd.Play();
		}
	}
}
