using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Gun
{
    [SerializeField] private float rocketSpeed = 5;
    [SerializeField] private int rocketCount = 1;

    protected override IEnumerator Shoot(Vector2 target)
    {
        //Shoot gun
        GameObject newBullet = Instantiate(bulletShot);

        Vector2 direction = (target - new Vector2(transform.position.x, transform.position.y)).normalized;

        newBullet.GetComponent<Rigidbody2D>().velocity = direction * rocketSpeed;
        newBullet.transform.position = transform.position + new Vector3(direction.x, direction.y, 0);
        newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x));

        canShoot = false;

        bulletCount--;

        yield return null;  //Only 1 rocket, so no need to add checks and timers
    }

    public override int getGunValue()
    {
        return 80;
    }

    private void Start()
    {
        canShoot = true;
        bulletCount = rocketCount;
    }
}
