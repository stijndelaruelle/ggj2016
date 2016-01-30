using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
	[Header("Properties")]
	public float _speedModifier = 1;

	[Header("Components")]
	public Animator _animator;
	public Rigidbody2D _rigidBody;

	[Header("Animations")]
	public string _parameter;
	public AnimationType _currentAnimation = AnimationType.Move;

	public enum AnimationType
	{
		Idle = 0,
		Move = 1,
		GeneralTask = 2
	}

	public void Initialize()
	{
		_animator = GetComponentInChildren<Animator>();
		_rigidBody = GetComponent<Rigidbody2D>();

		Play(AnimationType.Idle);
	}

	void FixedUpdate()
	{
		// Check if we need to update the animation
		if(_currentAnimation == AnimationType.Idle || _currentAnimation == AnimationType.Move)
		{
			if (_rigidBody.velocity.magnitude > 0)
			{
				Play(AnimationType.Move);
				SetSpeed(_rigidBody.velocity.magnitude * _speedModifier);
			}
			else
				Play(AnimationType.Idle);
		}
	}

	public void Play(AnimationType animationType)
	{
		if(animationType != _currentAnimation)
		{
			_currentAnimation = animationType;
			_animator.SetInteger(_parameter, (int)_currentAnimation);

			SetSpeed(1);
		}
	}

	public void SetSpeed(float animationSpeed)
	{
		_animator.speed = animationSpeed;
	}
}
