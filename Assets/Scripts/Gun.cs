using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected float fireRate;
    protected float bulletSpread;

    //Return GameObject if new bullet is fired, return null otherwise
    public virtual GameObject ShootAt(Vector2 direction)
    {
        return null;
    }
}
