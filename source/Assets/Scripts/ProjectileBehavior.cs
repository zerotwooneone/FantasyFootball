using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float Speed = 20f;
    public Rigidbody2D Body;
    public Transform Target;
    public Collider2D Collider;
    // Start is called before the first frame update
    void Start()
    {
        var targetPosition = new Vector2(Target.position.x, Target.position.y);
        var targetDirection = (targetPosition - Body.position).normalized;
        Body.velocity = targetDirection * Speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
