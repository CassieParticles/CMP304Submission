using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void execute(GameActor actor);
}

public class MoveCommand : Command
{ 
    public Vector2 moveDirection;
    public override void execute(GameActor actor)
    {
        actor.move(moveDirection);
    }
}

public class ShootCommand : Command
{
    public Vector2 shootDirection;
    public override void execute(GameActor actor)
    {
        actor.shoot(shootDirection);
    }
}
