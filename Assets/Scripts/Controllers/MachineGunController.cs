using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunController :  Controller
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
        if(distanceToTarget > 7)
        {
            //Move towards target
            Vector2 direction = (target.transform.position - actor.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);

        }
        else if(distanceToTarget < 3)
        {
            //Move away from target
            Vector2 direction = (actor.transform.position - target.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else
        {
            if(actorList.Count > 2)
            {
                //Get average position of all targets
                Vector3 averagePosition = getAverageLocation(actorList);
                Vector3 dAvg = averagePosition - actor.transform.position;
                Vector2 dAvg2 = dAvg;
                if(dAvg.magnitude > 5)
                {
                    //Move towards centre
                    actor.setMoveDirection(MoveEvents.Move, dAvg2.normalized);
                }
                else
                {
                    if(GetActorInWay(actor).Count > 0)
                    {
                        //Can't see target, move perpendicular
                        Vector2 directionToTarget = (target.transform.position - actor.transform.position).normalized;
                        Vector2 direction = new Vector2(-directionToTarget.y, directionToTarget.x);
                        direction += -directionToTarget * 0.05f;    //
                        actor.setMoveDirection(MoveEvents.Move, directionToTarget.normalized);
                    }
                }
            }
            else
            {
                //Do nothing
            }
        }

        //Shooting decision tree

        //Weapon is out of ammo
        if (actor.getCurrentWeapon().getBulletCount() == 0)
        {
            //Drop weapon (change to unarmed AI)
            actor.setAimDirection(ShootEvents.DequipWeapon, Vector2.zero);
        }
        else
        {
            //Check if shooting hits non-target
            if (GetActorInWay(actor).Count > 0)
            {
                //Don't shoot
            }
            else
            {
                Vector2 pos = target.transform.position;    //Explicitly(ish) cast position to a Vector2
                actor.setAimDirection(ShootEvents.FireAt, pos);
            }
        }
    }

}
