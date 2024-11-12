using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActor : MonoBehaviour
{
    public void move(Vector2 moveDir)
    {
        rb.velocity = moveDir;
    }

    public void shoot(Vector2 shootDir)
    {

    }

    [SerializeField] private Handler handler;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(0,0);
        Command command = handler.GetCommand(this);
        if(command!=null)
        {
            command.execute(this);
        }
    }

}
