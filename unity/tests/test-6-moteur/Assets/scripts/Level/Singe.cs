﻿using UnityEngine;
using System.Collections;

public class Singe : MonoBehaviour, LevelScriptedReceiver {

	public int typeListen;
	public LevelScriptedNotifier levelScripted;

	void Start() {
		levelScripted.connect (this);
	}

	public void onEventType (int type) {
		if (typeListen == type) {
			foreach (Transform s1 in transform) {
				s1.GetComponent<Animator>().SetTrigger("jump");
			}
		}
	}

}
