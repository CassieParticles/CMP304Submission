using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Node;

public class UnarmedController : Controller
{
     //Set up, only time Awake is above DoActions
    public override void DoActions(Actor actor)
    {

        List<GameObject> weaponList = new List<GameObject>();
        List<GameObject> actorList = new List<GameObject>();

        GetScene(actorList,weaponList);
        Actor target = actor.GetCurrentTarget();

        //Remove the target and actor from the actor list
        actorList.Remove(target.gameObject);
        actorList.Remove(actor.gameObject);

        //Movement decision tree
        //Get the distance from this game object to the target, path 0 if distance is < 5, path 1 otherwise
        actor.setMoveDirection(MoveEvents.StopMoving, Vector2.zero);
        float distance = (actor.gameObject.transform.position - target.gameObject.transform.position).magnitude;
        if (actor.getTouchingActorCount() > 0)
        {
            Vector2 direction = actor.getDirAwayFromTouchingActors();
            actor.setMoveDirection(MoveEvents.Move, direction);
        }
        else if (distance < 5)
        {
            //Get if there is a weapon within 3 units
            GameObject closestWeapon = GetClosestWithin(weaponList, actor.gameObject, 3);
            if (closestWeapon)
            {
                //Move towards weapon
                Vector2 direction = (closestWeapon.transform.position - actor.gameObject.transform.position).normalized;
                actor.setMoveDirection(MoveEvents.Move, direction);
            }
            else
            {
                //Move away from target
                Vector2 direction = (actor.gameObject.transform.position - target.gameObject.transform.position ).normalized;
                actor.setMoveDirection(MoveEvents.Move, direction);
            }
        }
        else
        {
            GameObject closestWeapon = GetClosestWithin(weaponList, actor.gameObject);
            if (closestWeapon)
            {
                //Move towards closest weapon
                //Move towards weapon
                Vector2 direction = (closestWeapon.transform.position - actor.gameObject.transform.position).normalized;
                actor.setMoveDirection(MoveEvents.Move, direction);
            }
            else
            {
                //idk, there are no weapons on the map
            }
        }


    }
}
