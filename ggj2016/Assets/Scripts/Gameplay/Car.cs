using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Sjabloon;

public class Car : Vehicle
{
    [SerializeField]
    private List<SimpleAnimation> m_SimpleAnimations;

    protected override bool CanDrive()
    {
        //Are all the children in a vehicle? (bus or car)
        if (!GameManager.Instance.AllChildrenInVehicle())
            return false;

        return base.CanDrive();
    }

    public override void Interact(Player player)
    {
        base.Interact(player);

        bool activateMotor = (m_Passengers.Count > 0);

        foreach(SimpleAnimation simpleAnimation in m_SimpleAnimations)
        {
            simpleAnimation.Play(activateMotor);
        }
    }
}
