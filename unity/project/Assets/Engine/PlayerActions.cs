using UnityEngine;
using System.Collections;

public class PlayerActions : HelikoObject, LevelScriptedReceiver, TempoReceiver {

	public LevelScripted level; // Utilisé pour le connect et pour tester le good

	private ArrayList successStep;
	private int failuresCount; // Nombre de fail total
	private int stepsCount;
	private int nbrFailInLoop = 0; // Nombre de fail dans une loop
	private ArrayList observers;

	private BeatCounter bc;

	public void Awake () {
		this.observers = new ArrayList ();
		this.successStep = new ArrayList();
		failuresCount = 0;
		stepsCount = 0;
		nbrFailInLoop = 0;
	}

	public new void Start() {
		base.Start();
		if (level != null)
			this.level.connect(this);
		bc = GetBeatCounter();
		bc.Connect(this);
	}

	public void Connect (PlayerActionReceiver r) {
		this.observers.Add(r);
	}

	public void Disconnect(PlayerActionReceiver r) {
		this.observers.Remove(r);
	}

	public bool IsGood (int type) {
		int stepTapped = bc.getStepClosest();
		if (level.getActionFromBeat(stepTapped) == type) {
			this.successStep.Add(stepTapped);
			notifySucces();
			return true;
		} else if (level.getActionFromBeat(stepTapped-1) == type ) {
			this.successStep.Add(stepTapped-1);
			notifySucces();
			return true;
		} else {
			if (level.getActionFromBeat(stepTapped+1) == type ) {
				this.successStep.Add(stepTapped+1);
				notifySucces();
				return true;
			}
		}
		addFail();
		return false;
	}

	private void addFail() {
		nbrFailInLoop++;
		failuresCount++;
		this.notifyFailure();
	}

	public void notifyFailure() {
		foreach (PlayerActionReceiver e in this.observers) {
			e.OnFailure();
		}
	}

	public void notifySucces() {
		foreach (PlayerActionReceiver e in this.observers) {
			e.OnSuccess();
		}
	}

	public void notifySuccesLoop() {
		foreach (PlayerActionReceiver e in this.observers) {
			e.OnSuccessLoop();
		}
	}

	public int getNumberOfSuccess() {
		return successStep.Count;
	}

	public int getScore() {
		return stepsCount - failuresCount;
	}

	public int getFailureCount() {
		return failuresCount;
	}

	public int getSuccessPercencage() {
		if(failuresCount == 0 || stepsCount == 0) {
			return 100;
		}
		return (int) (100 * Mathf.Max(0, (1.0f - ((float)failuresCount) / ((float)stepsCount))));
	}


	// EVENT RECEIVER
	public void OnAction(int action) {
		if (action == -1) { // Fin de la boucle
			if (nbrFailInLoop == 0) {
				notifySuccesLoop();
			}
			nbrFailInLoop = 0;
		} else {
			stepsCount++;
		}
	}
	
	public void OnStep (int nBeat) {
		if (nBeat > 2) {
			int previousStep = nBeat-2;
			if (level.isStepUseful (previousStep) && !successStep.Contains(previousStep)) {
				this.addFail();
			}
		}
	}

	public void OnEndMusic() { }

}
