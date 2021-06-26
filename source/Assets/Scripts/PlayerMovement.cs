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

    protected override void Awake()
    {
        base.Awake();
        Subscribe<TestArg>(SubjectKeys.Test, OnTest);
    }

    private void OnTest(TestArg obj)
    {
        Debug.Log("got test");
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
