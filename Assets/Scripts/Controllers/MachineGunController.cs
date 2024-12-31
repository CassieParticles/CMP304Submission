using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MachineGunController :  Controller
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

        //Movement decision tree
        actor.setMoveDirection(MoveEvents.StopMoving, Vector2.zero);

        float distanceToTarget = (actor.gameObject.transform.position - target.gameObject.transform.position).magnitude;
        if(actor.getTouchingActorCount() > 0 )
        {
            Vector2 direction = actor.getDirAwayFromTouchingActors();
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else if(distanceToTarget > 7)
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


                    if(GetActorInWay(actor, target.transform.position).Count > 0)
                    {
                        //Can't see target, move perpendicular
                        Vector2 directionToTarget = (target.transform.position - actor.transform.position).normalized;
                        Vector2 direction = new Vector2(-directionToTarget.y, directionToTarget.x);
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
            actor.setAimDirection(ShootEvents.DequipWeapon, defaultController);
        }
        else
        {
            //Calculate where the actor should aim
            Vector2 aimPos = target.transform.position;
            Vector2 targetMoveDirection = target.getMoveDirection();
            float shotLead = 2.0f;
            aimPos += targetMoveDirection * shotLead;

            //Check if shooting hits non-target
            if (GetActorInWay(actor, aimPos).Count > 0)
            {
                //Don't shoot
            }
            else
            {
                actor.setAimDirection(ShootEvents.FireAt, aimPos);
            }
        }
    }

}
