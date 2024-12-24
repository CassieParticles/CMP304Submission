using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public virtual float calcDamage(Actor actor) { return 0; }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            actor.TakeDamage(calcDamage(actor));
            Destroy(gameObject);
        }
    }
}
