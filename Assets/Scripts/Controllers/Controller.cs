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
        for (int i = 0; i < gameObjects.Count; ++i)
        {
            averageLocation+= gameObjects[i].transform.position;
        }
        averageLocation /= gameObjects.Count;
        return averageLocation;
    }
}
