using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float Speed = 20f;
    public int Damage = 10;
    public Rigidbody2D Body;
    public Transform Target;
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"other:{LayerMask.LayerToName(other.gameObject.layer)} this:{LayerMask.LayerToName(gameObject.layer)}");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return;
        }
        if (other.TryGetComponent<Damagable>(out var damagable))
        {
            damagable.Damage(Damage);
        }
        Debug.Log(other.name);
        Destroy(gameObject);
    }
}
