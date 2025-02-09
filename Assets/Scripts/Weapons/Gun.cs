using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gun : MonoBehaviour
{
    protected float fireRate;
    protected float bulletSpread;
    protected int bulletCount;
    protected bool canShoot = true;

    Controller relevantController;

    protected Actor owner;

    [SerializeField]protected GameObject bulletShot;



    //Return GameObject if new bullet is fired, return null otherwise
    public virtual GameObject ShootAt(Vector2 target)
    {
        if (canShoot)
        {
            StartCoroutine(Shoot(target));
        }

        return null;
    }

    protected virtual IEnumerator Shoot(Vector2 target)
    {
        yield return null;
    }
        
    //Called on creation, used so actors picking them up can take the AI from them
    public void setController(Controller controller)
    {
        this.relevantController = controller;
    }

    public Controller getController()
    {
        return relevantController;
    }

    public void pickedUp(Actor owner) { this.owner = owner; }
    public void dropped() 
    {
        this.owner= null; 
        if(bulletCount == 0)
        {
            Destroy(this.gameObject);
        }
    }
    public Actor getOwner() {  return this.owner; }

    public int getBulletCount() { return bulletCount; }

    public virtual int getGunValue() { return 0; }
}
