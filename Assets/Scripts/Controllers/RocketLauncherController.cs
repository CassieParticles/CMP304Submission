using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : Controller
{
    public override void DoActions(Actor actor)
    {
        actor.setMoveDirection(MoveEvents.StopMoving,Vector2.zero);
    }
}
