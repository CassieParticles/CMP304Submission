using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public virtual float calcDamage(Actor actor) { return 0; }

    private Gun gun;

    private IEnumerator CleanupBullet()
    {
        yield return new WaitForSeconds(15);

        if(gun.getOwner().gameObject.name!="Player")
        {
            AimTracker.RegisterMiss(AimTracker.GetEnumFromBullet(this));
        }

        Destroy(gameObject);
    }

    public virtual void Awake()
    {
        StartCoroutine(CleanupBullet());
    }

    public void SetGun(Gun gun)
    { this.gun = gun; }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            if(gun.getOwner().gameObject.name!="Player")
            {
                if (actor.gameObject.name == "Player")
                {
                    AimTracker.RegisterHit(AimTracker.GetEnumFromBullet(this));
                }
                else
                {
                    AimTracker.RegisterFriendlyFire(AimTracker.GetEnumFromBullet(this));
                }
            }

            actor.TakeDamage(calcDamage(actor));
            Destroy(gameObject);
        }
    }
}
