using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    protected float fireRate;
    protected float bulletSpread;
    protected int bulletCount;

    protected Actor owner;

    [SerializeField]protected GameObject bulletShot;
    
    //Return GameObject if new bullet is fired, return null otherwise
    public virtual GameObject ShootAt(Vector2 direction)
    {
        return null;
    }

    public void pickedUp(Actor owner) { this.owner = owner; }
    public void dropped() { this.owner= null; }
    public Actor getOwner() {  return this.owner; }

    public int getBulletCount() { return bulletCount; }

    public virtual int getGunValue() { return 0; }
}
