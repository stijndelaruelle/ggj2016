using UnityEngine;
using System.Collections;

public class PlayerAudio : MonoBehaviour
{
	[Header("Audio")]
	public AudioClip _win;
	public AudioClip _fail;


	[Header("Components")]
	private AudioSource _audiosource;

	void Awake()
	{
		Initialize();
	}

	void Initialize()
	{
		_audiosource = GetComponent<AudioSource>();
	}

	public void Play(AudioClip clip)
	{
		if (!_audiosource.isPlaying)
		{
			_audiosource.clip = clip;
			_audiosource.Play();
		}
			
	}
}
