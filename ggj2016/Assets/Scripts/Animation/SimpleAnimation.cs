﻿using UnityEngine;
using System.Collections;

public class SimpleAnimation : MonoBehaviour
{
	[Header("Properties")]
	public soSimpleAnimation _properties;

	[Space(15)]
	private float _wiggleTimer;
	private Vector3 _currentEulerAngles = Vector3.zero;

	[Header("Components")]
	public Transform _targetTransform;
	
	// Update is called once per frame
	void Update ()
	{
		Wiggle();
		Rotate();
	}

	void Wiggle()
	{
		if(_wiggleTimer < _properties._wiggleFrequency)
		{
			_wiggleTimer += Time.deltaTime;
		}
		else
		{
			// Random offset
			float posX = Random.Range(-_properties._wiggleOffset.x, _properties._wiggleOffset.x);
			float posY = Random.Range(-_properties._wiggleOffset.y, _properties._wiggleOffset.y);
			float posZ = Random.Range(-_properties._wiggleOffset.z, _properties._wiggleOffset.z);

			_targetTransform.localPosition = new Vector3(posX, posY, posZ);

			// Random rotation
			float rotX = Random.Range(-_properties._wiggleRotation.x, _properties._wiggleRotation.x);
			float rotY = Random.Range(-_properties._wiggleRotation.y, _properties._wiggleRotation.y);
			float rotZ = Random.Range(-_properties._wiggleRotation.z, _properties._wiggleRotation.z);

			_targetTransform.localEulerAngles = new Vector3(rotX, rotY, rotZ);

			// Reset timer
			_wiggleTimer = 0;
		}
	}

	void Rotate()
	{
		float rotSpeedX = _properties._rotationSpeed.x * Time.deltaTime;
		float rotSpeedY = _properties._rotationSpeed.y * Time.deltaTime;
		float rotSpeedZ = _properties._rotationSpeed.z * Time.deltaTime;

		_targetTransform.localEulerAngles += new Vector3(rotSpeedX, rotSpeedY, rotSpeedZ);
	}
}
