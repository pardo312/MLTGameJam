﻿using UnityEngine;
using UnityEngine.Audio;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }
	public Sound[] sounds;
	public AudioMixerGroup mixerGroup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
    }
    
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.Play();
	}
	public void StopPlaying (string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
		Debug.LogWarning("Sound: " + name + " not found!");
		return;
		}
		if(s.source.isPlaying)
		{
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

			s.source.Stop ();
		}
	}
	public void StopPlayingAll ()
	{
		foreach (Sound s in sounds)
		{
			if(s.source.isPlaying)
			{	
				s.source.Stop();
			}
		}
	}
}
