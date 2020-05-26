using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sound : MonoBehaviour {

	public List<AudioSource> sources = new List<AudioSource>();

	public AudioSource gong;
	public AudioSource card_flip;
	public AudioSource cardPlace;
	public AudioSource cardDeal;
	public AudioSource cardPickup;
	public AudioSource shuffle;
	public AudioSource bell;
	public AudioSource grumbleshake;
	public AudioSource slotOutcome;
	public AudioSource broken;
	public AudioSource duelSteal;
	public AudioSource duelThreaten;
	public AudioSource duelBribe;
	public AudioSource music;
	public AudioSource menuMusic;
	public List<AudioSource> rains = new List<AudioSource>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void PlayAudio(AudioSource aso){
		aso.Play();
	}

	public void PlayMusic(){
		music.Play();
	}

	public void StopAudio(AudioSource aso){
		aso.Stop();
	}

	public void PlayAudioPitched(AudioSource aso, float p){
		float orgp = aso.pitch;
		aso.pitch = p;
		aso.Play();
		StartCoroutine(WaitWithPitch(aso,orgp));
	}

	public void PlayRains(){
		int r = Random.Range(0,rains.Count);
		rains[r].Play();
	}

	public void PressGoButtonSound(){
		gong.Play();
	}

	IEnumerator WaitWithPitch(AudioSource s, float f){
		while(s.isPlaying){
			yield return new WaitForEndOfFrame();
		}
		s.pitch = f;
	}

	public void MuteSound(){
		if(AudioListener.volume != 0){
			AudioListener.volume = 0;
		}
		else{
			AudioListener.volume = 1;
		}
	}

	public void MuteMusic(){
		if(music.volume != 0){
			music.volume = 0;
			menuMusic.volume = 0;
		}
		else{
			music.volume = 1;
			menuMusic.volume = 1;
		}
	}


}
