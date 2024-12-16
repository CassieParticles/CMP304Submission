using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonoBehaviour
{
    public virtual void DoActions(Actor actor)
    {
        //Null, should be overwritten
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
}
