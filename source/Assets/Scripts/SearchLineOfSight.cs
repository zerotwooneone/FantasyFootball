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
        if (Physics.Raycast(Self.position, rayDirection, out var hit))
        {
            Debug.Log(Player.transform.position);
        }
        Debug.DrawRay(Self.position, rayDirection, Color.red, 2f);
        var hits = Physics.BoxCastAll(Self.transform.position, new Vector3(40, 40, 1), rayDirection);
        if (hits.Length > 0)
        {
            Debug.Log(hits.Select(h=>h.transform.ToString()));
        }
        // else
        // {
        //     Debug.Log($"dir:{rayDirection} tar:{Player.transform.position} pos:{Self.position} hit:{hit.point}");
        // }

        return null;
    }
}
