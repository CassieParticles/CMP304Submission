using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerController : Controller
{
    [SerializeField] private GameObject FellaPrefab;

    private Camera sceneCamera;

    private Actor player;

    public Actor getPlayer()
    {
        return player;
    }
    public override void DoActions(Actor actor)
    {
        //Handle actor movement
        actor.setMoveDirection(MoveEvents.StopMoving, Vector2.zero);
        if(Input.GetKey(KeyCode.W))
        {
            actor.setMoveDirection(MoveEvents.Move, Vector2.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            actor.setMoveDirection(MoveEvents.Move, Vector2.down);
        }
        if (Input.GetKey(KeyCode.A))
        {
            actor.setMoveDirection(MoveEvents.Move, Vector2.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            actor.setMoveDirection(MoveEvents.Move, Vector2.right);
        }

        if(Input.GetMouseButton(0))
        {
            Vector2 aimPos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            actor.setAimDirection(ShootEvents.FireAt, aimPos);
        }
        if (Input.GetMouseButton(1))
        {
            actor.setAimDirection(ShootEvents.DequipWeapon, Vector2.up);
        }
        if (Input.GetKey(KeyCode.E))
        {
            actor.setAimDirection(ShootEvents.MeleeAttack, Vector2.zero);
        }
    }

    private void Awake()
    {
        sceneCamera = Camera.main;

        GameObject playerActorGO = Instantiate(FellaPrefab);
        playerActorGO.name = "Player";
        player = playerActorGO.GetComponent<Actor>();

        GameObject mainCamera = Camera.main.gameObject;
        mainCamera.transform.parent = playerActorGO.transform;

        player.SetController(this);
    }
}
