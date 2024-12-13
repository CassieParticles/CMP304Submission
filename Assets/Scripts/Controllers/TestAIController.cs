using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIController : Controller
{
    public override void DoActions(Actor actor)
    {
        actor.setMoveDirection(MoveEvents.StopMoving, Vector2.zero);
        Actor target = actor.GetCurrentTarget();

        Vector2 moveDirection = (target.gameObject.transform.position - actor.transform.position).normalized;

        actor.setMoveDirection(MoveEvents.Move, moveDirection);
    }
}
