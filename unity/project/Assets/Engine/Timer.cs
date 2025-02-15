using UnityEngine;
using System.Collections;

public abstract class Timer : HelikoObject {

	public bool startCountAtLoad = true;
	protected bool stopIt = false;
	private bool loop;

	public AudioSource audioSource;
	public float loopTime = 30f; // Temps d'attente entre chaque boucle en MS

	private float nextBeatSample; // Le numéro du prochain sample à attendre pour un nouveau beat
	protected float samplePeriod; // Le temps en samples d'un beat
	protected float sampleDelay; // Le temps d'attente en sample avant de compter le premier beat

	protected MusicTempo music;

	private int myMsDelayStartCount = 0;
	private int myDelayTicks = 0;
	private int nBeat = 0;

	// Methode à implémenter, décrira le comportement lors d'un beat
	protected abstract void beat();

	// Methode à implémenter, décrira le comportement à la fin de la musique
	protected abstract void endMusic();

	protected void setSampleDelay(int msDelayStartCount, int delayTicks) {
		myMsDelayStartCount = msDelayStartCount;
		myDelayTicks = delayTicks;
	}

	public MusicTempo getMusic() {
		return music;
	}

	// Retourne le numéro du beau
	public int getNBeat() {
		return this.nBeat;
	}

	public new void Start() {
		if (isStart) return;

		base.Start();

		if (constantes.instantCalcul)
		loopTime = 0;

		music = audioSource.GetComponent<MusicTempo>();

		if (music == null) {
			Debug.LogError ("MusicTempo component not found");
			return;
		}

		float delayMS = myMsDelayStartCount + (myDelayTicks*music.GetTimePeriod()*1000f);

		sampleDelay = ((float) delayMS / 1000f) * music.GetFrequency();
		samplePeriod = music.GetSamplePeriod();
		nextBeatSample = sampleDelay;

		if (startCountAtLoad) {
			StartCount();
		} 
	}

	// Lance le beater
	public void StartCount() {
		audioSource.Play();
		StartCoroutine(BeatCheck());
	}

	// Stop complétement le thread de comptage
	public void stopCount() {
		this.stopIt = true;
	}

	// Remet la musique au début en réinitialisant toutes les valeurs
	public void reset() {
		Debug.Log("reset!");
		audioSource.Stop();
		audioSource.Play();
		nBeat = 0;

		float delayMS = myMsDelayStartCount;
		sampleDelay = ((float) delayMS / 1000f) * music.GetFrequency();
		samplePeriod = music.GetSamplePeriod();
		nextBeatSample = sampleDelay;
	}

	// Active ou desactive le bouclage de la musique
	public void setLoop(bool loop) {
		this.loop = loop;
	}

	// Thread principal qui se chargera d'envoyer les beats au bon moment
	IEnumerator BeatCheck () {
		while (!stopIt) {
			if (audioSource.isPlaying) {
				float currentSample = audioSource.timeSamples;
				if (currentSample >= (nextBeatSample)) {
					this.beat();
					nBeat++;
					nextBeatSample += samplePeriod;
				}
			}
			if (audioSource.timeSamples + 1000 >= audioSource.clip.samples) {
				if(loop) {
					reset();
				} else {
					this.endMusic();
				}
			}
			yield return new WaitForSeconds(loopTime / 1000f);
		}
	}
}
