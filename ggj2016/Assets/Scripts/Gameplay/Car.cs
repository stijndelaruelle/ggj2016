using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Sjabloon;

public class Car : Vehicle
{
    protected override bool CanDrive()
    {
        //Are all the children in a vehicle? (bus or car)
        if (!GameManager.Instance.AllChildrenInVehicle())
            return false;

        return base.CanDrive();
    }
}
