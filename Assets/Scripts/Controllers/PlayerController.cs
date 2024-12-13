using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //temp stuff, will be written over later
    [SerializeField] GameObject FellaPrefab;

    private Observer movementObserver;
    private Observer shootingObserver;

    private Camera sceneCamera;

    private Actor playerActor;

    private void Start()
    {
        //Set up observers for the player
        movementObserver = new Observer();
        shootingObserver = new Observer();

        //Set up the actor for the player to control
        GameObject instance = Instantiate(FellaPrefab);

        playerActor = instance.GetComponent<Actor>();
        playerActor.ChangeMovementObserver(movementObserver);
        playerActor.ChangeShootingObserver(shootingObserver);

        sceneCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        movementObserver.Notify(playerActor, MoveEvents.StopMoving,Vector2.zero);
        if (Input.GetKey(KeyCode.W))
        {
            movementObserver.Notify(playerActor, MoveEvents.Move,new Vector2(0, 1));
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementObserver.Notify(playerActor, MoveEvents.Move,new Vector2(-1, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementObserver.Notify(playerActor, MoveEvents.Move,new Vector2(0, -1));
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementObserver.Notify(playerActor, MoveEvents.Move,new Vector2(1, 0));
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 aimPos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            shootingObserver.Notify(playerActor, ShootEvents.FireAt, aimPos);
        }
        if (Input.GetMouseButton(1))
        { 
            shootingObserver.Notify(ShootEvents.DequipWeapon,new Vector2(0, 0));
        }
    }
}
