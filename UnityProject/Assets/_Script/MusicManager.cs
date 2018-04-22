using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	public AudioClip MainMusicClip;
	public AudioClip DarkEventMusicClip;

	public float maxMusicVolume = 0.5f;
	public float timeToWaitInTheDark = 3.0f;
	public float fadeInTime = 2.0f;

	private AudioSource MainMusicSource;
	private AudioSource DarkEventMusicSource;

	enum audioStatus {
		happy,
		fadeIn,
		dark,
		fadeOut
	};

	private audioStatus currentAudioStatus = audioStatus.happy;

	private float timeInTheDark = 0.0f;

	// Use this for a dark music event ...
	public void PlayDarkEvent() {
		currentAudioStatus = audioStatus.fadeIn;
	}

	// Use this for initialization
	void Start () {

		gameObject.AddComponent<AudioSource>();
		gameObject.AddComponent<AudioSource>();

		AudioSource[] allAudioSources = GetComponents<AudioSource>();

		MainMusicSource = allAudioSources[0];

		MainMusicSource.clip = MainMusicClip;
		MainMusicSource.volume = 0.5f;
		MainMusicSource.Play();
		MainMusicSource.loop = true;

		DarkEventMusicSource = allAudioSources[1];

		DarkEventMusicSource.clip = DarkEventMusicClip;
		DarkEventMusicSource.volume = 0.0f;
		DarkEventMusicSource.Play();
		DarkEventMusicSource.loop = true;
	}
	
	// Update is called once per frame
	void Update () {
		/* This is Only for testing !!!
		if(Input.GetKeyDown(KeyCode.Space)) {
			PlayDarkEvent();
		}
		*/

		float fadeValue = maxMusicVolume / (fadeInTime / Time.deltaTime);

		if (currentAudioStatus == audioStatus.fadeOut) {
			if (MainMusicSource.volume < maxMusicVolume) {
				MainMusicSource.volume = MainMusicSource.volume + fadeValue;
			}

			if (DarkEventMusicSource.volume > 0) {
				DarkEventMusicSource.volume = DarkEventMusicSource.volume - fadeValue;
			}

			if (MainMusicSource.volume == 0) {
				currentAudioStatus = audioStatus.happy;
			}
		}

		if (currentAudioStatus == audioStatus.dark) {
			timeInTheDark = timeInTheDark + Time.deltaTime;
			if (timeInTheDark > timeToWaitInTheDark) {
				currentAudioStatus = audioStatus.fadeOut;
			}
		}

		if (currentAudioStatus == audioStatus.fadeIn) {

			if (DarkEventMusicSource.volume < maxMusicVolume) {
				DarkEventMusicSource.volume = DarkEventMusicSource.volume + fadeValue;
			}

			if (MainMusicSource.volume > 0) {
				MainMusicSource.volume = MainMusicSource.volume - fadeValue;
			}

			if (MainMusicSource.volume == 0) {
				currentAudioStatus = audioStatus.dark;
				timeInTheDark = 0.0f;
			}
		}
	}
}
