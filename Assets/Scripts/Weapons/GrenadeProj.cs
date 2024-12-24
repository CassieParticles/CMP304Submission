using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeProj : Bullet
{
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private float maxExplosionDamage = 80;
    [SerializeField] private float fuseTimer = 3.0f;
    [SerializeField] private float explosionTimer=0.5f;

    private CircleCollider2D explosionCollider;
    private SpriteRenderer grenadeSprite;
    private SpriteRenderer explosionSprite;

    public override float calcDamage(Actor actor)
    {
        float distance = (transform.position - actor.transform.position).magnitude;

        return maxExplosionDamage * distance;
    }

    private IEnumerator FuseFunction()
    {
        //Wait down the fuse time 
        yield return new WaitForSeconds(fuseTimer);

        Explode();
    }

    private void Explode()
    {
        //Remove velocity
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        //Change sprite
        grenadeSprite.enabled = false;
        explosionSprite.enabled = true;

        //Enable collider
        explosionCollider.enabled = true;

        StartCoroutine(ClearExplosion());
    }

    private IEnumerator ClearExplosion()
    {
        yield return new WaitForSeconds(explosionTimer);

        Destroy(gameObject);
    }


    private void Awake()
    {
        explosionCollider = GetComponent<CircleCollider2D>();
        grenadeSprite = GetComponent<SpriteRenderer>();
        explosionSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        //Set up size of grenade properly
        explosionSprite.gameObject.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, 1);
        explosionCollider.radius = explosionRadius;

        StartCoroutine(FuseFunction());
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if (actor != null)
        {
            actor.TakeDamage(calcDamage(actor));
        }
    }
}
