using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour
{
	[Header("Properties")]
	public float _speedModifier = 1;

	[Header("Components")]
	private Animator _animator;
	private Player _player;
	private Transform _spriteTransform;

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
		_player = GetComponent<Player>();
		_spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;

		Play(AnimationType.Idle);
	}

	void FixedUpdate()
	{
		// Check if we need to update the animation
		if(_currentAnimation == AnimationType.Idle || _currentAnimation == AnimationType.Move)
		{
			if (_player.Velocity.magnitude > .1f)
			{
				Play(AnimationType.Move);
				SetSpeed(_player.Velocity.magnitude * _speedModifier);

				// To flip or not to flip
				if (_player.Velocity.x < 0)
					_spriteTransform.localScale = new Vector3(-1, 1, 1);
				else if(_player.Velocity.x > 0)
					_spriteTransform.localScale = Vector3.one;
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
