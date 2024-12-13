using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MachineGun : Gun   //Gun inherits from monobehavior
{
    public override GameObject ShootAt(Vector2 target)
    {
        if(canShoot)
        {
            StartCoroutine(Shoot(target));
        }

        return null;
    }

    private IEnumerator Shoot(Vector2 target)
    {
        //Shoot gun
        GameObject newBullet = Instantiate(bulletShot);

        Vector2 direction = (target - new Vector2(transform.position.x, transform.position.y)).normalized;

        newBullet.GetComponent<Rigidbody2D>().velocity = direction;
        newBullet.transform.position = transform.position;
        newBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x));

        canShoot = false;

        yield return new WaitForSeconds(0.1f);

        canShoot = true;
    }

    private bool canShoot=true;

    public override int getGunValue()
    {
        return 30;
    }

    private void Start()
    {
        canShoot = true;
    }
}
