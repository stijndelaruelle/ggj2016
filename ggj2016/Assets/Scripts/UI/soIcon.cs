using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "GGJ16/Icon")]
public class soIcon : ScriptableObject
{
	[Header("Win properties")]
	public float _scaleUp;
	public float _scaleDuration;

	[Header("Fail properties")]
	public int _flashes;
	public float _flashLength;
}
