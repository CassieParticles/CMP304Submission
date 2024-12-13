using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : Bullet
{
    public override float calcDamage(Actor actor) 
    {
        return 10;
    }
}
