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
    DequipWeapon,
    MeleeAttack
}

public class Actor : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;

    private Controller controller;

    private Rigidbody2D rb;

    private Gun gunHeld;
    private Gun ignoreGun;   //Once dropped, ignore till player steps off the gun
    private Stack<Actor> targets;

    private float health;

    //Melee attack
    private bool charging;

    public void setMoveDirection(System.Enum eventType, object moveDirection)
    {
        switch((MoveEvents)eventType)
        {
            case MoveEvents.Move:
                rb.velocity += (Vector2)moveDirection * moveSpeed * (charging ? 2.0f : 1.0f);   //Double speed if charging
                break;

            case MoveEvents.StopMoving:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    public void setAimDirection(System.Enum eventType, object aimDirection)
    {
        switch ((ShootEvents)eventType)
        {
            case ShootEvents.FireAt:
                if(gunHeld!=null && !charging)
                {
                    gunHeld.ShootAt((Vector2)aimDirection);
                }
                break;
            case ShootEvents.DequipWeapon:
                DequipCurrentWeapon();
                break;
            case ShootEvents.MeleeAttack:
                StartCoroutine(MeleeAttack());
                break;
        }

    }

    public void SetController(Controller controller)
    {
        this.controller= controller;
    }

    public Actor GetCurrentTarget()
    {
        return targets.Peek();
    }

    public void AddNewTarget(Actor actor)
    {
        targets.Push(actor);
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

    private IEnumerator MeleeAttack()
    {
        charging = true;

        yield return new WaitForSeconds(0.5f);

        charging = false;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        gunHeld = null;

        Debug.Log("Created creature");
        Debug.Log(rb);

        health = 100;

        targets = new Stack<Actor>();
    }

    private void FixedUpdate()
    {
        //Ask controller to carry out functions
        if (controller == null) { return; }
        controller.DoActions(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<Actor>())
        {
            if (charging)
            {
                go.GetComponent<Actor>().health -= 20;  //Damage opponent via melee attack
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.GetComponent<Gun>())
        {
            //Exit early if gun is already held
            if (gunHeld != null) { return; }
            Gun gun = go.GetComponent<Gun>();
            if (gun != null)
            {
                if (gun == ignoreGun) { return; }   //Don't pick up the ignore gun
                EquipWeapon(gun);
            }
        }
        if(go.GetComponent<Bullet>())
        {
            //Hit by a bullet
            float damageTaken = collision.gameObject.GetComponent<Bullet>().calcDamage(this);
            health -= damageTaken;
            Destroy(go);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Gun>() == ignoreGun) { ignoreGun = null; }
    }
}
