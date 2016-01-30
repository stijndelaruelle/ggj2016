using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
	[Header("Properties")]
	public soAudio _properties;

	[Header("Components")]
	private AudioSource _source;

	void Awake()
	{
		Initialize();

		if (_properties._playOnAwake)
			Play();
	}

	void Initialize()
	{
		_source = GetComponent<AudioSource>();

		// Set the properties of the audiosource
		_source.clip = _properties._audioClip;
		_source.loop = _properties._loop;
	}

	public void Play()
	{
		if (_source == null)
			Initialize();

		if (!_source.isPlaying)
			_source.Play();
	}
}
