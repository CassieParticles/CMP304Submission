using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolController : Controller
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
        if(distanceToTarget < 3)
        {
            //Move away from player
            Vector2 direction = (actor.transform.position - target.transform.position).normalized;
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else
        {
            //Find the nearest weapon that is better
            GameObject closestBetterWeapon = GetClosestWeapon(weaponList, actor.gameObject, 1000, actor.getCurrentWeapon().getGunValue());
            if (closestBetterWeapon != null)
            {
                //Move to nearest weapon
                Vector2 direction = (closestBetterWeapon.transform.position - actor.gameObject.transform.position).normalized;
                actor.setMoveDirection(MoveEvents.Move, direction);
            }
            else
            {
                //If there is other enemies, move towards them
                Vector3 averagePosition = getAverageLocation(actorList);
                if (averagePosition != null)
                {
                    Vector2 direction = averagePosition - actor.transform.position;
                    actor.setMoveDirection(MoveEvents.Move, direction);
                }
                else
                {
                    //Do nothing
                }

            }
        }

        

        //Weapon is out of ammo
        if (actor.getCurrentWeapon().getBulletCount() == 0)
        {
            //Drop weapon (change to unarmed AI)
            actor.setAimDirection(ShootEvents.DequipWeapon, Vector2.zero);
        }
        else
        {
            Vector2 aimDirection = (target.transform.position - actor.transform.position).normalized;
            actor.setAimDirection(ShootEvents.FireAt,aimDirection);
        }

    }
}
