using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;

    private Vector2 moveDirection;
    private Vector2 aimDirection;

    private Rigidbody2D rb;

    private Observer movementObserver;
    private Observer shootingObserver;

    private Gun gunHeld;

    private void setMoveDirection(object moveDirection)
    {
        if((Vector2)moveDirection == Vector2.zero)
        {
            rb.velocity = Vector2.zero;
            return;
        }
    
        rb.velocity += (Vector2)moveDirection * moveSpeed;
    }

    private void setAimDirection(object aimDirection)
    {
         this.aimDirection += (Vector2)aimDirection;
        DequipCurrentWeapon();
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
    }

    private void DequipCurrentWeapon()
    {
        if (gunHeld == null) { return; }
        gunHeld.gameObject.transform.parent = null;
        gunHeld = null;
    }

    private void Start()
    {
        moveDirection = new Vector2();
        aimDirection= new Vector2();

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
        if(go.GetComponent<Gun>() !=null)
        {
            EquipWeapon(go.GetComponent<Gun>());
        }
    }

}
