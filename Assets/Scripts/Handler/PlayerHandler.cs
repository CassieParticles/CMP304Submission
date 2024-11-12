using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : Handler
{
    [SerializeField] private InputAction moveAction;

    private void Awake()
    {
        moveAction.Enable();
    }


    public override Command GetCommand(GameActor actor)
    {
        Command command = null;
        //Use input to determine which command to send
        Vector2 direction = moveAction.ReadValue<Vector2>();

        if(direction!=Vector2.zero) 
        {
            MoveCommand moveCommand = new MoveCommand();
            moveCommand.moveDirection = direction;
            command = moveCommand;
        }

        return command;
    }
}
