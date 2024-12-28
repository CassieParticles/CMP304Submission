using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public virtual float calcDamage(Actor actor) { return 0; }

    private IEnumerator CleanupBullet()
    {
        yield return new WaitForSeconds(15);

        Destroy(gameObject);
    }

    public void Awake()
    {
        StartCoroutine(CleanupBullet());
    }

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
