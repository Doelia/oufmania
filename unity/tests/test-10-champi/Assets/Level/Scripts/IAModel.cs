﻿using UnityEngine;
using System.Collections;

public class IAModel : MonoBehaviour, LevelScriptedReceiver {

	private Animator brasDroit;
	private Animator brasGauche;
	public LevelScripted level;

	public Transform brasGaucheTrasform;
	public Transform brasDroitTrasform;
	public Transform carapace;

	public void Start () {
		this.level.connect(this);
		brasDroit = brasDroitTrasform.GetComponent<Animator>();
		brasGauche = brasGaucheTrasform.GetComponent<Animator>();
	}
	
	public void onAction (int type) {
		if (type == 1) {
			brasDroit.SetTrigger ("Down");
		} else if (type == 2) {
			brasGauche.SetTrigger ("Down");
		} else if (type == 3) {
			brasDroit.SetTrigger ("Down");
			brasGauche.SetTrigger ("Down");
		}
		carapace.GetComponent<Animator>().SetTrigger("Move");
		this.GetComponent<AudioSource>().Play();
		
	}

	public void onFailure() {
		
	}

}
