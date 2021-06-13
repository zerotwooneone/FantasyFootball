using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public SearchStrategy SearchStrategy;

    public MoveStrategy MoveStrategy;
    
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
        }
    }
}
