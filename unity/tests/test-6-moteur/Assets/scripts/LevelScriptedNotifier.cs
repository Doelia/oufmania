﻿using UnityEngine;
using System.Collections;

/*
 * Contient un script de niveau, reçoit les évenements rythmique du BPMControlor
 * 
 */

public class LevelScriptedNotifier : TempoReceiver
{

	public TextAsset levelData;
	private int[] stepEvents;
	private int eventIndex;
	public BPMControlor bpm;
	public bool loop;
	private bool successThisStep;
	private bool isInWindow;

	ArrayList observers;

	public void Awake ()
	{
		this.observers = new ArrayList ();
		loop = true;
		successThisStep = false;
	}

	public void Start ()
	{
		loadData ();
	}
	
	public void connect (LevelScriptedReceiver r)
	{
		this.observers.Add (r);
	}

	public void loadData ()
	{
		Debug.Log (levelData.text);
		string [] tracks = levelData.text.Split ('\n');
		stepEvents = stringToIntEvents (tracks);
	}

	private int [] stringToIntEvents (string[] s)
	{
		string [] steps = s [0].Split (' ');
		string [] halfSteps = s [1].Split (' ');
		int [] toReturn = new int[steps.Length * 2];
		for (int i = 0; i < steps.Length; i++) {
			toReturn [i * 2] = int.Parse (steps [i]);
			toReturn [i * 2 + 1] = int.Parse (halfSteps [i]);
		}

		for (int i = 0; i < steps.Length * 2; i++) {
			Debug.Log (i + " : " + toReturn [i]);
		}

		return toReturn;
	}

	private void notifyChildren (int type)
	{
		foreach (LevelScriptedReceiver e in this.observers) {
			e.onEventType (type);
		}
	}

	private void notifyChildrenOfFailure ()
	{
		foreach (LevelScriptedReceiver e in this.observers) {
			e.onFailure ();
		}
	}
	                                
	public override void onStep ()
	{
		notifyChildren (stepEvents [eventIndex]);
	}

	public bool isGood (int type)
	{
		if (successThisStep) {
			return true;
		}

		if (this.bpm.timeIsInWindow () && stepEvents [eventIndex] == type) {
			successThisStep = true;
			Debug.Log ("good");
			return true;
		}
		Debug.Log ("bad");
		return false;
	}

	public override void onSuccessWindowExit ()
	{
		if (stepEvents [eventIndex] != 0) {
			if (!successThisStep) {
				notifyChildrenOfFailure ();
			}
			isInWindow = false;
		}
	}

	public override void onSuccessWindowEnter ()
	{
		eventIndex++;
		if (loop && eventIndex >= stepEvents.Length) {
			eventIndex = 0;
		}

		successThisStep = false;
		isInWindow = true;
	}
}
