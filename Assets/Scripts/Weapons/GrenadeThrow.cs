using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class GrenadeThrow : Gun
{
    [SerializeField] private float bulletSpeed = 2;
    [SerializeField] private int startingBulletCount = 3;

    protected override IEnumerator Shoot(Vector2 target)
    {
        //Create grenade
        GameObject gren = Instantiate(bulletShot);

        Vector2 direction = (target - new Vector2(transform.position.x, transform.position.y)).normalized;
        gren.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed; 

        gren.transform.position = transform.position;
        gren.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x));
        gren.GetComponent<Bullet>().SetGun(this);

        canShoot = false;
        bulletCount--;

        if (owner.gameObject.name != "Player")
        {
            AimTracker.RegisterFire(Weapon.GrenadeLauncher);
        }

        yield return new WaitForSeconds(2.0f);

        canShoot = bulletCount > 0;
    }

    public override int getGunValue()
    {
        return 50;
    }

    void Start()
    {
        canShoot = true;
        bulletCount = startingBulletCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
