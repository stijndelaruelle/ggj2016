using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
	[Header("Properties")]
	private soAudio _currentClip;

	[Header("Components")]
	private AudioSource _source;

	void Awake()
	{
		Initialize();
	}

	void Initialize()
	{
		_source = GetComponent<AudioSource>();
	}

	public void Play(soAudio audioToPlay)
	{
		if (_source == null)
			Initialize();

		if (_source.isPlaying && _currentClip != audioToPlay)
		{
			_source.Stop();
		}

		UpdateProperties(audioToPlay);

		if (!_source.isPlaying)
			_source.Play();

		_currentClip = audioToPlay;
	}

	public void Stop()
	{
		_source.Stop();
	}

	// Set the properties of the audiosource
	void UpdateProperties(soAudio audio)
	{
		_source.clip = audio._audioClip;
		_source.loop = audio._loop;
	}
}
