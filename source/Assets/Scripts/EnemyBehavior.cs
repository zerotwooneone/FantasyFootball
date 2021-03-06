using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : HubParticipantBehavior
{
    public SearchStrategy SearchStrategy;

    public MoveStrategy MoveStrategy;
    public SpriteRenderer SpriteRenderer;
    public Rigidbody2D Rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var target = SearchStrategy.FindTarget();
        if (target != null)
        {
            MoveStrategy.Move(target.Value);
            bool flipX = Vector2.Angle(Rigidbody.velocity.normalized, Vector2.right) < 90;
            SpriteRenderer.flipX = flipX;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Publish(SubjectKeys.PlayerHealthChanged, PlayerHealthChangedArgs.Factory(delta: -10));
            // Debug.Log("contacted player");
            // Publish(SubjectKeys.Test, new TestArg());
        }
    }
}
