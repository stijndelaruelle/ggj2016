using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "GGJ16/Simple Animation")]
public class soSimpleAnimation : ScriptableObject
{
	[Header("Wiggle")]
	public float _wiggleFrequency;
	public Vector3 _wiggleOffset;
	public Vector3 _wiggleRotation;

	[Header("Rotation")]
	public Vector3 _rotationSpeed;
}
