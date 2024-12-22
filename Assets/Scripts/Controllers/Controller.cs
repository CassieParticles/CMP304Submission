using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Controller
{
    public virtual void DoActions(Actor actor)
    {
        //Null, should be overwritten
    }

    protected void GetScene(List<GameObject> actors, List<GameObject> guns)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        SceneManager.GetActiveScene().GetRootGameObjects(gameObjects);

        //Get a list of the guns and actors in the scene
        for (int i = 0; i < gameObjects.Count; ++i)
        {
            if (gameObjects[i].GetComponent<Gun>())
            {
                guns.Add(gameObjects[i]);
            }
            if (gameObjects[i].GetComponent<Actor>())
            {
                actors.Add(gameObjects[i]);
            }
        }
    }

    protected GameObject GetClosestWithin(List<GameObject> gameObjects, GameObject subject, int maxDist = int.MaxValue)
    {
        GameObject closest = null;
        float closestDist = maxDist;

        for(int i=0;i<gameObjects.Count;++i)
        {
            float distance = (subject.transform.position - gameObjects[i].transform.position).magnitude;
            if (distance < closestDist)
            {
                closest = gameObjects[i]; 
                closestDist = distance;
            }
        }


        return closest;
    }

    protected GameObject GetClosestWeapon(List<GameObject> gameObjects, GameObject subject, int maxDist = int.MaxValue, int minValue = 0)
    {
        GameObject closestWeapon = null;
        float closestDist = maxDist;

        for (int i = 0; i < gameObjects.Count; ++i)
        {
            float distance = (subject.transform.position - gameObjects[i].transform.position).magnitude;
            if (distance < closestDist)
            {
                if (gameObjects[i].GetComponent<Gun>().getGunValue() >minValue)
                {
                    closestWeapon = gameObjects[i];
                    closestDist = distance;
                }
            }
        }

        return closestWeapon;
    }

    protected Vector3 getAverageLocation(List<GameObject> gameObjects)
    {
        Vector3 averageLocation = Vector3.zero;
        if (gameObjects.Count == 0)
        {
            return averageLocation;
        }
        for (int i = 0; i < gameObjects.Count; ++i)
        {
            averageLocation+= gameObjects[i].transform.position;
        }
        averageLocation /= gameObjects.Count;
        return averageLocation;
    }

    protected List<RaycastHit2D> GetActorInWay(Actor actor)
    {
        Actor target = actor.GetCurrentTarget();
        //Shooting decision tree
        Vector3 rayLine = target.transform.position - actor.transform.position;
        List<RaycastHit2D> colliderArray = new List<RaycastHit2D>();
        ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();

        //Get all enemies between target and actor
        //Get all colliders in raycast between player and target
        int collideCount = Physics2D.Raycast(actor.transform.position, rayLine, contactFilter, colliderArray);
        for (int i = 0; i < colliderArray.Count; ++i)
        {
            //Collider isn't an actor
            if (colliderArray[i].collider.gameObject.GetComponent<Actor>() == null)
            {
                colliderArray.RemoveAt(i);  //Remove item then restart loop without incrementing i
                i--;
                continue;
            }
            //Collider is target (will always be colliding)
            if (colliderArray[i].collider.gameObject == target.gameObject)
            {
                colliderArray.RemoveAt(i);  //Remove item then restart loop without incrementing i
                i--;
                continue;
            }
            //Collider is actor (will always be colliding)
            if (colliderArray[i].collider.gameObject == actor.gameObject)
            {
                colliderArray.RemoveAt(i);  //Remove item then restart loop without incrementing i
                i--;
                continue;
            }
        }

        return colliderArray;
    }
}
