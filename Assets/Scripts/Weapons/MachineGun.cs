using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;
using static UnityEngine.GraphicsBuffer;

public class MachineGun : Gun   //Gun inherits from monobehavior
{
    [SerializeField] private float bulletSpeed = 15;
    [SerializeField] private int startingBulletCount = 50;


    protected override IEnumerator Shoot(Vector2 target)
    {
        //Shoot gun
        GameObject newBullet = Instantiate(bulletShot);

        Vector2 direction = (target - new Vector2(transform.position.x, transform.position.y)).normalized;

        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        newBullet.transform.position = transform.position + new Vector3(direction.x,direction.y,0);
        newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x));
        newBullet.GetComponent<Bullet>().SetGun(this);

        canShoot = false;

        bulletCount--;

        if (owner.gameObject.name != "Player")
        {
            AimTracker.RegisterFire(Weapon.MachineGun);
        }

        yield return new WaitForSeconds(0.1f);

        canShoot = bulletCount > 0;
    }


    public override int getGunValue()
    {
        return 30;
    }

    private void Start()
    {
        canShoot = true;
        bulletCount = startingBulletCount;
    }
}
