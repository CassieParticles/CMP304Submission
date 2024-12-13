using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //temp stuff, will be written over later
    [SerializeField] GameObject FellaPrefab;

    private Observer movementObserver;
    private Observer shootingObserver;

    private void Start()
    {
        //Set up observers for the player
        movementObserver = new Observer();
        shootingObserver = new Observer();

        //Set up the actor for the player to control
        GameObject instance = Instantiate(FellaPrefab);

        Actor a = instance.GetComponent<Actor>();
        a.ChangeMovementObserver(movementObserver);
        a.ChangeShootingObserver(shootingObserver);
    }


    private void FixedUpdate()
    {
        movementObserver.Notify(Vector2.zero);
        if (Input.GetKey(KeyCode.W))
        {
            movementObserver.Notify(new Vector2(0, 1));
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementObserver.Notify(new Vector2(-1, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementObserver.Notify(new Vector2(0, -1));
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementObserver.Notify(new Vector2(1, 0));
        }
    }
}
