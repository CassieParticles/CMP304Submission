using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    MeleeAttack,
}

public class Actor : MonoBehaviour
{
    [SerializeField] float moveSpeed=5;

    private Controller controller;

    private Rigidbody2D rb;

    private Gun gunHeld;
    private Gun ignoreGun;   //Once dropped, ignore till player steps off the gun
    private Actor target;

    private float health;

    //Melee attack
    private bool charging;
    //Direction actor was moving in last frame, used to predict movement direction
    private Vector2 LFMoveDirection;

    private List<Actor> touchingActors;

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
                DequipCurrentWeapon((Controller)aimDirection);
                break;
            case ShootEvents.MeleeAttack:
                StartCoroutine(MeleeAttack());
                break;
        }

    }

    public Vector2 getMoveDirection()
    {
        return LFMoveDirection;
    }

    public void SetController(Controller controller)
    {

        this.controller= controller;
    }

    public Actor GetCurrentTarget()
    {
        return target;
    }

    public void AddNewTarget(Actor actor)
    {
        target = actor;
    }

    private void EquipWeapon(Gun gun)
    {
        //Set the gun held to the gun
        gunHeld = gun;
        gun.pickedUp(this);

        //Set the weapon to be child of the actor
        GameObject gunGameObject = gun.gameObject;
        gunGameObject.transform.parent = this.transform;
        gunGameObject.transform.localPosition = Vector3.zero;

        //Do not overwrite player controller
        if (controller is not PlayerController)
        {
            SetController(gun.getController());
        }
    }

    private void DequipCurrentWeapon(Controller unarmedController)
    {
        if (gunHeld == null) { return; }
        gunHeld.gameObject.transform.parent = null;
        gunHeld.dropped();

        ignoreGun = gunHeld;

        gunHeld = null;

        //Do not overwrite player controller
        if (controller is not PlayerController)
        {
            SetController(unarmedController);
        }
    }

    public Gun getCurrentWeapon()
    {
        return gunHeld;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    private IEnumerator MeleeAttack()
    {
        charging = true;

        yield return new WaitForSeconds(0.5f);

        charging = false;
    }

    public int getTouchingActorCount()
    {
        return touchingActors.Count;
    }
    public Vector2 getDirAwayFromTouchingActors()
    {
        Vector2 avgDir = Vector2.zero;
        if(touchingActors.Count == 0)
        {
            return avgDir;
        }
        for (int i = 0; i < touchingActors.Count; i++)
        {
            if (touchingActors==null)
            {
                touchingActors.Remove(touchingActors[i]);
                i--;
                continue;
            }
            Vector2 dir = transform.position - touchingActors[i].transform.position;
            avgDir += dir;
        }

        return avgDir.normalized;
    }

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
      

        gunHeld = null;

        health = 100;

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            AddNewTarget(player.GetComponent<Actor>());

        }


        touchingActors = new List<Actor>();
    }

    private void FixedUpdate()
    {
        //Ask controller to carry out functions
        if (controller == null) { return; }
        controller.DoActions(this);
        LFMoveDirection = rb.velocity;

        if(health <= 0)
        {
            DequipCurrentWeapon((Controller)null);
            Destroy(gameObject);    //If other systems need to know this, change
        }
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
            touchingActors.Add(go.GetComponent<Actor>());
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Actor>())
        {
            touchingActors.Remove(collision.gameObject.GetComponent<Actor>());
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
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
                if(gun.getOwner() != null) { return; }  //Don't pick upa  gun that is held
                EquipWeapon(gun);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Gun>() == ignoreGun) { ignoreGun = null; }
    }
}
