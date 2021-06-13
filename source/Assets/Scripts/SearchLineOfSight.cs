using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SearchLineOfSight : SearchStrategy
{
    public GameObject Player;
    public Transform Self;
    private Vector2 selfSize;
    public float MaxDistance = 20f;

    // Start is called before the first frame update
    void Start()
    {
        selfSize = (GetComponent<CapsuleCollider2D>() as CapsuleCollider2D).size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override Vector2? FindTarget()
    {
        var rayDirection = (Player.transform.position - Self.position);
        Color rayColor;
        var hit = Physics2D.CapsuleCast(
            Self.position, 
            selfSize, 
            CapsuleDirection2D.Vertical,
            0f, 
            rayDirection,
            MaxDistance);
        if(hit.transform == Player.transform)
        {
            rayColor = Color.green;
            return hit.transform.position;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(Self.position, rayDirection, rayColor);

        return null;
    }
}
