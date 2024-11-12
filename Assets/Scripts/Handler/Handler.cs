using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public abstract class Handler : MonoBehaviour
{
    public abstract Command GetCommand(GameActor actor);
}
