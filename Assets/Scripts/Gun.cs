using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected float fireRate;
    protected float bulletSpread;

    public virtual GameObject ShootAt(Vector2 direction)
    {
        return null;
    }
}
