using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SearchLineOfSight : SearchStrategy
{
    public GameObject Player;
    public Transform Self;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override Vector2? FindTarget()
    {
        var rayDirection = (Player.transform.position - Self.position);
        Color rayColor;
        var hit = Physics2D.Raycast(Self.position, rayDirection);
        if(hit.transform == Player.transform)
        {
            rayColor = Color.green;
            Debug.Log(Player.transform.position);
        }
        else
        {
            Debug.Log(hit.transform.position);
            rayColor = Color.red;
        }
        Debug.DrawRay(Self.position, rayDirection, rayColor);
        // var hits = Physics2D.BoxCastAll(Self.transform.position, new Vector2(40, 40), rayDirection);
        // if (hits.Length > 0)
        // {
        //     Debug.Log(hits.Select(h=>h.transform.ToString()));
        // }
        // else
        // {
        //     Debug.Log($"dir:{rayDirection} tar:{Player.transform.position} pos:{Self.position} hit:{hit.point}");
        // }

        return null;
    }
}
