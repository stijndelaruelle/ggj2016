using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="GGJ16/Audio")]
public class soAudio : ScriptableObject
{
	[Header("Properties")]
	public AudioClip _audioClip;

	[Space(15)]
	public bool _loop;
}
