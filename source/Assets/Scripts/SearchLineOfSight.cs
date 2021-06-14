using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SearchLineOfSight : SearchStrategy
{
    public GameObject Player;
    public Transform Self;
    private Vector2 selfSize;
    public float MaxDistance = 20f;
    public float ChargeDelaySeconds = 1f;
    [CanBeNull] private Tuple<Vector2, float> lastSpotted;

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
        var hit = Physics2D.CapsuleCast(
            Self.position, 
            selfSize, 
            CapsuleDirection2D.Vertical,
            0f, 
            rayDirection,
            MaxDistance);

        var current = Time.timeSinceLevelLoad;
        var timeSinceLastSpotted = current - lastSpotted?.Item2;
        var waitElapsed = timeSinceLastSpotted.HasValue && timeSinceLastSpotted.Value > ChargeDelaySeconds;
        var spotted = hit.transform == Player.transform;
        if(spotted)
        {
            var spotTime = lastSpotted?.Item2 ?? current;
            lastSpotted = new Tuple<Vector2, float>(hit.transform.position, spotTime);
            if (waitElapsed)
            {
                return hit.transform.position;
            }
        }
        else
        {
            if (waitElapsed)
            {
                var position = lastSpotted != null
                    ? lastSpotted.Item1
                    : (Vector2?)null;
                lastSpotted = null;
                if (position.HasValue)
                {
                    var partialPosition = position.Value.normalized * 10;
                    return partialPosition;
                }
            }
        }
        return null;
    }
}
