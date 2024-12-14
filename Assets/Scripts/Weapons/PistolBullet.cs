using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : Bullet
{
    public override float calcDamage(Actor actor)
    {
        return 35;
    }
}
