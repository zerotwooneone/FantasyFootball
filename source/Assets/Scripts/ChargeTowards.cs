using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeTowards : MoveStrategy
{
    public Rigidbody2D Body;
    public float Speed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Move(Vector2 target)
    {
        var pos = Body.transform.position;
        var pos2D = new Vector2(pos.x, pos.y);
        var direction = (target - pos2D).normalized;
        var force = direction * Speed;
        Body.AddForce(force, ForceMode2D.Impulse);
    }
}
