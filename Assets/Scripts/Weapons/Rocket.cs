using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField] private float explosionRadius = 4;
    [SerializeField] private float maxExplosionDamage = 120;
    [SerializeField] private float explosionTimer = 0.5f;

    private BoxCollider2D rocketCollider;
    private CircleCollider2D explosionCollider;
    private SpriteRenderer rocketSprite;
    private SpriteRenderer explosionSprite;

    private bool exploded = false;  //Set to true once rocket collides (and explodes)

    public override float calcDamage(Actor actor)
    {
        float distance = Mathf.Max((transform.position - actor.transform.position).magnitude / explosionRadius,0);

        return maxExplosionDamage * (1 - distance);
    }

    private void Explode()
    {   
        //Set the rocket to having exploded
        exploded = true;

        //Stop velocity
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        //Disable rocket components
        rocketCollider.enabled = false;
        rocketSprite.enabled = false;

        //Enable explosion components
        explosionCollider.enabled = true;
        explosionSprite.enabled = true;

        StartCoroutine(ClearExplosion());
    }

    private IEnumerator ClearExplosion()
    {
        yield return new WaitForSeconds(explosionTimer);

        Destroy(gameObject);
    }

    public override void Awake()
    {
        base.Awake();

        //Get components
        rocketCollider = GetComponent<BoxCollider2D>();
        explosionCollider = GetComponent<CircleCollider2D>();
        rocketSprite = GetComponent<SpriteRenderer>();
        explosionSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        //Set up components
        explosionSprite.gameObject.transform.localScale = new Vector3(explosionRadius * 2, explosionRadius * 2, 1);
        explosionCollider.radius = explosionRadius;
        explosionCollider.enabled = false;
        explosionSprite.enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Actor actor = collision.gameObject.GetComponent<Actor>();
        if(actor)
        {
            if (!exploded)
            {
                Explode();
            }
            else
            {
                actor.TakeDamage(calcDamage(actor));
            }
        }
    }
}
