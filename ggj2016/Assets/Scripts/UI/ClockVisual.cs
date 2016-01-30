using UnityEngine;
using System.Collections;

public class ClockVisual : MonoBehaviour
{
	[Header("Properties")]
	public bool _countDown;

	[Space(15)]
	public float _swingTime = 2f;
	public float _swingRange = 10;

	private float _timer = 0;
	private float _swingDirection = 1;

	[Header("Components")]
	public Transform _pointer;
	public Transform _hanger;

	private Clock _clock;

	// Use this for initialization
	void Start ()
	{
		Initialize();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_countDown)
			CountDown();
		else
			CountUp();

		SwingHanger();

		Debug.Log(_clock.TimeLeftInSeconds());
	}

	void Initialize()
	{
		_clock = Object.FindObjectOfType<Clock>();
	}

	void CountUp()
	{
		float newRotation = (_clock.TimePerDay - _clock.TimeLeftInSeconds()) * 6;
		_pointer.localEulerAngles = new Vector3(_pointer.localEulerAngles.x, _pointer.localEulerAngles.y, newRotation);
	}

	void CountDown()
	{
		float newRotation = _clock.TimeLeftInSeconds() * 6;
		_pointer.localEulerAngles = new Vector3(_pointer.localEulerAngles.x, _pointer.localEulerAngles.y, newRotation);
	}

	void SwingHanger()
	{
		if(_timer < _swingTime)
		{
			float hangerRotation = Mathf.Lerp(_swingDirection * -_swingRange, _swingDirection * _swingRange, _timer / _swingTime);
			_hanger.localEulerAngles = new Vector3(_hanger.localEulerAngles.x, _hanger.localEulerAngles.y, hangerRotation);

			_timer += Time.deltaTime;
		}
		else
		{
			_swingDirection *= -1;
			_timer = 0;
		}
	}
}
