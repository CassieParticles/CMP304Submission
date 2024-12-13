using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Gun   //Gun inherits from monobehavior
{
    public override GameObject ShootAt(Vector2 direction)
    {
        Debug.Log("Machine gun");
        return null;
    }
}
