using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private float bulletSpeed = 15;
    [SerializeField] private int startingBulletCount = 24;

    public int GetGunValue()
    {
        return 10;
    }

    protected override IEnumerator Shoot(Vector2 target)
    {
        //Shoot gun
        GameObject newBullet = Instantiate(bulletShot);

        Vector2 direction = (target - new Vector2(transform.position.x, transform.position.y)).normalized;

        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        newBullet.transform.position = transform.position + new Vector3(direction.x, direction.y, 0);
        newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x));

        canShoot = false;

        bulletCount--;

        yield return new WaitForSeconds(0.5f);

        canShoot = bulletCount > 0;
    }

    private void Start()
    {
        canShoot = true;
        bulletCount = startingBulletCount;
    }
}
