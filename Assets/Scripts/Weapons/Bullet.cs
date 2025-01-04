using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public virtual float calcDamage(Actor actor) { return 0; }

    private string shotBy;

    private IEnumerator CleanupBullet()
    {
        yield return new WaitForSeconds(3);

        if (shotBy != "Player")
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
    { this.shotBy = gun.getOwner().gameObject.name; }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            if(shotBy !="Player")
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
