using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : Controller
{
    public override void DoActions(Actor actor)
    {
        List<GameObject> weaponList = new List<GameObject>();
        List<GameObject> actorList = new List<GameObject>();

        GetScene(actorList, weaponList);
        Actor target = actor.GetCurrentTarget();

        //Remove the target and actor from the actor list
        actorList.Remove(target.gameObject);
        actorList.Remove(actor.gameObject);

        //Movement decision tree
        actor.setMoveDirection(MoveEvents.StopMoving, Vector2.zero);
        float distanceToTarget = (actor.gameObject.transform.position - target.gameObject.transform.position).magnitude;
        if (distanceToTarget < 4)
        {
            //Move away from target
            Vector2 direction = (actor.transform.position - target.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else if(distanceToTarget > 8)
        {
            //Move towards to target
            Vector2 direction = (target.transform.position - actor.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else
        {
            //Move perpendicular to target
            Vector2 direction = (target.transform.position - actor.transform.position).normalized;
            direction = new Vector2(-direction.y, direction.x);
            actor.setMoveDirection(MoveEvents.Move, direction);
        }

        //Shooting decision tree
        if(actor.getCurrentWeapon().getBulletCount() == 0)
        {
            actor.setAimDirection(ShootEvents.DequipWeapon, Vector2.zero);
        }
        else
        {
            Vector3 targetMoveDirection = target.getMoveDirection();
            Vector2 targetPos = targetMoveDirection + target.transform.position;    //Throw infront of target
            actor.setAimDirection(ShootEvents.FireAt, targetPos);
        }
    }
}