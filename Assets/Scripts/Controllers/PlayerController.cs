using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerController : Controller
{
    private Camera sceneCamera;

    private Actor player;

    public Actor getPlayer()
    {
        return player;
    }

    public void SetCamera(Camera camera)
    {
        sceneCamera = camera;
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
}
