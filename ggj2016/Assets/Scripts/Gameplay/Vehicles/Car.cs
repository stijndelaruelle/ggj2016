using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Sjabloon;

public class Car : Vehicle
{
    [SerializeField]
    private List<SimpleAnimation> m_SimpleAnimations;

	[Header("Audio Clips")]
	public soAudio _audioEngineRunning;
	public soAudio _audioHorn;

	[Header("Components")]
	private AudioController _audio;

	protected override bool CanDrive()
    {
        //Are all the children in a vehicle? (bus or car)
        if (!GameManager.Instance.AllChildrenInVehicle())
            return false;

		// Play the car horn
		if(base.CanDrive())
			_audio.Play(_audioHorn);

		return base.CanDrive();
    }

    public override void Interact(Player player)
    {
        base.Interact(player);

        bool activateMotor = (m_Passengers.Count > 0);

		// Play audio
		if(_audio == null)
			_audio = GetComponentInChildren<AudioController>();

		_audio.Play(_audioEngineRunning);

		foreach (SimpleAnimation simpleAnimation in m_SimpleAnimations)
        {
            simpleAnimation.Play(activateMotor);
        }
    }

    protected override void OnStartDay()
    {
        base.OnStartDay();

        foreach (SimpleAnimation simpleAnimation in m_SimpleAnimations)
        {
            simpleAnimation.Play(false);
        }
    }
}
