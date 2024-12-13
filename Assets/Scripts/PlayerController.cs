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

        sceneCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        movementObserver.Notify(MoveEvents.StopMoving,Vector2.zero);
        if (Input.GetKey(KeyCode.W))
        {
            movementObserver.Notify(MoveEvents.Move,new Vector2(0, 1));
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementObserver.Notify(MoveEvents.Move,new Vector2(-1, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementObserver.Notify(MoveEvents.Move,new Vector2(0, -1));
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementObserver.Notify(MoveEvents.Move,new Vector2(1, 0));
        }
        if(Input.GetMouseButton(0))
        {
            Vector2 aimPos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            shootingObserver.Notify(ShootEvents.FireAt, aimPos);
        }
        if (Input.GetMouseButton(1))
        { 
            shootingObserver.Notify(ShootEvents.DequipWeapon,new Vector2(0, 0));
        }
    }
}
