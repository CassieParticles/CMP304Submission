using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

//Events for actors
public enum MoveEvents
{
    Move,
    StopMoving
}

public enum ShootEvents
{
    FireAt,
    DequipWeapon
}

public class Actor : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;

    private Rigidbody2D rb;

    private Observer movementObserver;
    private Observer shootingObserver;

    private Gun gunHeld;
    private Gun ignoreGun;   //Once dropped, ignore till player steps off the gun
    private Actor Target;


    private void setMoveDirection(System.Enum eventType, object moveDirection)
    {
        switch((MoveEvents)eventType)
        {
            case MoveEvents.Move:
                rb.velocity += (Vector2)moveDirection * moveSpeed;
                break;

            case MoveEvents.StopMoving:
                rb.velocity = Vector2.zero;
                break;
        }
        
    }

    private void setAimDirection(System.Enum eventType, object aimDirection)
    {
        switch ((ShootEvents)eventType)
        {
            case ShootEvents.FireAt:
                if(gunHeld!=null)
                {
                    gunHeld.ShootAt((Vector2)aimDirection);
                }
                break;
            case ShootEvents.DequipWeapon:
                DequipCurrentWeapon();
                break;
        }

    }

    public void ChangeMovementObserver(Observer newObserver)
    {
        if(movementObserver!= null) 
        {
            movementObserver.removeListener(setMoveDirection);
        }

        movementObserver = newObserver;
        newObserver.addListener(setMoveDirection);
    }

    public void ChangeShootingObserver(Observer newObserver)
    {
        if (shootingObserver != null)
        {
            shootingObserver.removeListener(setAimDirection);
        }

        shootingObserver = newObserver;
        newObserver.addListener(setAimDirection);
    }

    private void EquipWeapon(Gun gun)
    {
        //Set the gun held to the gun
        gunHeld = gun;

        //Set the weapon to be child of the actor
        GameObject gunGameObject = gun.gameObject;
        gunGameObject.transform.parent = this.transform;
        gunGameObject.transform.localPosition = Vector3.zero;
    }

    private void DequipCurrentWeapon()
    {
        if (gunHeld == null) { return; }
        gunHeld.gameObject.transform.parent = null;

        ignoreGun = gunHeld;

        gunHeld = null;
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        gunHeld = null;

        Debug.Log("Created creature");
        Debug.Log(rb);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Exit early if gun is already held
        if (gunHeld != null) { return; }
        GameObject go = collision.gameObject;
        Gun gun = go.GetComponent<Gun>();
        if(gun !=null)
        {
            if (gun == ignoreGun) { return; }   //Don't pick up the ignore gun
            EquipWeapon(gun);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Gun>() == ignoreGun) { ignoreGun = null; }
    }
}
