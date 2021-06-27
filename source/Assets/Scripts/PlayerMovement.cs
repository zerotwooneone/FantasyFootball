using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : HubParticipantBehavior
{
    public Rigidbody2D body;
    public float speedFactor = 100f;
    private const float speedSanityFactor = 0.007f;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;

    private void OnTest(TestArg obj)
    {
        Debug.Log($"{nameof(PlayerMovement)} got test");
    }

    void Awake()
    {
        Debug.Log($"{nameof(PlayerMovement)} awake. about to subscribe");
        Subscribe<TestArg>(SubjectKeys.Test, OnTest);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = speedFactor * speedSanityFactor * Input.GetAxisRaw("Horizontal");
        
        var verticalInput = Input.GetAxisRaw("Vertical");
        verticalMove = speedFactor * speedSanityFactor * verticalInput;
    }
    private void FixedUpdate()
    {
        body.AddForce(new Vector2(horizontalMove, verticalMove), ForceMode2D.Impulse);
    }
}
