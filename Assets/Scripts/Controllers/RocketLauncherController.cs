using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : Controller
{
    public override void DoActions(Actor actor)
    {
        List<GameObject> weaponList = new List<GameObject>();
        List<GameObject> actorList = new List<GameObject>();

        GetScene(actorList, weaponList);
        Actor target = actor.GetCurrentTarget();

        if (target == null)
        {
            return;
        }

        //Remove the target and actor from the actor list
        actorList.Remove(target.gameObject);
        actorList.Remove(actor.gameObject);

        actor.setMoveDirection(MoveEvents.StopMoving,Vector2.zero);

        float distanceToTarget = (actor.gameObject.transform.position - target.gameObject.transform.position).magnitude;
        if (distanceToTarget < 5)
        {
            //Move away from target
            Vector2 direction = (actor.transform.position - target.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else if (distanceToTarget > 6)
        {
            //Move towards target
            Vector2 direction = (target.transform.position - actor.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else 
        {
            if(GetActorInWay(actor, target.transform.position).Count > 0)
            {
                //Can't see target, move perpendicular
                Vector2 directionToTarget = (target.transform.position - actor.transform.position).normalized;
                Vector2 direction = new Vector2(-directionToTarget.y, directionToTarget.x);
                actor.setMoveDirection(MoveEvents.Move, directionToTarget.normalized);
            }
            //else, do nothing
        }

        //Shooting decision tree
        //Since rocket launcher only fires once, you can drop it immediately, and don't need to check ammo count
        if(distanceToTarget < 6)
        {
            if(!GetClosestWithin(actorList,target.gameObject,4))
            {
                //No actors within 4 units of target
                if(GetActorInWay(actor, target.transform.position).Count == 0)
                {
                    //Can clearly see target, shoot
                    Vector2 aim = target.transform.position;
                    actor.setAimDirection(ShootEvents.FireAt,aim);
                    actor.setAimDirection(ShootEvents.DequipWeapon, defaultController);
                }
                //Else, do nothing (cannot clearly see target)
            }
            //Else, Do nothing (other actors too close)
        }
        //Else, do nothing (Target too far away)

    }
}
